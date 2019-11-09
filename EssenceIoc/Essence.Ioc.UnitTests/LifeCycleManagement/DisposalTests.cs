using System;
using System.Diagnostics.CodeAnalysis;
using Essence.Ioc.FluentRegistration;
using NUnit.Framework;

namespace Essence.Ioc.LifeCycleManagement
{
    [TestFixture]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class DisposalTests
    {
        [Test]
        public void ServiceIsNotDisposedBeforeContainerIs()
        {
            var container = new Container(r => r.RegisterService<IService>().ImplementedBy<DisposableSpy>());
            var service = container.Resolve<IService>();

            Assert.That(((DisposableSpy)service).IsDisposed, Is.False);
        }
        
        [Test]
        public void ServiceIsDisposedAfterContainerIs()
        {
            var container = new Container(r => r.RegisterService<IService>().ImplementedBy<DisposableSpy>());
            var service = container.Resolve<IService>();

            container.Dispose();
            
            Assert.That(((DisposableSpy)service).IsDisposed, Is.True);
        }
        
        [Test]
        public void DependencyIsNotDisposedBeforeContainerIs()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IService>().ImplementedBy<DisposableSpy>();
                r.RegisterService<ServiceDependingOnDisposable>().ImplementedBy<ServiceDependingOnDisposable>();
            });
            var service = container.Resolve<ServiceDependingOnDisposable>();

            Assert.That(((DisposableSpy)service.Dependency).IsDisposed, Is.False);
        }
        
        [Test]
        public void DependencyIsDisposedAfterContainerIs()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IService>().ImplementedBy<DisposableSpy>();
                r.RegisterService<ServiceDependingOnDisposable>().ImplementedBy<ServiceDependingOnDisposable>();
            });
            var service = container.Resolve<ServiceDependingOnDisposable>();

            container.Dispose();
            
            Assert.That(((DisposableSpy)service.Dependency).IsDisposed, Is.True);
        }
        
        [Test]
        [SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
        public void UsingDisposedContainerThrows()
        {
            var container = new Container(_ => { });
            container.Dispose();

            TestDelegate when = () => container.Resolve<object>();
            
            Assert.That(when, Throws.Exception.InstanceOf<ObjectDisposedException>());
        }
        
        private interface IService
        {
        }
        
        private class DisposableSpy : IService, IDisposable
        {
            public bool IsDisposed { get; private set; }

            public void Dispose()
            {
                IsDisposed = true;
            }
        }

        private class ServiceDependingOnDisposable
        {
            public IService Dependency { get; }

            public ServiceDependingOnDisposable(IService dependency)
            {
                Dependency = dependency;
            }
        }
    }
}