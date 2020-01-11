using System;
using System.Diagnostics.CodeAnalysis;
using Essence.Ioc.FluentRegistration;
using NUnit.Framework;

namespace Essence.Ioc.LifeCycleManagement
{
    [TestFixture]
    public class DisposalExceptionPropagationTests
    {
        [Test]
        public void TransientService()
        {
            var container = new Container(r =>
                r.RegisterService<IService>().ImplementedBy<ExceptionThrowingDisposableStub>());
            var transientLifeScope = container.Resolve<IService>(out _);

            Assert.That(container.Dispose, Throws.Nothing);
            Assert.That(transientLifeScope.Dispose, Throws.InstanceOf<TestDisposalException>());
        }

        [Test]
        public void SingletonService()
        {
            var container = new Container(r =>
                r.RegisterService<IService>().ImplementedBy<ExceptionThrowingDisposableStub>().AsSingleton());
            var transientLifeScope = container.Resolve<IService>(out _);

            Assert.That(transientLifeScope.Dispose, Throws.Nothing);
            Assert.That(container.Dispose, Throws.InstanceOf<TestDisposalException>());
        }

        [Test]
        public void TransientDependency()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IDependency>().ImplementedBy<ExceptionThrowingDisposableStub>();
                r.RegisterService<IService>().ImplementedBy<DependencyConsumerStub>();
            });
            var transientLifeScope = container.Resolve<IService>(out _);

            Assert.That(container.Dispose, Throws.Nothing);
            Assert.That(transientLifeScope.Dispose, Throws.InstanceOf<TestDisposalException>());
        }

        [Test]
        public void SingletonDependency()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IDependency>().ImplementedBy<ExceptionThrowingDisposableStub>().AsSingleton();
                r.RegisterService<IService>().ImplementedBy<DependencyConsumerStub>();
            });
            var transientLifeScope = container.Resolve<IService>(out _);

            Assert.That(transientLifeScope.Dispose, Throws.Nothing);
            Assert.That(container.Dispose, Throws.InstanceOf<TestDisposalException>());
        }

        [Test]
        public void SingletonDependencyConsumer()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IDependency>().ImplementedBy<ExceptionThrowingDisposableStub>();
                r.RegisterService<IService>().ImplementedBy<DependencyConsumerStub>().AsSingleton();
            });
            var transientLifeScope = container.Resolve<IService>(out _);

            Assert.That(transientLifeScope.Dispose, Throws.Nothing);
            Assert.That(container.Dispose, Throws.InstanceOf<TestDisposalException>());
        }

        [Test]
        public void TransientLazyServiceDependency()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IDependency>().ImplementedBy<ExceptionThrowingDisposableStub>();
                r.RegisterService<IUsableService>().ImplementedBy<LazyDependencyConsumerStub>();
            });
            var transientLifeScope = container.Resolve<IUsableService>(out var service);
            service.Use();

            Assert.That(container.Dispose, Throws.Nothing);
            Assert.That(transientLifeScope.Dispose, Throws.InstanceOf<TestDisposalException>());
        }

        [Test]
        public void SingletonLazyServiceDependency()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IDependency>().ImplementedBy<ExceptionThrowingDisposableStub>().AsSingleton();
                r.RegisterService<IUsableService>().ImplementedBy<LazyDependencyConsumerStub>();
            });
            var transientLifeScope = container.Resolve<IUsableService>(out var service);
            service.Use();

            Assert.That(transientLifeScope.Dispose, Throws.Nothing);
            Assert.That(container.Dispose, Throws.InstanceOf<TestDisposalException>());
        }

        [Test]
        public void TransientServiceFactoryDependency()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IDependency>().ImplementedBy<ExceptionThrowingDisposableStub>();
                r.RegisterService<IUsableService>().ImplementedBy<DependencyFactoryConsumerStub>();
            });
            var transientLifeScope = container.Resolve<IUsableService>(out var service);
            service.Use();

            Assert.That(container.Dispose, Throws.Nothing);
            Assert.That(transientLifeScope.Dispose, Throws.InstanceOf<TestDisposalException>());
        }

        [Test]
        public void SingletonServiceFactoryDependency()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IDependency>().ImplementedBy<ExceptionThrowingDisposableStub>().AsSingleton();
                r.RegisterService<IUsableService>().ImplementedBy<DependencyFactoryConsumerStub>();
            });
            var transientLifeScope = container.Resolve<IUsableService>(out var service);
            service.Use();

            Assert.That(transientLifeScope.Dispose, Throws.Nothing);
            Assert.That(container.Dispose, Throws.InstanceOf<TestDisposalException>());
        }

        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        private class ExceptionThrowingDisposableStub : IService, IDependency, IDisposable
        {
            public void Dispose() => throw new TestDisposalException();
        }

        private class TestDisposalException : Exception
        {
        }

        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private class DependencyConsumerStub : IService
        {
            public DependencyConsumerStub(IDependency dependency)
            {
            }
        }

        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        private class LazyDependencyConsumerStub : IUsableService
        {
            private readonly Lazy<IDependency> _lazyDependency;

            public LazyDependencyConsumerStub(Lazy<IDependency> lazyDependency) =>
                _lazyDependency = lazyDependency;

            [SuppressMessage("ReSharper", "AssignmentIsFullyDiscarded")]
            public void Use() => _ = _lazyDependency.Value;
        }

        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        private class DependencyFactoryConsumerStub : IUsableService
        {
            private readonly Func<IDependency> _dependencyFactory;

            public DependencyFactoryConsumerStub(Func<IDependency> dependencyFactory) =>
                _dependencyFactory = dependencyFactory;

            [SuppressMessage("ReSharper", "AssignmentIsFullyDiscarded")]
            public void Use() => _ = _dependencyFactory.Invoke();
        }

        private interface IService
        {
        }

        private interface IDependency
        {
        }

        private interface IUsableService
        {
            void Use();
        }
    }
}