using System;
using System.Diagnostics.CodeAnalysis;
using Essence.Ioc.FluentRegistration;
using NUnit.Framework;

namespace Essence.Ioc.Resolution
{
    [TestFixture]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class ResolutionTests
    {
        [Test]
        public void ClassAsService()
        {
            var container = new Container(r =>
                r.RegisterService<ConcreteService>().ImplementedBy<ConcreteService>());

            container.Resolve<ConcreteService>(out var service);

            Assert.IsInstanceOf<ConcreteService>(service);
        }

        [TestFixture]
        public class ServiceTests
        {
            private Container _container;

            [SetUp]
            public void SetUp()
            {
                _container = new Container(r =>
                    r.RegisterService<IService>().ImplementedBy<ServiceImplementation>());
            }

            [Test]
            public void Service()
            {
                _container.Resolve<IService>(out var service);

                Assert.IsInstanceOf<ServiceImplementation>(service);
            }

            [Test]
            public void LazyService()
            {
                _container.Resolve<Lazy<IService>>(out var lazyService);

                Assert.IsFalse(lazyService.IsValueCreated);
                Assert.IsInstanceOf<ServiceImplementation>(lazyService.Value);
            }

            [Test]
            public void ServiceFactory()
            {
                _container.Resolve<Func<IService>>(out var serviceFactory);

                Assert.IsInstanceOf<ServiceImplementation>(serviceFactory.Invoke());
            }

            [Test]
            public void ServiceFactoryDelegate()
            {
                _container.Resolve<DelegateReturningService>(out var serviceFactory);

                Assert.IsInstanceOf<ServiceImplementation>(serviceFactory.Invoke());
            }

            private delegate IService DelegateReturningService();
        }

        [TestFixture]
        public class GenericallyRegisteredGenericServiceTests
        {
            private Container _container;

            [SetUp]
            public void SetUp()
            {
                _container = new Container(r =>
                    r.GenericallyRegisterService(typeof(IService<>)).ImplementedBy(typeof(ServiceImplementation<>)));
            }

            [Test]
            public void Service()
            {
                _container.Resolve<IService<IActualGenericArg>>(out var service);

                Assert.IsInstanceOf<ServiceImplementation<IActualGenericArg>>(service);
            }

            [Test]
            public void LazyService()
            {
                _container.Resolve<Lazy<IService<IActualGenericArg>>>(out var lazyService);

                Assert.IsFalse(lazyService.IsValueCreated);
                Assert.IsInstanceOf<ServiceImplementation<IActualGenericArg>>(lazyService.Value);
            }

            [Test]
            public void ServiceFactory()
            {
                _container.Resolve<Func<IService<IActualGenericArg>>>(out var serviceFactory);

                Assert.IsInstanceOf<ServiceImplementation<IActualGenericArg>>(serviceFactory.Invoke());
            }

            [Test]
            public void ServiceFactoryDelegate()
            {
                _container.Resolve<DelegateReturningService>(out var serviceFactory);

                Assert.IsInstanceOf<ServiceImplementation<IActualGenericArg>>(serviceFactory.Invoke());
            }

            private delegate IService<IActualGenericArg> DelegateReturningService();
        }

        [Test]
        public void GenericService()
        {
            var container = new Container(r =>
                r.RegisterService<IService<IActualGenericArg>>()
                    .ImplementedBy<ServiceImplementation<IActualGenericArg>>());

            container.Resolve<IService<IActualGenericArg>>(out var service);

            Assert.IsInstanceOf<ServiceImplementation<IActualGenericArg>>(service);
        }

        [Test]
        public void GenericallyRegisteredGenericServiceImplementationNestedInAGenericClass()
        {
            var container = new Container(r =>
                r.GenericallyRegisterService(typeof(IService<>))
                    .ImplementedBy(typeof(GenericClass<>.NestedGenericServiceImplementation)));

            container.Resolve<IService<IActualGenericArg>>(out var service);

            Assert.IsInstanceOf<GenericClass<IActualGenericArg>.NestedGenericServiceImplementation>(service);
        }

        private class GenericClass<T>
        {
            public class NestedGenericServiceImplementation : IService<T>
            {
            }
        }

        [Test]
        public void GenericallyRegisteredGenericServiceWithTwoGenericArguments()
        {
            var container = new Container(r =>
                r.GenericallyRegisterService(typeof(IService<,>)).ImplementedBy(typeof(ServiceImplementation<,>)));

            container.Resolve<IService<IFirstActualGenericArg, ISecondActualGenericArg>>(out var service);

            Assert.IsInstanceOf<ServiceImplementation<IFirstActualGenericArg, ISecondActualGenericArg>>(service);
        }

        private interface IFirstActualGenericArg
        {
        }

        private interface ISecondActualGenericArg
        {
        }

        private interface IActualGenericArg
        {
        }

        private class ServiceImplementation<TFirst, TSecond> : IService<TFirst, TSecond>
        {
        }

        private class ServiceImplementation<T> : IService<T>
        {
        }

        private class ServiceImplementation : IService
        {
        }

        [SuppressMessage("ReSharper", "UnusedTypeParameter")]
        private interface IService<TFirst, TSecond>
        {
        }

        [SuppressMessage("ReSharper", "UnusedTypeParameter")]
        private interface IService<T>
        {
        }

        private interface IService
        {
        }

        private class ConcreteService
        {
        }
    }
}
