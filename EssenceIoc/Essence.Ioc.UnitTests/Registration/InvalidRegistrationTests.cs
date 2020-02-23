using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Essence.Ioc.FluentRegistration;
using Essence.Ioc.Registration.RegistrationExceptions;
using Essence.Ioc.Resolution;
using NUnit.Framework;

namespace Essence.Ioc.Registration
{
    [TestFixture]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    [SuppressMessage("ReSharper", "EmptyConstructor")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    public class InvalidRegistrationTests
    {
        [TestFixture(typeof(ClassWithPrivateConstructor), typeof(NoConstructorException))]
        [TestFixture(typeof(ClassWithInternalConstructor), typeof(NoConstructorException))]
        [TestFixture(typeof(ClassWithMultipleConstructors), typeof(AmbiguousConstructorsException))]
        [TestFixture(typeof(AbstractClass), typeof(NonConcreteClassException))]
        [TestFixture(typeof(IInterface), typeof(NonConcreteClassException))]
        [TestFixture(typeof(ClassWithRefConstructorParameter), typeof(UnsupportedConstructorParametersException))]
        [TestFixture(typeof(ClassWithOutConstructorParameter), typeof(UnsupportedConstructorParametersException))]
        [TestFixture(typeof(ClassWithOptionalConstructorParameter), typeof(UnsupportedConstructorParametersException))]
        public class RegisteringInvalidServiceImplementation<TServiceImplementation, TExpectedException>
            where TServiceImplementation : class, IService
            where TExpectedException : Exception
        {
            [Test]
            public void RegisteringThrows()

            {
                TestDelegate when = () => new Container(r =>
                    r.RegisterService<IService>().ImplementedBy<TServiceImplementation>());

                Assert.That(when, Throws.Exception.InstanceOf<TExpectedException>());
            }

            [Test]
            public void RegisteringAsSingletonThrows()

            {
                TestDelegate when = () => new Container(r =>
                    r.RegisterService<IService>().ImplementedBy<TServiceImplementation>().AsSingleton());

                Assert.That(when, Throws.Exception.InstanceOf<TExpectedException>());
            }
        }

        [TestFixture(typeof(ClassDependingOnDelegateWithNoReturnType), typeof(NonFactoryDelegateException))]
        [TestFixture(typeof(ClassDependingOnDelegateWithParameters), typeof(NonFactoryDelegateException))]
        [TestFixture(typeof(ClassDependingOnDelegateWithParametersAndNoReturnType), typeof(NonFactoryDelegateException))]
        [TestFixture(typeof(ClassDependingOnServiceNotRegisteredYet), typeof(NotRegisteredDependencyException))]
        [TestFixture(typeof(ClassDependingOnGenericServiceNotRegisteredYet), typeof(NotRegisteredDependencyException))]
        [TestFixture(typeof(ClassDependingOnGenericServiceWithMultipleGenericArgsNotRegisteredYet), typeof(NotRegisteredDependencyException))]
        public class RegisteringServiceImplementationWithInvalidDependency<TServiceImplementation, TExpectedException>
            where TServiceImplementation : class, IService
            where TExpectedException : Exception
        {
            [Test]
            public void RegisteringThrows()
            {
                TestDelegate when = () => new Container(r =>
                    r.RegisterService<IService>().ImplementedBy<TServiceImplementation>());

                Assert.That(
                    when,
                    Throws.Exception.InstanceOf<DependencyRegistrationException>()
                        .With.InnerException.InstanceOf<TExpectedException>());
            }

            [Test]
            public void RegisteringAsSingletonThrows()
            {
                TestDelegate when = () => new Container(r =>
                    r.RegisterService<IService>().ImplementedBy<TServiceImplementation>().AsSingleton());

                Assert.That(
                    when,
                    Throws.Exception.InstanceOf<DependencyRegistrationException>()
                        .With.InnerException.InstanceOf<TExpectedException>());
            }
        }

        [TestFixture(typeof(IEnumerable<IDummy>))]
        [TestFixture(typeof(IReadOnlyCollection<IDummy>))]
        [TestFixture(typeof(IReadOnlyList<IDummy>))]
        [TestFixture(typeof(ICollection<IDummy>))]
        [TestFixture(typeof(ISet<IDummy>))]
        [TestFixture(typeof(IList<IDummy>))]
        [TestFixture(typeof(List<IDummy>))]
        [TestFixture(typeof(HashSet<IDummy>))]
        [TestFixture(typeof(LinkedList<IDummy>))]
        [TestFixture(typeof(SortedSet<IDummy>))]
        [TestFixture(typeof(Stack<IDummy>))]
        [TestFixture(typeof(Queue<IDummy>))]
        public class RegisteringClassDependentOnSequenceServiceNotRegisteredYet<TSequence>
            where TSequence : IEnumerable<IDummy>
        {
            [Test]
            public void RegisteringThrowsSpecificException()
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
            public void RegisteringAsSingletonThrowsSpecificException()
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
        public void RegistrationIsNotPossibleAfterContainerIsCreated()
        {
            ExtendableRegistration.Registerer registerer = null;
            var container = new Container(r => registerer = r);
            registerer.RegisterService<IService>().ImplementedBy<ServiceImplementation>();

            TestDelegate when = () => container.Resolve<IService>(out _);

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

        private class ClassDependingOnGenericServiceNotRegisteredYet : IService
        {
            public ClassDependingOnGenericServiceNotRegisteredYet(INotRegisteredService<IActualGenericArg> dependency)
            {
            }
        }

        private class ClassDependingOnGenericServiceWithMultipleGenericArgsNotRegisteredYet : IService
        {
            public ClassDependingOnGenericServiceWithMultipleGenericArgsNotRegisteredYet(
                INotRegisteredService<IActualGenericArg, IActualGenericArg> dependency)
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

        [SuppressMessage("ReSharper", "UnusedTypeParameter")]
        private interface INotRegisteredService<T>
        {
        }

        [SuppressMessage("ReSharper", "UnusedTypeParameter")]
        private interface INotRegisteredService<T1, T2>
        {
        }

        private interface IActualGenericArg
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