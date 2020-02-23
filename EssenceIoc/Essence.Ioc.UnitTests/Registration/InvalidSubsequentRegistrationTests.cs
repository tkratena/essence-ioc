using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Essence.Ioc.FluentRegistration;
using Essence.Ioc.Registration.RegistrationExceptions;
using NUnit.Framework;

namespace Essence.Ioc.Registration
{
    [TestFixture]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    public class InvalidSubsequentRegistrationTests
    {
        private static IEnumerable<TestRegistration> AllServiceRegistrations { get; } = new[]
        {
            new TestRegistration(
                "Transient",
                r => r.RegisterService<IService>().ImplementedBy<ServiceImplementation>()),

            new TestRegistration(
                "Singleton",
                r => r.RegisterService<IService>().ImplementedBy<ServiceImplementation>().AsSingleton()),

            new TestRegistration(
                "Custom factory",
                r => r.RegisterService<IService>().ConstructedBy(() => new ServiceImplementation())),

            new TestRegistration(
                "Custom factory singleton",
                r => r.RegisterService<IService>().ConstructedBy(() => new ServiceImplementation()).AsSingleton()),

            new TestRegistration(
                "Custom factory using container",
                r => r.RegisterService<IService>().ConstructedBy(_ => new ServiceImplementation())),

            new TestRegistration(
                "Custom factory singleton using container",
                r => r.RegisterService<IService>().ConstructedBy(_ => new ServiceImplementation()).AsSingleton())
        };

        [Test]
        public void AlreadyRegisteredServiceThrows(
            [ValueSource(nameof(AllServiceRegistrations))] TestRegistration registration,
            [ValueSource(nameof(AllServiceRegistrations))] TestRegistration subsequentRegistration)
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
    }
}