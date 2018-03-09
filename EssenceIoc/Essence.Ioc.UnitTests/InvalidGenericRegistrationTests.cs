using System;
using System.Diagnostics.CodeAnalysis;
using Essence.Ioc.Registration.RegistrationExceptions;
using NUnit.Framework;
using TestCaseAttribute = Essence.TestFramework.TestCaseAttribute;

namespace Essence.Ioc
{
    [TestFixture]
    [SuppressMessage("ReSharper", "UnusedTypeParameter")]
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    public class InvalidGenericRegistrationTests
    {
        [Test]
        [TestCase(typeof(INonGenericService))]
        [TestCase(typeof(IGenericService<object>))]
        public void RegisteringNonGenericTypeDefinitionAsGenericServiceThrows(Type serviceType)
        {
            TestDelegate when = () => new Container(r =>
                r.GenericlyRegisterService(serviceType).ImplementedBy(typeof(GenericServiceImplementation<>)));

            Assert.That(when, Throws.Exception.InstanceOf<ServiceTypeNotGenericTypeDefinitionException>());
        }
        
        [Test]
        [TestCase(typeof(NonGenericServiceImplementation))]
        [TestCase(typeof(GenericServiceImplementation<object>))]
        public void RegisteringNonGenericTypeDefinitionAsGenericServiceImplementationThrows(Type implementationType)
        {
            TestDelegate when = () => new Container(r => 
                r.GenericlyRegisterService(typeof(IGenericService<>)).ImplementedBy(implementationType));

            Assert.That(when, Throws.Exception.InstanceOf<ImplementationTypeNotGenericTypeDefinitionException>());
        }
        
        [Test]
        public void RegisteringClassNotImplementingGivenServiceAsGenericServiceImplementationThrows()
        {
            TestDelegate when = () => new Container(r => 
                r.GenericlyRegisterService(typeof(IGenericService<>))
                    .ImplementedBy(typeof(GenericClassNotImplementingAService<>)));

            Assert.That(when, Throws.Exception.InstanceOf<ImplementationTypeNotImplementingGenericService>());
        }

        private interface IGenericService<T>
        {
        }
        
        private interface INonGenericService
        {
        }

        private class GenericServiceImplementation<T> : IGenericService<T>
        {
        }

        private class NonGenericServiceImplementation : INonGenericService
        {
        }
        
        private class GenericClassNotImplementingAService<T>
        {
        }
    }
}