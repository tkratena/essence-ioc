using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Essence.Ioc.FluentRegistration;
using Essence.Ioc.Registration.RegistrationExceptions;
using Essence.Ioc.Resolution;
using NUnit.Framework;
using TestCaseAttribute = Essence.TestFramework.TestCaseAttribute;

namespace Essence.Ioc
{
    [TestFixture]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    [SuppressMessage("ReSharper", "EmptyConstructor")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    public class InvalidRegistrationTests
    {
        [Test]
        [TestCase(typeof(NoConstructorException), Generic = typeof(ClassWithPrivateConstructor))]
        [TestCase(typeof(NoConstructorException), Generic = typeof(ClassWithInternalConstructor))]
        [TestCase(typeof(AmbiguousConstructorsException), Generic = typeof(ClassWithMultipleConstructors))]
        [TestCase(typeof(NonConcreteClassException), Generic = typeof(AbstractClass))]
        [TestCase(typeof(NonConcreteClassException), Generic = typeof(IInterface))]
        [TestCase(typeof(DisposableClassException), Generic = typeof(DisposableClass))]
        [TestCase(typeof(UnsupportedConstructorParametersException), Generic = typeof(ClassWithRefConstructorParameter))]
        [TestCase(typeof(UnsupportedConstructorParametersException), Generic = typeof(ClassWithOutConstructorParameter))]
        [TestCase(typeof(UnsupportedConstructorParametersException), Generic = typeof(ClassWithOptionalConstructorParameter))]
        public void RegisteringInvalidServiceImplementationThrows<T>(Type expectedExceptionType)
            where T : class, IService
        {
            TestDelegate when = () => new Container(r => 
                r.RegisterService<IService>().ImplementedBy<T>());

            Assert.That(when, Throws.Exception.InstanceOf(expectedExceptionType));
        }
        
        [Test]
        [TestCase(typeof(NonFactoryDelegateException), Generic = typeof(ClassDependingOnDelegateWithNoReturnType))]
        [TestCase(typeof(NonFactoryDelegateException), Generic = typeof(ClassDependingOnDelegateWithParameters))]
        [TestCase(typeof(NonFactoryDelegateException), Generic = typeof(ClassDependingOnDelegateWithParametersAndNoReturnType))]
        [TestCase(typeof(NotRegisteredDependencyException), Generic = typeof(ClassDependingOnServiceNotRegisteredYet))]
        public void RegisteringServiceImplementationWithInvalidDependencyThrows<T>(Type expectedInnerExceptionType)
            where T : class, IService
        {
            TestDelegate when = () => new Container(r => 
                r.RegisterService<IService>().ImplementedBy<T>());

            Assert.That(
                when,
                Throws.Exception.InstanceOf<DependencyRegistrationException>()
                    .With.InnerException.InstanceOf(expectedInnerExceptionType));
        }

        [Test]
        [TestCase(typeof(NoConstructorException), Generic = typeof(ClassWithPrivateConstructor))]
        [TestCase(typeof(NoConstructorException), Generic = typeof(ClassWithInternalConstructor))]
        [TestCase(typeof(AmbiguousConstructorsException), Generic = typeof(ClassWithMultipleConstructors))]
        [TestCase(typeof(NonConcreteClassException), Generic = typeof(AbstractClass))]
        [TestCase(typeof(NonConcreteClassException), Generic = typeof(IInterface))]
        [TestCase(typeof(DisposableClassException), Generic = typeof(DisposableClass))]
        [TestCase(typeof(UnsupportedConstructorParametersException), Generic = typeof(ClassWithRefConstructorParameter))]
        [TestCase(typeof(UnsupportedConstructorParametersException), Generic = typeof(ClassWithOutConstructorParameter))]
        [TestCase(typeof(UnsupportedConstructorParametersException), Generic = typeof(ClassWithOptionalConstructorParameter))]
        public void RegisteringInvalidServiceImplementationAsSingletonThrows<T>(Type expectedExceptionType) 
            where T : class, IService
        {
            TestDelegate when = () => new Container(r => 
                r.RegisterService<IService>().ImplementedBy<T>().AsSingleton());

            Assert.That(when, Throws.Exception.InstanceOf(expectedExceptionType));
        }
        
        [Test]
        [TestCase(typeof(NonFactoryDelegateException), Generic = typeof(ClassDependingOnDelegateWithNoReturnType))]
        [TestCase(typeof(NonFactoryDelegateException), Generic = typeof(ClassDependingOnDelegateWithParameters))]
        [TestCase(typeof(NonFactoryDelegateException), Generic = typeof(ClassDependingOnDelegateWithParametersAndNoReturnType))]
        [TestCase(typeof(NotRegisteredDependencyException), Generic = typeof(ClassDependingOnServiceNotRegisteredYet))]
        public void RegisteringServiceImplementationWithInvalidDependencyAsSingletonThrows<T>(Type expectedInnerExceptionType)
            where T : class, IService
        {
            TestDelegate when = () => new Container(r => 
                r.RegisterService<IService>().ImplementedBy<T>().AsSingleton());

            Assert.That(
                when,
                Throws.Exception.InstanceOf<DependencyRegistrationException>()
                    .With.InnerException.InstanceOf(expectedInnerExceptionType));
        }

        [Test]
        public void RegisteringCustomDisposableServiceThrows()
        {
            TestDelegate when = () => new Container(r => 
                r.RegisterService<IDisposableService>().ConstructedBy(() => (IDisposableService)null));

            Assert.That(when, Throws.Exception.InstanceOf<DisposableClassException>());
        }

        [Test]
        [TestCase(Generic = typeof(IEnumerable<IDummy>))]
        [TestCase(Generic = typeof(IReadOnlyCollection<IDummy>))]
        [TestCase(Generic = typeof(IReadOnlyList<IDummy>))]
        [TestCase(Generic = typeof(ICollection<IDummy>))]
        [TestCase(Generic = typeof(ISet<IDummy>))]
        [TestCase(Generic = typeof(IList<IDummy>))]
        [TestCase(Generic = typeof(List<IDummy>))]
        [TestCase(Generic = typeof(HashSet<IDummy>))]
        [TestCase(Generic = typeof(LinkedList<IDummy>))]
        [TestCase(Generic = typeof(SortedSet<IDummy>))]
        [TestCase(Generic = typeof(Stack<IDummy>))]
        [TestCase(Generic = typeof(Queue<IDummy>))]
        public void RegisteringClassDependentOnSequenceServiceNotRegisteredYetThrowsSpecificException<TSequence>()
            where TSequence : IEnumerable<IDummy>
        {
            TestDelegate when = () => new Container(r => 
                r.RegisterService<IService>()
                    .ImplementedBy<ClassDependingOnSequenceServiceNotRegisteredYet<TSequence>>());

            Assert.That(
                when, 
                Throws.Exception.InstanceOf<DependencyRegistrationException>().With.InnerException
                    .With.InstanceOf<NotRegisteredSequenceDependencyException>());
        }
        
        [Test]
        [TestCase(Generic = typeof(IEnumerable<IDummy>))]
        [TestCase(Generic = typeof(IReadOnlyCollection<IDummy>))]
        [TestCase(Generic = typeof(IReadOnlyList<IDummy>))]
        [TestCase(Generic = typeof(ICollection<IDummy>))]
        [TestCase(Generic = typeof(ISet<IDummy>))]
        [TestCase(Generic = typeof(IList<IDummy>))]
        [TestCase(Generic = typeof(List<IDummy>))]
        [TestCase(Generic = typeof(HashSet<IDummy>))]
        [TestCase(Generic = typeof(LinkedList<IDummy>))]
        [TestCase(Generic = typeof(SortedSet<IDummy>))]
        [TestCase(Generic = typeof(Stack<IDummy>))]
        [TestCase(Generic = typeof(Queue<IDummy>))]
        public void RegisteringClassDependentOnSequenceServiceNotRegisteredYetAsSingletonThrowsSpecificException<TSequence>()
            where TSequence : IEnumerable<IDummy>
        {
            TestDelegate when = () => new Container(r => 
                r.RegisterService<IService>()
                    .ImplementedBy<ClassDependingOnSequenceServiceNotRegisteredYet<TSequence>>()
                    .AsSingleton());

            Assert.That(
                when, 
                Throws.Exception.InstanceOf<DependencyRegistrationException>().With.InnerException
                    .With.InstanceOf<NotRegisteredSequenceDependencyException>());
        }
        
        [Test]
        public void RegisteringClassDependentOnSequenceDescendantNotRegisteredYetThrowsNonSpecificException()
        {
            TestDelegate when = () => new Container(r => 
                r.RegisterService<IService>().ImplementedBy<ClassDependingOnSequenceDescendantNotRegisteredYet>());

            Assert.That(
                when, 
                Throws.Exception.InstanceOf<DependencyRegistrationException>().With.InnerException
                    .With.InstanceOf<NotRegisteredDependencyException>());
        }

        [Test]
        [SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
        public void RegistrationIsNotPossibleAfterContainerIsCreated()
        {
            IRegistrator registrator = null;
            var container = new Container(r => registrator = r);
            registrator.RegisterService<IService>().ImplementedBy<ServiceImplementation>();

            TestDelegate when = () => container.Resolve<IService>();

            Assert.That(when, Throws.Exception.InstanceOf<NotRegisteredServiceException>());
        }

        private class ClassWithPrivateConstructor : IService
        {
            private ClassWithPrivateConstructor()
            {
            }
        }

        private class ClassWithInternalConstructor : IService
        {
            internal ClassWithInternalConstructor()
            {
            }
        }

        private class ClassWithMultipleConstructors : IService
        {
            public ClassWithMultipleConstructors()
            {
            }


            public ClassWithMultipleConstructors(object dummy)
            {
            }
        }

        private abstract class AbstractClass : IService
        {
        }

        private interface IInterface : IService
        {
        }

        private class DisposableClass : IService, IDisposable
        {
            public void Dispose()
            {
            }
        }

        private class ClassWithRefConstructorParameter : IService
        {
            public ClassWithRefConstructorParameter(ref IDummy dependency)
            {
            }
        }

        private class ClassWithOutConstructorParameter : IService
        {
            public ClassWithOutConstructorParameter(out IDummy dependency)
            {
                dependency = null;
            }
        }

        private class ClassWithOptionalConstructorParameter : IService
        {
            public ClassWithOptionalConstructorParameter(IDummy dependency = null)
            {
            }
        }

        private class ClassDependingOnDelegateWithNoReturnType : IService
        {
            public delegate void DelegateWithNoReturnType();

            public ClassDependingOnDelegateWithNoReturnType(DelegateWithNoReturnType dependency)
            {
            }
        }

        private class ClassDependingOnDelegateWithParameters : IService
        {
            public delegate IDummy DelegateWithParameters(object dummyParameter);

            public ClassDependingOnDelegateWithParameters(DelegateWithParameters dependency)
            {
            }
        }

        private class ClassDependingOnDelegateWithParametersAndNoReturnType : IService
        {
            public delegate void DelegateWithParametersAndNoReturnType(object dummyParameter);

            public ClassDependingOnDelegateWithParametersAndNoReturnType(
                DelegateWithParametersAndNoReturnType dependency)
            {
            }
        }
        
        private class ClassDependingOnServiceNotRegisteredYet : IService
        {
            public ClassDependingOnServiceNotRegisteredYet(INotRegisteredService dependency)
            {
            }
        }

        private class ClassDependingOnSequenceServiceNotRegisteredYet<TSequence> : IService
            where TSequence : IEnumerable<IDummy>
        {
            public ClassDependingOnSequenceServiceNotRegisteredYet(TSequence dependency)
            {
            }
        }
        
        private class ClassDependingOnSequenceDescendantNotRegisteredYet : IService
        {
            public ClassDependingOnSequenceDescendantNotRegisteredYet(ISequenceDescendant dependency)
            {
            }
        }

        private interface ISequenceDescendant : IEnumerable<IDummy>
        {
        }

        private interface INotRegisteredService
        {
        }

        private interface IDisposableService : IDisposable
        {
        }

        public interface IDummy
        {
        }

        private class ServiceImplementation : IService
        {
        }
        
        public interface IService
        {
        }
    }
}