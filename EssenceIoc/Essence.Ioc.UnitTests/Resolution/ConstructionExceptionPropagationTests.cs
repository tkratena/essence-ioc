using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Essence.Ioc.FluentRegistration;
using NUnit.Framework;

namespace Essence.Ioc.Resolution
{
    [TestFixture]
    [SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
    public class ConstructionExceptionPropagationTests
    {
        public static IEnumerable TestCases = new[]
        {
            new TestCaseData(new Container(r => 
                    r.RegisterService<IService>().ImplementedBy<ImplementationWithThrowingConstructor>()))
                .SetName("Transient"),
            
            new TestCaseData(new Container(r => 
                    r.RegisterService<IService>().ImplementedBy<ImplementationWithThrowingConstructor>().AsSingleton()))
                .SetName("Singleton")
        };

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void Service(Container container)
        {
            Assert.Throws<ConstructorException>(() => container.Resolve<IService>(out _));
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void LazyService(Container container)
        {
            container.Resolve<Lazy<IService>>(out var lazyService);

            Assert.Throws<ConstructorException>(() =>
            {
                var _ = lazyService.Value;
            });
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void ServiceFactory(Container container)
        {
            container.Resolve<Func<IService>>(out var serviceFactory);

            Assert.Throws<ConstructorException>(() => serviceFactory.Invoke());
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void ServiceFactoryDelegate(Container container)
        {
            container.Resolve<DelegateReturningService>(out var serviceFactory);

            Assert.Throws<ConstructorException>(() => serviceFactory.Invoke());
        }

        private delegate IService DelegateReturningService();

        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        private class ImplementationWithThrowingConstructor : IService
        {
            public ImplementationWithThrowingConstructor()
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