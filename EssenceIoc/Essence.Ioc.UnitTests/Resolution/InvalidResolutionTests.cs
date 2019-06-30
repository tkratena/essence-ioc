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

            TestDelegate when = () => container.Resolve<INotImplementedService>();

            Assert.That(when, Throws.Exception.InstanceOf<NotRegisteredServiceException>());
        }
        
        private interface INotImplementedService
        {
        }

        [Test]
        public void NotRegisteredServiceThatIsImplementedByRegisteredImplementationThrows()
        {
            var container = new Container(r =>
                r.RegisterService<IRegisteredService>()
                    .ImplementedBy<ImplementationOfRegisteredAndNotRegisteredServices>());

            TestDelegate when = () => container.Resolve<INotRegisteredService>();

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

            TestDelegate when = () => container.Resolve<INotRegisteredBaseOfRegisteredService>();

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