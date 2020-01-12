using System;
using System.Diagnostics.CodeAnalysis;
using Essence.Ioc.FluentRegistration;
using NUnit.Framework;

namespace Essence.Ioc.Resolution
{
    [TestFixture]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class ResolutionLifeStyleTests
    {
        [Test]
        public void ContainerCreatesNewInstancesOfTransientService()
        {
            var container = new Container(r =>
                r.RegisterService<IService>().ImplementedBy<ServiceImplementation>());

            container.Resolve<IService>(out var firstInstance);
            container.Resolve<IService>(out var secondInstance);

            Assert.AreNotSame(firstInstance, secondInstance);
        }

        [Test]
        public void ContainerCreatesNewInstancesOfTransientDependency()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IService>()
                    .ImplementedBy<ServiceImplementation>();
                r.RegisterService<SpyClassDependentTwiceOnService>()
                    .ImplementedBy<SpyClassDependentTwiceOnService>();
            });

            container.Resolve<SpyClassDependentTwiceOnService>(out var spy);

            var firstInstance = spy.FirstDependency;
            var secondInstance = spy.SecondDependency;
            Assert.AreNotSame(firstInstance, secondInstance);
        }

        [Test]
        public void ServiceFactoryDependencyCreatesNewInstancesOfTransientService()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IService>()
                    .ImplementedBy<ServiceImplementation>();
                r.RegisterService<SpyClassDependentOnServiceFactory>()
                    .ImplementedBy<SpyClassDependentOnServiceFactory>();
            });

            container.Resolve<SpyClassDependentOnServiceFactory>(out var spy);

            var firstInstance = spy.Dependency.Invoke();
            var secondInstance = spy.Dependency.Invoke();
            Assert.AreNotSame(firstInstance, secondInstance);
        }

        [Test]
        public void ServiceFactoryDelegateDependencyCreatesNewInstancesOfTransientService()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IService>()
                    .ImplementedBy<ServiceImplementation>();
                r.RegisterService<SpyClassDependentOnServiceFactoryDelegate>()
                    .ImplementedBy<SpyClassDependentOnServiceFactoryDelegate>();
            });

            container.Resolve<SpyClassDependentOnServiceFactoryDelegate>(out var spy);

            var firstInstance = spy.Dependency.Invoke();
            var secondInstance = spy.Dependency.Invoke();
            Assert.AreNotSame(firstInstance, secondInstance);
        }

        [Test]
        public void ContainerProvidesSameInstanceOfSingletonService()
        {
            var container = new Container(r =>
                r.RegisterService<IService>().ImplementedBy<ServiceImplementation>().AsSingleton());

            container.Resolve<IService>(out var firstInstance);
            container.Resolve<IService>(out var secondInstance);

            Assert.AreSame(firstInstance, secondInstance);
        }

        [Test]
        public void ContainerProvidesSameInstanceOfSingletonDependency()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IService>()
                    .ImplementedBy<ServiceImplementation>().AsSingleton();
                r.RegisterService<SpyClassDependentTwiceOnService>()
                    .ImplementedBy<SpyClassDependentTwiceOnService>();
            });

            container.Resolve<SpyClassDependentTwiceOnService>(out var spy);

            var firstInstance = spy.FirstDependency;
            var secondInstance = spy.SecondDependency;
            Assert.AreSame(firstInstance, secondInstance);
        }

        [Test]
        public void ServiceFactoryDependencyProvidesSameInstanceOfSingletonService()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IService>()
                    .ImplementedBy<ServiceImplementation>().AsSingleton();
                r.RegisterService<SpyClassDependentOnServiceFactory>()
                    .ImplementedBy<SpyClassDependentOnServiceFactory>();
            });

            container.Resolve<SpyClassDependentOnServiceFactory>(out var spy);

            var firstInstance = spy.Dependency.Invoke();
            var secondInstance = spy.Dependency.Invoke();
            Assert.AreSame(firstInstance, secondInstance);
        }

        [Test]
        public void ServiceFactoryDelegateDependencyProvidesSameInstanceOfSingletonService()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IService>()
                    .ImplementedBy<ServiceImplementation>().AsSingleton();
                r.RegisterService<SpyClassDependentOnServiceFactoryDelegate>()
                    .ImplementedBy<SpyClassDependentOnServiceFactoryDelegate>();
            });

            container.Resolve<SpyClassDependentOnServiceFactoryDelegate>(out var spy);

            var firstInstance = spy.Dependency.Invoke();
            var secondInstance = spy.Dependency.Invoke();
            Assert.AreSame(firstInstance, secondInstance);
        }

        [Test]
        public void ContainerProvidesSameInstanceOfSingletonServiceCreatedByCustomFactory()
        {
            var container = new Container(r =>
                r.RegisterService<IService>().ConstructedBy(() => new ServiceImplementation()).AsSingleton());

            container.Resolve<IService>(out var instance);
            var firstInstance = instance;
            container.Resolve<IService>(out var instance1);
            var secondInstance = instance1;

            Assert.AreSame(firstInstance, secondInstance);
        }

        [Test]
        public void ContainerProvidesSameInstanceOfSingletonDependencyCreatedByCustomFactory()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IService>()
                    .ConstructedBy(() => new ServiceImplementation()).AsSingleton();
                r.RegisterService<SpyClassDependentTwiceOnService>()
                    .ImplementedBy<SpyClassDependentTwiceOnService>();
            });

            container.Resolve<SpyClassDependentTwiceOnService>(out var spy);

            var firstInstance = spy.FirstDependency;
            var secondInstance = spy.SecondDependency;
            Assert.AreSame(firstInstance, secondInstance);
        }

        [Test]
        public void ServiceFactoryDependencyProvidesSameInstanceOfSingletonServiceCreatedByCustomFactory()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IService>()
                    .ConstructedBy(() => new ServiceImplementation()).AsSingleton();
                r.RegisterService<SpyClassDependentOnServiceFactory>()
                    .ImplementedBy<SpyClassDependentOnServiceFactory>();
            });

            container.Resolve<SpyClassDependentOnServiceFactory>(out var spy);

            var firstInstance = spy.Dependency.Invoke();
            var secondInstance = spy.Dependency.Invoke();
            Assert.AreSame(firstInstance, secondInstance);
        }

        [Test]
        public void ServiceFactoryDelegateDependencyProvidesSameInstanceOfSingletonServiceCreatedByCustomFactory()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IService>()
                    .ConstructedBy(() => new ServiceImplementation()).AsSingleton();
                r.RegisterService<SpyClassDependentOnServiceFactoryDelegate>()
                    .ImplementedBy<SpyClassDependentOnServiceFactoryDelegate>();
            });

            container.Resolve<SpyClassDependentOnServiceFactoryDelegate>(out var spy);

            var firstInstance = spy.Dependency.Invoke();
            var secondInstance = spy.Dependency.Invoke();
            Assert.AreSame(firstInstance, secondInstance);
        }

        [Test]
        public void ContainerProvidesSameInstanceOfSingletonDependencyConstructedByCustomFactoryUsingContainer()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IServiceDependency>().ImplementedBy<DependencyImplementation>();
                r.RegisterService<IService>()
                    .ConstructedBy(c => new ServiceImplementationDependentOnService(c.Resolve<IServiceDependency>()))
                    .AsSingleton();
                r.RegisterService<SpyClassDependentTwiceOnService>()
                    .ImplementedBy<SpyClassDependentTwiceOnService>();
            });

            container.Resolve<SpyClassDependentTwiceOnService>(out var spy);

            var firstInstance = spy.FirstDependency;
            var secondInstance = spy.SecondDependency;
            Assert.AreSame(firstInstance, secondInstance);
        }

        private class ServiceImplementation : IService
        {
        }

        private class SpyClassDependentTwiceOnService
        {
            public IService FirstDependency { get; }
            public IService SecondDependency { get; }

            public SpyClassDependentTwiceOnService(IService firstDependency, IService secondDependency)
            {
                FirstDependency = firstDependency;
                SecondDependency = secondDependency;
            }
        }

        private class SpyClassDependentOnServiceFactory
        {
            public Func<IService> Dependency { get; }

            public SpyClassDependentOnServiceFactory(Func<IService> dependency)
            {
                Dependency = dependency;
            }
        }

        private class SpyClassDependentOnServiceFactoryDelegate
        {
            public delegate IService ServiceFactory();

            public ServiceFactory Dependency { get; }

            public SpyClassDependentOnServiceFactoryDelegate(ServiceFactory dependency)
            {
                Dependency = dependency;
            }
        }

        private class ServiceImplementationDependentOnService : IService
        {
            [SuppressMessage("ReSharper", "UnusedParameter.Local")]
            public ServiceImplementationDependentOnService(IServiceDependency dependency)
            {
            }
        }

        private class DependencyImplementation : IServiceDependency
        {
        }

        private interface IService
        {
        }

        private interface IServiceDependency
        {
        }
    }
}