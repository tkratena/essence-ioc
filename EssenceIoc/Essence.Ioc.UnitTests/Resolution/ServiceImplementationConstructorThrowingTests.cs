using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Essence.Ioc.FluentRegistration;
using NUnit.Framework;

namespace Essence.Ioc.Resolution
{
    [TestFixture]
    [SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
    public class ServiceImplementationConstructorThrowingTests
    {
        public static IEnumerable TestCases = new[]
        {
            new TestCaseData(
                    new Container(r => r.RegisterService<IService>()
                        .ImplementedBy<ServiceImplementationWithThrowingConstructor>()))
                .SetName("Transient"),
            
            new TestCaseData(
                    new Container(r => r.RegisterService<IService>()
                        .ImplementedBy<ServiceImplementationWithThrowingConstructor>()
                        .AsSingleton()))
                .SetName("Singleton")
        };

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void Service(Container container)
        {
            Assert.Throws<ConstructorException>(() => container.Resolve<IService>());
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void LazyService(Container container)
        {
            var lazyService = container.Resolve<Lazy<IService>>();

            Assert.Throws<ConstructorException>(() =>
            {
                var _ = lazyService.Value;
            });
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void ServiceFactory(Container container)
        {
            var serviceFactory = container.Resolve<Func<IService>>();

            Assert.Throws<ConstructorException>(() => serviceFactory.Invoke());
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void ServiceFactoryDelegate(Container container)
        {
            var serviceFactory = container.Resolve<DelegateReturningService>();

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