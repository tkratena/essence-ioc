using System;
using System.Diagnostics.CodeAnalysis;
using Essence.Ioc.FluentRegistration;
using NUnit.Framework;

namespace Essence.Ioc.Resolution
{
    [TestFixture]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    public class DependencyResolutionTests
    {
        [Test]
        public void ClassDependentOnService()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IServiceDependency>().ImplementedBy<DependencyImplementation>();
                r.RegisterService<IService>().ImplementedBy<SpyServiceImplementationDependentOnService>();
            });

            container.Resolve<IService>(out var service);

            Assert.IsInstanceOf<SpyServiceImplementationDependentOnService>(service);
            var dependency = ((SpyServiceImplementationDependentOnService) service).Dependency;
            Assert.IsInstanceOf<DependencyImplementation>(dependency);
        }

        [Test]
        public void ClassDependentOnServiceConstructedByCustomFactory()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IServiceDependency>().ConstructedBy(() => new DependencyImplementation());
                r.RegisterService<IService>().ImplementedBy<SpyServiceImplementationDependentOnService>();
            });

            container.Resolve<IService>(out var service);

            Assert.IsInstanceOf<SpyServiceImplementationDependentOnService>(service);
            var dependency = ((SpyServiceImplementationDependentOnService) service).Dependency;
            Assert.IsInstanceOf<DependencyImplementation>(dependency);
        }

        private class SpyServiceImplementationDependentOnService : IService
        {
            public IServiceDependency Dependency { get; }

            public SpyServiceImplementationDependentOnService(IServiceDependency dependency)
            {
                Dependency = dependency;
            }
        }

        [Test]
        public void ClassDependentOnServiceFactory()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IServiceDependency>().ImplementedBy<DependencyImplementation>();
                r.RegisterService<IService>().ImplementedBy<SpyServiceImplementationDependentOnServiceFactory>();
            });

            container.Resolve<IService>(out var service);

            Assert.IsInstanceOf<SpyServiceImplementationDependentOnServiceFactory>(service);
            var dependency = ((SpyServiceImplementationDependentOnServiceFactory) service).Dependency;
            Assert.IsInstanceOf<DependencyImplementation>(dependency.Invoke());
        }

        private class SpyServiceImplementationDependentOnServiceFactory : IService
        {
            public Func<IServiceDependency> Dependency { get; }

            public SpyServiceImplementationDependentOnServiceFactory(Func<IServiceDependency> dependency)
            {
                Dependency = dependency;
            }
        }

        [Test]
        public void ClassDependentOnServiceFactoryDelegate()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IServiceDependency>()
                    .ImplementedBy<DependencyImplementation>();
                r.RegisterService<IService>()
                    .ImplementedBy<SpyServiceImplementationDependentOnServiceFactoryDelegate>();
            });

            container.Resolve<IService>(out var service);

            Assert.IsInstanceOf<SpyServiceImplementationDependentOnServiceFactoryDelegate>(service);
            var dependency = ((SpyServiceImplementationDependentOnServiceFactoryDelegate) service).Dependency;
            Assert.IsInstanceOf<DependencyImplementation>(dependency.Invoke());
        }

        private class SpyServiceImplementationDependentOnServiceFactoryDelegate : IService
        {
            public delegate IServiceDependency DependencyFactory();

            public DependencyFactory Dependency { get; }

            public SpyServiceImplementationDependentOnServiceFactoryDelegate(DependencyFactory dependency)
            {
                Dependency = dependency;
            }
        }
        
        [Test]
        public void ClassDependentOnGenericServiceFactoryDelegate()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IServiceDependency>()
                    .ImplementedBy<DependencyImplementation>();
                r.RegisterService<IService>()
                    .ImplementedBy<SpyServiceImplementationDependentOnGenericServiceFactoryDelegate>();
            });

            container.Resolve<IService>(out var service);

            Assert.IsInstanceOf<SpyServiceImplementationDependentOnGenericServiceFactoryDelegate>(service);
            var dependency = ((SpyServiceImplementationDependentOnGenericServiceFactoryDelegate) service).Dependency;
            Assert.IsInstanceOf<DependencyImplementation>(dependency.Invoke());
        }
        
        private class SpyServiceImplementationDependentOnGenericServiceFactoryDelegate : IService
        {
            public delegate T DependencyFactory<out T>();

            public DependencyFactory<IServiceDependency> Dependency { get; }

            public SpyServiceImplementationDependentOnGenericServiceFactoryDelegate(
                DependencyFactory<IServiceDependency> dependency)
            {
                Dependency = dependency;
            }
        }

        [Test]
        public void ClassDependentOnLazyService()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IServiceDependency>().ImplementedBy<DependencyImplementation>();
                r.RegisterService<IService>().ImplementedBy<SpyServiceImplementationDependentOnLazyService>();
            });

            container.Resolve<IService>(out var service);

            Assert.IsInstanceOf<SpyServiceImplementationDependentOnLazyService>(service);
            var dependency = ((SpyServiceImplementationDependentOnLazyService) service).Dependency;
            Assert.IsFalse(dependency.IsValueCreated);
            Assert.IsInstanceOf<DependencyImplementation>(dependency.Value);
        }

        private class SpyServiceImplementationDependentOnLazyService : IService
        {
            public Lazy<IServiceDependency> Dependency { get; }

            public SpyServiceImplementationDependentOnLazyService(Lazy<IServiceDependency> dependency)
            {
                Dependency = dependency;
            }
        }

        private class DependencyImplementation : IServiceDependency
        {
        }

        private interface IServiceDependency
        {
        }

        [Test]
        public void ClassDependentOnGenericService()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IService<IActualGenericArg>>()
                    .ImplementedBy<ServiceImplementation<IActualGenericArg>>();
                r.RegisterService<IService>().ImplementedBy<SpyServiceImplementationDependentOnGenericService>();
            });

            container.Resolve<IService>(out var service);

            Assert.IsInstanceOf<SpyServiceImplementationDependentOnGenericService>(service);
            var dependency = ((SpyServiceImplementationDependentOnGenericService) service).Dependency;
            Assert.IsInstanceOf<ServiceImplementation<IActualGenericArg>>(dependency);
        }

        [Test]
        public void ClassDependentOnGenericallyRegisteredGenericService()
        {
            var container = new Container(r =>
            {
                r.GenericallyRegisterService(typeof(IService<>)).ImplementedBy(typeof(ServiceImplementation<>));
                r.RegisterService<IService>().ImplementedBy<SpyServiceImplementationDependentOnGenericService>();
            });

            container.Resolve<IService>(out var service);

            Assert.IsInstanceOf<SpyServiceImplementationDependentOnGenericService>(service);
            var dependency = ((SpyServiceImplementationDependentOnGenericService) service).Dependency;
            Assert.IsInstanceOf<ServiceImplementation<IActualGenericArg>>(dependency);
        }

        private class SpyServiceImplementationDependentOnGenericService : IService
        {
            public IService<IActualGenericArg> Dependency { get; }

            public SpyServiceImplementationDependentOnGenericService(IService<IActualGenericArg> dependency)
            {
                Dependency = dependency;
            }
        }

        [Test]
        public void ServiceConstructedByCustomFactory()
        {
            var container = new Container(r =>
                r.RegisterService<IService>().ConstructedBy(() => new ServiceImplementation()));

            container.Resolve<IService>(out var service);

            Assert.IsInstanceOf<ServiceImplementation>(service);
        }
        
        [Test]
        public void ServiceConstructedByCustomFactoryThatUsesContainer()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IServiceDependency>().ImplementedBy<DependencyImplementation>();
                r.RegisterService<IService>()
                    .ConstructedBy(c => new SpyServiceImplementationDependentOnService(c.Resolve<IServiceDependency>()));
            });

            container.Resolve<IService>(out var service);

            Assert.IsInstanceOf<SpyServiceImplementationDependentOnService>(service);
            var dependency = ((SpyServiceImplementationDependentOnService) service).Dependency;
            Assert.IsInstanceOf<DependencyImplementation>(dependency);
        }

        private class ServiceImplementation : IService
        {
        }

        [Test]
        public void GenericallyRegisteredGenericServiceWithDependencyOfTheGenericArgumentType()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IService>()
                    .ImplementedBy<ServiceImplementation>();
                r.GenericallyRegisterService(typeof(IService<>))
                    .ImplementedBy(typeof(ServiceImplementationDependentOn<>));
            });

            container.Resolve<IService<IService>>(out var service);

            Assert.IsInstanceOf<ServiceImplementationDependentOn<IService>>(service);
        }

        private class ServiceImplementationDependentOn<TDependency> : IService<TDependency>
            where TDependency : IService
        {
            public ServiceImplementationDependentOn(TDependency dependency)
            {
            }
        }

        [Test]
        public void SubsequentlyRegisteredServiceWithDifferentGenericParameter()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IService<IActualGenericArg>>()
                    .ImplementedBy<ServiceImplementation<IActualGenericArg>>();
                r.RegisterService<IService<IAnotherActualGenericArg>>()
                    .ImplementedBy<ServiceImplementation<IAnotherActualGenericArg>>();
            });

            container.Resolve<IService<IAnotherActualGenericArg>>(out var service);

            Assert.IsInstanceOf<ServiceImplementation<IAnotherActualGenericArg>>(service);
        }

        private interface IAnotherActualGenericArg
        {
        }

        [Test]
        public void ServiceRegisteredGenericallyAndNonGenericallyAtTheSameTime()
        {
            var container = new Container(r =>
            {
                r.GenericallyRegisterService(typeof(IService<>))
                    .ImplementedBy(typeof(ServiceImplementation<>));
                r.RegisterService<IService<INonGenericallyRegisteredGenericArg>>()
                    .ImplementedBy<NonGenericallyRegisteredGenericServiceImplementation>();
            });

            container.Resolve<IService<IActualGenericArg>>(out var genericallyRegistered);
            container.Resolve<IService<INonGenericallyRegisteredGenericArg>>(out var nonGenericallyRegistered);

            Assert.IsInstanceOf<ServiceImplementation<IActualGenericArg>>(genericallyRegistered);
            Assert.IsInstanceOf<NonGenericallyRegisteredGenericServiceImplementation>(nonGenericallyRegistered);
        }
        
        [SuppressMessage("ReSharper", "UnusedTypeParameter")]
        private interface IService<T>
        {
        }

        private interface IActualGenericArg
        {
        }

        private class ServiceImplementation<T> : IService<T>
        {
        }

        private interface INonGenericallyRegisteredGenericArg
        {
        }

        private class NonGenericallyRegisteredGenericServiceImplementation
            : IService<INonGenericallyRegisteredGenericArg>
        {
        }
        
        [Test]
        [SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
        public void ServiceCanBeResolvedAfterDependencyOfItsImplementationHasBeenResolved()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IServiceDependency>().ImplementedBy<DependencyImplementation>();
                r.RegisterService<IService>().ImplementedBy<ServiceImplementationDependentOnService>();
            });

            container.Resolve<IServiceDependency>(out _);
            container.Resolve<IService>(out var service);

            Assert.IsInstanceOf<ServiceImplementationDependentOnService>(service);
        }

        private class ServiceImplementationDependentOnService : IService
        {
            public ServiceImplementationDependentOnService(IServiceDependency dependency)
            {
            }
        }

        private interface IService
        {
        }
    }
}