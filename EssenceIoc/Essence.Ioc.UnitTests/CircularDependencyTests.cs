using System;
using System.Diagnostics.CodeAnalysis;
using Essence.Ioc.Registration.RegistrationExceptions;
using NUnit.Framework;

namespace Essence.Ioc
{
    [TestFixture]
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class CircularDependencyTests
    {
        [TestFixture(typeof(ClassDependingOnSelf))]
        [TestFixture(typeof(ClassDependingOnLazySelf))]
        [TestFixture(typeof(ClassDependingOnSelfFactory))]
        public class RegisteringServiceImplementationWithDependencyOnSelfTests<T> where T : class, IService
        {
            [Test]
            public void RegisteringThrows()
            {
                TestDelegate when = () => new Container(r => 
                    r.RegisterService<IService>().ImplementedBy<T>());

                Assert.That(
                    when, 
                    Throws.Exception.InstanceOf<DependencyRegistrationException>().With.InnerException
                        .With.InstanceOf<NotRegisteredDependencyException>()
                        .And.Property(nameof(NotRegisteredDependencyException.DependencyType)).EqualTo(typeof(T)));
            }
            
            [Test]
            public void RegisteringAsSingletonThrows()
            {
                TestDelegate when = () => new Container(r => 
                    r.RegisterService<IService>().ImplementedBy<T>().AsSingleton());

                Assert.That(
                    when, 
                    Throws.Exception.InstanceOf<DependencyRegistrationException>().With.InnerException
                        .With.InstanceOf<NotRegisteredDependencyException>()
                        .And.Property(nameof(NotRegisteredDependencyException.DependencyType)).EqualTo(typeof(T)));
            }
        }
        
        [TestFixture(typeof(ClassDependingOnServiceItImplements))]
        [TestFixture(typeof(ClassDependingOnLazyServiceItImplements))]
        [TestFixture(typeof(ClassDependingOnServiceItImplementsFactory))]
        public class RegisteringServiceImplementationWithDependencyOnServiceItImplementeds<T> where T : class, IService
        {
            [Test]
            public void RegisteringThrows()
            {
                TestDelegate when = () => new Container(r => 
                    r.RegisterService<IService>().ImplementedBy<T>());

                Assert.That(
                    when, 
                    Throws.Exception.InstanceOf<DependencyRegistrationException>().With.InnerException
                        .With.InstanceOf<NotRegisteredDependencyException>()
                        .And.Property(nameof(NotRegisteredDependencyException.DependencyType)).EqualTo(typeof(IService)));
            }
        
            [Test]
            public void RegisteringAsSingletonThrows()
            {
                TestDelegate when = () => new Container(r => 
                    r.RegisterService<IService>().ImplementedBy<T>().AsSingleton());

                Assert.That(
                    when, 
                    Throws.Exception.InstanceOf<DependencyRegistrationException>().With.InnerException
                        .With.InstanceOf<NotRegisteredDependencyException>()
                        .And.Property(nameof(NotRegisteredDependencyException.DependencyType)).EqualTo(typeof(IService)));
            }
        }
        
        [Test]
        public void RegisteringServiceImplementationDependentOnSelfByItsDependencyThrows()
        {
            TestDelegate when = () => new Container(r =>
            {
                r.RegisterService<IServiceA>().ImplementedBy<ServiceAImplementationDependingOnServiceB>();
                r.RegisterService<IServiceB>().ImplementedBy<ServiceBImplementationDependingOnServiceA>();
            });

            Assert.That(
                when, 
                Throws.Exception.InstanceOf<DependencyRegistrationException>().With.InnerException
                    .With.InstanceOf<NotRegisteredDependencyException>());
        }
        
        private class ClassDependingOnSelf : IService
        {
            public ClassDependingOnSelf(ClassDependingOnSelf circularDependency)
            {
            }
        }
        
        private class ClassDependingOnLazySelf : IService
        {
            public ClassDependingOnLazySelf(Lazy<ClassDependingOnLazySelf> circularDependency)
            {
            }
        }
        
        private class ClassDependingOnSelfFactory : IService
        {
            public ClassDependingOnSelfFactory(Func<ClassDependingOnSelfFactory> circularDependency)
            {
            }
        }
        
        private class ClassDependingOnServiceItImplements : IService
        {
            public ClassDependingOnServiceItImplements(IService circularDependency)
            {
            }
        }
        
        private class ClassDependingOnLazyServiceItImplements : IService
        {
            public ClassDependingOnLazyServiceItImplements(Lazy<IService> circularDependency)
            {
            }
        }
        
        private class ClassDependingOnServiceItImplementsFactory : IService
        {
            public ClassDependingOnServiceItImplementsFactory(Func<IService> circularDependency)
            {
            }
        }
        
        public interface IService
        {
        }
        
        private class ServiceAImplementationDependingOnServiceB : IServiceA
        {
            public ServiceAImplementationDependingOnServiceB(IServiceB circularDependency)
            {
            }
        }

        private class ServiceBImplementationDependingOnServiceA : IServiceB
        {
            public ServiceBImplementationDependingOnServiceA(IServiceA circularDependency)
            {
            }
        }
        
        private interface IServiceA
        {
        }

        private interface IServiceB
        {
        }
    }
}