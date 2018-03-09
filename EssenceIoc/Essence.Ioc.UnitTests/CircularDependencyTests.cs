using System;
using System.Diagnostics.CodeAnalysis;
using Essence.Ioc.Registration.RegistrationExceptions;
using NUnit.Framework;
using TestCaseAttribute = Essence.TestFramework.TestCaseAttribute;

namespace Essence.Ioc
{
    [TestFixture]
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class CircularDependencyTests
    {
        [Test]
        [TestCase(Generic = typeof(ClassDependingOnSelf))]
        [TestCase(Generic = typeof(ClassDependingOnLazySelf))]
        [TestCase(Generic = typeof(ClassDependingOnSelfFactory))]
        public void RegisteringClassWithDependencyOnSelfThrows<T>() where T : class, IService
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
        [TestCase(Generic = typeof(ClassDependingOnServiceItImplements))]
        [TestCase(Generic = typeof(ClassDependingOnLazyServiceItImplements))]
        [TestCase(Generic = typeof(ClassDependingOnServiceItImplementsFactory))]
        public void RegisteringClassWithDependencyOnServiceItImplementsThrows<T>() where T : class, IService
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
        [TestCase(Generic = typeof(ClassDependingOnSelf))]
        [TestCase(Generic = typeof(ClassDependingOnLazySelf))]
        [TestCase(Generic = typeof(ClassDependingOnSelfFactory))]
        public void RegisteringClassWithDependencyOnSelfAsSingletonThrows<T>() where T : class, IService
        {
            TestDelegate when = () => new Container(r => 
                r.RegisterService<IService>().ImplementedBy<T>().AsSingleton());

            Assert.That(
                when, 
                Throws.Exception.InstanceOf<DependencyRegistrationException>().With.InnerException
                    .With.InstanceOf<NotRegisteredDependencyException>()
                    .And.Property(nameof(NotRegisteredDependencyException.DependencyType)).EqualTo(typeof(T)));
        }
        
        [Test]
        [TestCase(Generic = typeof(ClassDependingOnServiceItImplements))]
        [TestCase(Generic = typeof(ClassDependingOnLazyServiceItImplements))]
        [TestCase(Generic = typeof(ClassDependingOnServiceItImplementsFactory))]
        public void RegisteringClassWithDependencyOnServiceItImplementsAsSingletonThrows<T>() where T : class, IService
        {
            TestDelegate when = () => new Container(r => 
                r.RegisterService<IService>().ImplementedBy<T>().AsSingleton());

            Assert.That(
                when, 
                Throws.Exception.InstanceOf<DependencyRegistrationException>().With.InnerException
                    .With.InstanceOf<NotRegisteredDependencyException>()
                    .And.Property(nameof(NotRegisteredDependencyException.DependencyType)).EqualTo(typeof(IService)));
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
        
        [Test]
        public void RegisteringClassDependentOnSelfByItsDependencyThrows()
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