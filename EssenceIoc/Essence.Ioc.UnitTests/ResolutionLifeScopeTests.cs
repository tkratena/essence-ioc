using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Essence.Ioc
{
    [TestFixture]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class ResolutionLifeScopeTests
    {
        [Test]
        public void ContainerCreatesNewInstancesOfTransientService()
        {
            var container = new Container(r => 
                r.RegisterService<IService>().ImplementedBy<ServiceImplementation>());

            var firstInstance = container.Resolve<IService>();
            var secondInstance = container.Resolve<IService>();
            
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

            var spy = container.Resolve<SpyClassDependentTwiceOnService>();
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

            var spy = container.Resolve<SpyClassDependentOnServiceFactory>();
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

            var spy = container.Resolve<SpyClassDependentOnServiceFactoryDelegate>();
            var firstInstance = spy.Dependency.Invoke();
            var secondInstance = spy.Dependency.Invoke();
            
            Assert.AreNotSame(firstInstance, secondInstance);
        }

        [Test]
        public void ContainerProvidesSameInstanceOfSingletonService()
        {
            var container = new Container(r => 
                r.RegisterService<IService>().ImplementedBy<ServiceImplementation>().AsSingleton());

            var firstInstance = container.Resolve<IService>();
            var secondInstance = container.Resolve<IService>();
            
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

            var spy = container.Resolve<SpyClassDependentTwiceOnService>();
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

            var spy = container.Resolve<SpyClassDependentOnServiceFactory>();
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

            var spy = container.Resolve<SpyClassDependentOnServiceFactoryDelegate>();
            var firstInstance = spy.Dependency.Invoke();
            var secondInstance = spy.Dependency.Invoke();
            
            Assert.AreSame(firstInstance, secondInstance);
        }
        
        [Test]
        public void ContainerProvidesSameInstanceOfSingletonServiceCreatedByCustomFactory()
        {
            var container = new Container(r => 
                r.RegisterService<IService>().ConstructedBy(() => new ServiceImplementation()).AsSingleton());

            var firstInstance = container.Resolve<IService>();
            var secondInstance = container.Resolve<IService>();
            
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

            var spy = container.Resolve<SpyClassDependentTwiceOnService>();
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

            var spy = container.Resolve<SpyClassDependentOnServiceFactory>();
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

            var spy = container.Resolve<SpyClassDependentOnServiceFactoryDelegate>();
            var firstInstance = spy.Dependency.Invoke();
            var secondInstance = spy.Dependency.Invoke();
            
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
        
        private interface IService
        {
        }
    }
}