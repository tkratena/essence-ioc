using System;
using System.Diagnostics.CodeAnalysis;
using Essence.Ioc.FluentRegistration;
using NUnit.Framework;

namespace Essence.Ioc.LifeCycleManagement
{
    [TestFixture]
    public class GenericallyRegisteredServiceDisposalTests
    {
        [Test]
        public void TransientServiceIsDisposedWithTransientLifeScope()
        {
            var container = new Container(r =>
                r.GenericallyRegisterService(typeof(IService<>)).ImplementedBy(typeof(DisposableSpy<>)));
            var transientLifeScope = container.Resolve<IService<IActualGenericArg>>(out var disposableSpy);

            transientLifeScope.Dispose();

            Assert.That(disposableSpy, Has.Property(nameof(DisposableSpy.IsDisposed)).True);
        }

        [Test]
        public void TransientServiceIsNotDisposedWithContainer()
        {
            var container = new Container(r =>
                r.GenericallyRegisterService(typeof(IService<>)).ImplementedBy(typeof(DisposableSpy<>)));
            var _ = container.Resolve<IService<IActualGenericArg>>(out var disposableSpy);

            container.Dispose();

            Assert.That(disposableSpy, Has.Property(nameof(DisposableSpy.IsDisposed)).False);
        }

        [Test]
        public void TransientServiceIsDisposedOnlyOnce()
        {
            var container = new Container(r =>
                r.GenericallyRegisterService(typeof(IService<>)).ImplementedBy(typeof(DisposableSpy<>)));
            var transientLifeScope = container.Resolve<IService<IActualGenericArg>>(out var disposableSpy);

            transientLifeScope.Dispose();
            transientLifeScope.Dispose();

            Assert.That(disposableSpy, Has.Property(nameof(DisposableSpy.DisposeCount)).EqualTo(1));
        }

        private class DisposableSpy<T> : DisposableSpy, IService<T>, IDisposable
        {
        }

        private abstract class DisposableSpy
        {
            public int DisposeCount { get; private set; }

            public bool IsDisposed => DisposeCount > 0;

            public void Dispose() => DisposeCount++;
        }

        [SuppressMessage("ReSharper", "UnusedTypeParameter")]
        private interface IService<T>
        {
        }

        private interface IActualGenericArg
        {
        }
    }
}