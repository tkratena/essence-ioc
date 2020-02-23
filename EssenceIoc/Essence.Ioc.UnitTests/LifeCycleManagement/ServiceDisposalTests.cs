using System;
using System.Collections.Generic;
using Essence.Ioc.FluentRegistration;
using NUnit.Framework;

namespace Essence.Ioc.LifeCycleManagement
{
    [TestFixture]
    public class ServiceDisposalTests
    {
        private static IEnumerable<TestRegistrationType> ServiceRegistrationTypes => new[]
        {
            new TestRegistrationType(
                "Transient",
                r => r.RegisterService<IService>().ImplementedBy<DisposableSpy>()),

            new TestRegistrationType(
                "Custom factory",
                r => r.RegisterService<IService>().ConstructedBy(() => new DisposableSpy())),

            new TestRegistrationType(
                "Custom factory using container",
                r => r.RegisterService<IService>().ConstructedBy(_ => new DisposableSpy())),

            new TestRegistrationType(
                "Custom factory not returning the instance as disposable",
                r => r.RegisterService<IService>().ConstructedBy(() => (NonDisposable) new DisposableSpy())),
        };

        [Test]
        public void TransientServiceIsDisposedWithTransientLifeScope(
            [ValueSource(nameof(ServiceRegistrationTypes))] TestRegistrationType registration)
        {
            var container = new Container(r => registration.Invoke(r));
            var transientLifeScope = container.Resolve<IService>(out var disposableSpy);

            transientLifeScope.Dispose();

            Assert.That(disposableSpy, Has.Property(nameof(DisposableSpy.IsDisposed)).True);
        }

        [Test]
        public void TransientServiceIsNotDisposedWithContainer(
            [ValueSource(nameof(ServiceRegistrationTypes))] TestRegistrationType registration)
        {
            var container = new Container(r => registration.Invoke(r));
            var _ = container.Resolve<IService>(out var disposableSpy);

            container.Dispose();

            Assert.That(disposableSpy, Has.Property(nameof(DisposableSpy.IsDisposed)).False);
        }

        [Test]
        public void TransientServiceIsDisposedOnlyOnce(
            [ValueSource(nameof(ServiceRegistrationTypes))] TestRegistrationType registration)
        {
            var container = new Container(r => registration.Invoke(r));
            var transientLifeScope = container.Resolve<IService>(out var disposableSpy);

            transientLifeScope.Dispose();
            transientLifeScope.Dispose();

            Assert.That(disposableSpy, Has.Property(nameof(DisposableSpy.DisposeCount)).EqualTo(1));
        }

        [Test]
        public void SingletonServiceIsNotDisposedWithTransientLifeScope(
            [ValueSource(nameof(ServiceRegistrationTypes))] TestRegistrationType registration)
        {
            var container = new Container(r => registration.Invoke(r).AsSingleton());
            var transientLifeScope = container.Resolve<IService>(out _);
            transientLifeScope.Dispose();

            container.Resolve<IService>(out var disposableSpy);

            Assert.That(disposableSpy, Has.Property(nameof(DisposableSpy.IsDisposed)).False);
        }

        [Test]
        public void SingletonServiceIsDisposedWithContainer(
            [ValueSource(nameof(ServiceRegistrationTypes))] TestRegistrationType registration)
        {
            var container = new Container(r => registration.Invoke(r).AsSingleton());
            var transientLifeScope = container.Resolve<IService>(out _);
            transientLifeScope.Dispose();
            transientLifeScope = container.Resolve<IService>(out var disposableSpy);
            transientLifeScope.Dispose();

            container.Dispose();

            Assert.That(disposableSpy, Has.Property(nameof(DisposableSpy.IsDisposed)).True);
        }

        [Test]
        public void SingletonServiceIsDisposedOnlyOnce(
            [ValueSource(nameof(ServiceRegistrationTypes))] TestRegistrationType registration)
        {
            var container = new Container(r => registration.Invoke(r).AsSingleton());
            var transientLifeScope = container.Resolve<IService>(out var disposableSpy);
            transientLifeScope.Dispose();

            container.Dispose();
            container.Dispose();

            Assert.That(disposableSpy, Has.Property(nameof(DisposableSpy.DisposeCount)).EqualTo(1));
        }

        private class DisposableSpy : NonDisposable, IDisposable
        {
            public int DisposeCount { get; private set; }

            public bool IsDisposed => DisposeCount > 0;

            public void Dispose() => DisposeCount++;
        }

        private abstract class NonDisposable : IService
        {
        }

        private interface IService
        {
        }
    }
}