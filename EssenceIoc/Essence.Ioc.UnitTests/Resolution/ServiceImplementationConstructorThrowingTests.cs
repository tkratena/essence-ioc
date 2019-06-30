using System;
using System.Diagnostics.CodeAnalysis;
using Essence.Ioc.FluentRegistration;
using NUnit.Framework;

namespace Essence.Ioc.Resolution
{
    [TestFixture]
    [SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
    public class ServiceImplementationConstructorThrowingTests
    {
        private Container _container;

        [SetUp]
        public void SetUp()
        {
            _container = new Container(r =>
                r.RegisterService<IService>().ImplementedBy<ServiceImplementationWithThrowingConstructor>());
        }

        [Test]
        public void Service()
        {
            Assert.Throws<ConstructorException>(() => _container.Resolve<IService>());
        }

        [Test]
        public void LazyService()
        {
            var lazyService = _container.Resolve<Lazy<IService>>();

            Assert.Throws<ConstructorException>(() => { var _ = lazyService.Value; });
        }

        [Test]
        public void ServiceFactory()
        {
            var serviceFactory = _container.Resolve<Func<IService>>();

            Assert.Throws<ConstructorException>(() => serviceFactory.Invoke());
        }

        [Test]
        public void ServiceFactoryDelegate()
        {
            var serviceFactory = _container.Resolve<DelegateReturningService>();

            Assert.Throws<ConstructorException>(() => serviceFactory.Invoke());
        }

        private delegate IService DelegateReturningService();

        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        private class ServiceImplementationWithThrowingConstructor : IService
        {
            public ServiceImplementationWithThrowingConstructor()
            {
                throw new ConstructorException();
            }
        }

        private class ConstructorException : Exception
        {
        }

        private interface IService
        {
        }
    }
}