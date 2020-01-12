using System.Diagnostics.CodeAnalysis;
using Essence.Ioc.FluentRegistration;
using NUnit.Framework;

namespace Essence.Ioc.Resolution
{
    [TestFixture]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    [SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
    public class InvalidResolutionTests
    {
        [Test]
        public void NotRegisteredServiceThrows()
        {
            var container = new Container(_ => { });

            TestDelegate when = () => container.Resolve<INotImplementedService>(out _);

            Assert.That(when, Throws.Exception.InstanceOf<NotRegisteredServiceException>());
        }
        
        private interface INotImplementedService
        {
        }
        
        [Test]
        public void ClassNotRegisteredAsServiceOnlyUsedAsImplementationThrows()
        {
            var container = new Container(r => 
                r.RegisterService<IDummyService>().ImplementedBy<ServiceImplementation>());

            TestDelegate when = () => container.Resolve<ServiceImplementation>(out _);

            Assert.That(when, Throws.Exception.InstanceOf<NotRegisteredServiceException>());
        }
        
        private class ServiceImplementation : IDummyService
        {
        }
        
        private interface IDummyService
        {
        }

        [Test]
        public void NotRegisteredServiceThatIsImplementedByRegisteredImplementationThrows()
        {
            var container = new Container(r =>
                r.RegisterService<IRegisteredService>()
                    .ImplementedBy<ImplementationOfRegisteredAndNotRegisteredServices>());

            TestDelegate when = () => container.Resolve<INotRegisteredService>(out _);

            Assert.That(when, Throws.Exception.InstanceOf<NotRegisteredServiceException>());
        }

        private class ImplementationOfRegisteredAndNotRegisteredServices : IRegisteredService, INotRegisteredService
        {
        }

        private interface IRegisteredService
        {
        }

        private interface INotRegisteredService
        {
        }

        [Test]
        public void NotRegisteredBaseOfRegisteredServiceThrows()
        {
            var container = new Container(r =>
                r.RegisterService<IDerivedRegisteredService>()
                    .ImplementedBy<ImplementationDerivedRegisteredService>());

            TestDelegate when = () => container.Resolve<INotRegisteredBaseOfRegisteredService>(out _);

            Assert.That(when, Throws.Exception.InstanceOf<NotRegisteredServiceException>());
        }

        private class ImplementationDerivedRegisteredService : IDerivedRegisteredService
        {
        }

        private interface IDerivedRegisteredService : INotRegisteredBaseOfRegisteredService
        {
        }

        private interface INotRegisteredBaseOfRegisteredService
        {
        }
    }
}