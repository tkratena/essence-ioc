using System;
using System.Diagnostics.CodeAnalysis;
using Essence.Ioc.FluentRegistration;
using NUnit.Framework;

namespace Essence.Ioc.LifeCycleManagement
{
    [TestFixture]
    [SuppressMessage("ReSharper", "AssignmentIsFullyDiscarded")]
    public class InvalidResolutionAfterDisposalTests
    {
        [Test]
        public void ServiceResolvingThrowsAfterContainerIsDisposed()
        {
            var container = new Container(_ => { });
            container.Dispose();

            TestDelegate when = () => _ = container.Resolve<object>(out _);
            
            Assert.That(when, Throws.Exception.InstanceOf<ObjectDisposedException>());
        }

        [TestFixture]
        private class TransientDependency
        {
            private IDependencySpy _dependencySpy;

            [SetUp]
            public void SetUp()
            {
                var container = new Container(r =>
                {
                    r.RegisterService<IDependency>().ImplementedBy<Disposable>();
                    r.RegisterService<IDependencySpy>().ImplementedBy<DependencySpy>();
                });
                
                var transientLifeScope = container.Resolve(out _dependencySpy);
                transientLifeScope.Dispose();
            }
            
            [Test]
            public void TransientLazyUsageThrowsAfterTransientLifeScopeIsDisposed()
            {
                TestDelegate when = () => _ = _dependencySpy.LazyDependency.Value;
            
                Assert.That(when, Throws.Exception.InstanceOf<ObjectDisposedException>());
            }
            
            [Test]
            public void TransientFactoryUsageThrowsAfterTransientLifeScopeIsDisposed()
            {
                TestDelegate when = () => _dependencySpy.DependencyFactory.Invoke();
            
                Assert.That(when, Throws.Exception.InstanceOf<ObjectDisposedException>());
            }
        }
        
        [TestFixture]
        private class SingletonDependency
        {
            private IDependencySpy _dependencySpy;

            [SetUp]
            public void SetUp()
            {
                var container = new Container(r =>
                {
                    r.RegisterService<IDependency>().ImplementedBy<Disposable>().AsSingleton();
                    r.RegisterService<IDependencySpy>().ImplementedBy<DependencySpy>();
                });
                
                _ = container.Resolve(out _dependencySpy);
                container.Dispose();
            }
            
            [Test]
            public void SingletonLazyUsageThrowsAfterTransientLifeScopeIsDisposed()
            {
                TestDelegate when = () => _ = _dependencySpy.LazyDependency.Value;
            
                Assert.That(when, Throws.Exception.InstanceOf<ObjectDisposedException>());
            }
            
            [Test]
            public void SingletonFactoryUsageThrowsAfterTransientLifeScopeIsDisposed()
            {
                TestDelegate when = () => _dependencySpy.DependencyFactory.Invoke();
            
                Assert.That(when, Throws.Exception.InstanceOf<ObjectDisposedException>());
            }
        }
        
        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        private class DependencySpy : IDependencySpy
        {
            public Lazy<IDependency> LazyDependency { get; }
            public Func<IDependency> DependencyFactory { get; }

            public DependencySpy(Lazy<IDependency> lazyDependency, Func<IDependency> dependencyFactory)
            {
                LazyDependency = lazyDependency;
                DependencyFactory = dependencyFactory;
            }
        }

        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        private class Disposable : IDependency, IDisposable
        {
            public void Dispose()
            {
            }
        }

        private interface IDependencySpy
        {
            Lazy<IDependency> LazyDependency { get; }
            Func<IDependency> DependencyFactory { get; }
        }

        private interface IDependency
        {
        }
    }
}