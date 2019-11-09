using System;
using System.Diagnostics.CodeAnalysis;
using Essence.Ioc.FluentRegistration;
using Essence.Ioc.Registration.RegistrationExceptions;
using NUnit.Framework;

namespace Essence.Ioc.Registration
{
    [TestFixture]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    public class InvalidDisposableServiceRegistrationTests
    {
        [Test]
        public void RegisteringThrows()
        {
            TestDelegate when = () => new Container(r => 
                r.RegisterService<IDisposableService>().ImplementedBy<DisposableServiceImplementation>());

            Assert.That(when, Throws.Exception.InstanceOf<DisposableServiceException>());
        }
            
        [Test]
        public void RegisteringAsSingletonThrows()
        {
            TestDelegate when = () => new Container(r => 
                r.RegisterService<IDisposableService>().ImplementedBy<DisposableServiceImplementation>().AsSingleton());

            Assert.That(when, Throws.Exception.InstanceOf<DisposableServiceException>());
        }
            
        [Test]
        public void RegisteringWithCustomFactoryThrows()
        {
            TestDelegate when = () => new Container(r => 
                r.RegisterService<IDisposableService>().ConstructedBy(() => (IDisposableService)null));

            Assert.That(when, Throws.Exception.InstanceOf<DisposableServiceException>());
        }
            
        [Test]
        public void GenericallyRegisteringThrows()
        {
            TestDelegate when = () => new Container(r => 
                r.GenericallyRegisterService(typeof(IDisposableGenericService<>))
                    .ImplementedBy(typeof(DisposableGenericServiceImplementation<>)));

            Assert.That(when, Throws.Exception.InstanceOf<DisposableServiceException>());
        }
        
        private interface IDisposableService : IDisposable
        {
        }
        
        private class DisposableServiceImplementation : IDisposableService
        {
            public void Dispose()
            {
            }
        }
        
        [SuppressMessage("ReSharper", "UnusedTypeParameter")]
        private interface IDisposableGenericService<T> : IDisposable
        {
        }
        
        private class DisposableGenericServiceImplementation<T> : IDisposableGenericService<T>
        {
            public void Dispose()
            {
            }
        }
    }
}