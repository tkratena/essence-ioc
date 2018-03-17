using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Essence.Ioc
{
    [TestFixture]
    public class PerformanceTests
    {
        [TestFixture]
        public class RegistrationPerformanceTests
        {
            private const int TryCount = 1_000;
            
            [Test]
            public void RegisteringToContainerIsNegligible()
            {
                var containerStopWatch = new Stopwatch();

                containerStopWatch.Start();
                for (var i = 0; i < TryCount; i++)
                {
                    CreateContainerWithRegisteredServices();
                }
                containerStopWatch.Stop();

                var milliseconds = containerStopWatch.ElapsedMilliseconds / (double)TryCount;
                Assert.Less(milliseconds, 0.5);
            }
        }

        [TestFixture]
        public class ResolutionPerformanceTests
        {
            private const int TryCount = 5_000_000;
            private const int TryCountChunk = 1_000;
            
            [SetUp]
            public void WarmUp()
            {
                var container = CreateContainerWithRegisteredServices();
                for (var i = 0; i < TryCount; i++)
                {
                    new RootServiceImplementation(CreateTestServiceDependencyManually()).Use();
                    container.Resolve<IRootService>().Use();
                }
            }
        
            [Test]
            [Explicit]
            public void CreatingAnInstanceByContainerIsComparablyFastAsInjectingDependenciesManually()
            {
                var manualInjectionStopWatch = new Stopwatch();
                var containerStopWatch = new Stopwatch();

                containerStopWatch.Start();
                var container = CreateContainerWithRegisteredServices();
                containerStopWatch.Stop();

                for (var i = 0; i < TryCount / TryCountChunk; i++)
                {
                    IRootService instance;
                    
                    manualInjectionStopWatch.Start();
                    for (var j = 0; j < TryCountChunk; j++)
                    {
                        instance = new RootServiceImplementation(CreateTestServiceDependencyManually());
                        instance.Use();
                    }
                    manualInjectionStopWatch.Stop();

                    containerStopWatch.Start();
                    for (var j = 0; j < TryCountChunk; j++)
                    {
                        instance = container.Resolve<IRootService>();
                        instance.Use();
                    }
                    containerStopWatch.Stop();
                }

                var manualInjectionDuration = manualInjectionStopWatch.ElapsedMilliseconds;
                var containerDuration = containerStopWatch.ElapsedMilliseconds;

                var containerPerformancePercentage = (double) containerDuration / manualInjectionDuration * 100;
                TestContext.WriteLine("Container performance: {0:0}%", containerPerformancePercentage);

                Assert.Less(containerPerformancePercentage, 200);
            }
            
            private static IRootServiceDependency CreateTestServiceDependencyManually()
            {
                var terminalServiceSingleton = new TerminalService();
                
                return new RootServiceDependency(
                    new Lazy<ILazyService>(() => new LazyService(terminalServiceSingleton)),
                    () => new FactoryService(terminalServiceSingleton),
                    new ConcreteService(terminalServiceSingleton),
                    new CustomService(1, terminalServiceSingleton),
                    new GenericService<object>(terminalServiceSingleton));
            }
        }

        private static IContainer CreateContainerWithRegisteredServices()
        {
            var container = new Container(r =>
            {
                r.RegisterService<ITerminalService>().ImplementedBy<TerminalService>().AsSingleton();
                r.GenericlyRegisterService(typeof(IGenericService<>)).ImplementedBy(typeof(GenericService<>));
                r.RegisterService<ICustomService>()
                    .ConstructedBy(c => new CustomService(1, c.Resolve<ITerminalService>()));
                r.RegisterService<ConcreteService>().ImplementedBy<ConcreteService>();
                r.RegisterService<IFactoryService>().ImplementedBy<FactoryService>();
                r.RegisterService<ILazyService>().ImplementedBy<LazyService>();
                r.RegisterService<IRootServiceDependency>().ImplementedBy<RootServiceDependency>();
                r.RegisterService<IRootService>().ImplementedBy<RootServiceImplementation>();
            });
            
            return container;
        }
    }

    public interface ILazyService : IUsableService
    {
    }

    public interface IFactoryService : IUsableService
    {
    }
    
    public interface ICustomService : IUsableService
    {
    }

    [SuppressMessage("ReSharper", "UnusedTypeParameter")]
    public interface IGenericService<T> : IUsableService
    {
    }

    public interface IRootServiceDependency : IUsableService
    {
    }

    public interface IRootService : IUsableService
    {
    }

    public interface ITerminalService : IUsableService
    {
    }

    public interface IUsableService
    {
        void Use();
    }

    public class RootServiceImplementation : IRootService
    {
        private readonly IRootServiceDependency _dependency;

        public RootServiceImplementation(IRootServiceDependency dependency)
        {
            _dependency = dependency;
        }

        public void Use()
        {
            _dependency.Use();
        }
    }

    public class RootServiceDependency : IRootServiceDependency
    {
        private readonly Lazy<ILazyService> _lazyService;
        private readonly Func<IFactoryService> _factoryService;
        private readonly ConcreteService _concreteService;
        private readonly ICustomService _customService;
        private readonly IGenericService<object> _genericService;

        public RootServiceDependency(
            Lazy<ILazyService> lazyService,
            Func<IFactoryService> factoryService,
            ConcreteService concreteService,
            ICustomService customService,
            IGenericService<object> genericService)
        {
            _lazyService = lazyService;
            _factoryService = factoryService;
            _concreteService = concreteService;
            _customService = customService;
            _genericService = genericService;
        }

        public void Use()
        {
            _lazyService.Value.Use();

            for (var i = 0; i < 2; i++)
            {
                _factoryService.Invoke().Use();
            }

            _concreteService.Use();
            _customService.Use();
            _genericService.Use();
        }
    }

    public class LazyService : ILazyService
    {
        private readonly ITerminalService _terminalService;

        public LazyService(ITerminalService terminalService)
        {
            _terminalService = terminalService;
        }

        public void Use()
        {
            _terminalService.Use();
        }
    }

    public class FactoryService : IFactoryService
    {
        private readonly ITerminalService _terminalService;

        public FactoryService(ITerminalService terminalService)
        {
            _terminalService = terminalService;
        }

        public void Use()
        {
            _terminalService.Use();
        }
    }

    public class ConcreteService : IUsableService
    {
        private readonly ITerminalService _terminalService;

        public ConcreteService(ITerminalService terminalService)
        {
            _terminalService = terminalService;
        }

        public void Use()
        {
            _terminalService.Use();
        }
    }

    public class CustomService : ICustomService
    {
        private readonly int _notInjectedParameter;
        private readonly ITerminalService _terminalService;

        public CustomService(int notInjectedParameter, ITerminalService terminalService)
        {
            _notInjectedParameter = notInjectedParameter;
            _terminalService = terminalService;
        }

        public void Use()
        {
            for (var i = 0; i < _notInjectedParameter; i++)
            {
                _terminalService.Use();
            }
        }
    }

    public class GenericService<T> : IGenericService<T>
    {
        private readonly ITerminalService _terminalService;

        public GenericService(ITerminalService terminalService)
        {
            _terminalService = terminalService;
        }

        public void Use()
        {
            _terminalService.Use();
        }
    }

    public class TerminalService : ITerminalService
    {
        public void Use()
        {
        }
    }
}