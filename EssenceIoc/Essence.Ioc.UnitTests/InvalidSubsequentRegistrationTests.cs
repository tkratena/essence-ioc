using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Essence.Ioc.FluentRegistration;
using Essence.Ioc.Registration.RegistrationExceptions;
using NUnit.Framework;

namespace Essence.Ioc
{
    [TestFixture]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    public class InvalidSubsequentRegistrationTests
    {
        private static IEnumerable<Registration> AllServiceRegistrations { get; } = new[]
        {
            new Registration(
                "Transient",
                r => r.RegisterService<IService>().ImplementedBy<ServiceImplementation>()),

            new Registration(
                "Singleton",
                r => r.RegisterService<IService>().ImplementedBy<ServiceImplementation>().AsSingleton()),

            new Registration(
                "Custom factory",
                r => r.RegisterService<IService>().ConstructedBy(() => new ServiceImplementation())),
            
            new Registration(
                "Custom factory singleton",
                r => r.RegisterService<IService>().ConstructedBy(() => new ServiceImplementation()).AsSingleton()),

            new Registration(
                "Custom factory using container",
                r => r.RegisterService<IService>().ConstructedBy(_ => new ServiceImplementation())),
            
            new Registration(
                "Custom factory singleton using container",
                r => r.RegisterService<IService>().ConstructedBy(_ => new ServiceImplementation()).AsSingleton())
        };

        [Test]
        public void AlreadyRegisteredServiceThrows(
            [ValueSource(nameof(AllServiceRegistrations))] Registration registration,
            [ValueSource(nameof(AllServiceRegistrations))] Registration subsequentRegistration)
        {
            TestDelegate when = () => new Container(r =>
            {
                registration.Invoke(r);
                subsequentRegistration.Invoke(r);
            });

            Assert.That(when, Throws.Exception.InstanceOf<AlreadyRegisteredException>());
        }
        
        private class ServiceImplementation : IService
        {
        }

        private interface IService
        {
        }      

        public class Registration
        {
            private readonly string _description;
            private readonly Action<IRegisterer> _registerServices;

            public Registration(string description, Action<IRegisterer> registerServices)
            {
                _description = description;
                _registerServices = registerServices;
            }

            public void Invoke(IRegisterer registerer)
            {
                _registerServices.Invoke(registerer);
            }

            public override string ToString()
            {
                return _description;
            }
        }
    }
}