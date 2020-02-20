using System;
using System.Diagnostics.CodeAnalysis;
using Essence.Ioc.FluentRegistration;
using NUnit.Framework;

namespace Essence.Ioc.Resolution
{
    [TestFixture]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class SingletonConstructionExceptionHandlingTests
    {
        private Container _container;

        [SetUp]
        public void SingletonServiceConstructionFailsForTheFirstTime()
        {
            _container = new Container(r =>
            {
                r.RegisterService<IService>().ImplementedBy<ThrowingConstructionStub>().AsSingleton();
            });
            
            ThrowingConstructionStub.IsConstructorThrowing = true;
            Assert.That(() => _container.Resolve<IService>(out _), Throws.Exception);
            ThrowingConstructionStub.IsConstructorThrowing = false;
        }
        
        [Test]
        public void SingletonServiceIsConstructedAgainAfterFailure()
        {
            _container.Resolve<IService>(out var service);
            
            Assert.That(service, Is.InstanceOf<ThrowingConstructionStub>());
        }
        
        [Test]
        public void SingletonServiceIsNotConstructedAgainAfterSuccess()
        {
            _container.Resolve<IService>(out var firstInstance);
            _container.Resolve<IService>(out var secondInstance);
            
            Assert.That(firstInstance, Is.SameAs(secondInstance));
        }

        private class ThrowingConstructionStub : IService
        {
            public static bool IsConstructorThrowing { get; set; }
            
            public ThrowingConstructionStub()
            {
                if (IsConstructorThrowing)
                {
                    throw new Exception("Test constructor exception");
                }
            }
        }
        
        private interface IService
        {
        }
    }
}