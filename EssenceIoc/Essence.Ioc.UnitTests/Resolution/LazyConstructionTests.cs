using System;
using System.Diagnostics.CodeAnalysis;
using Essence.Ioc.FluentRegistration;
using NUnit.Framework;

namespace Essence.Ioc.Resolution
{
    [TestFixture]
    public class LazyConstructionTests
    {
        [SetUp]
        public void ResetConstructionSpy()
        {
            ConstructionSpy.WasConstructed = false;
        }

        [Test]
        public void TransientLazyServiceDoesNotConstructServiceWhenUnused()
        {
            var container = new Container(r => r.RegisterService<IService>().ImplementedBy<ConstructionSpy>());
            container.Resolve<Lazy<IService>>(out _);
            Assert.That(ConstructionSpy.WasConstructed, Is.False);
        }

        [Test]
        public void SingletonLazyServiceDoesNotConstructServiceWhenUnused()
        {
            var container = new Container(r =>
                r.RegisterService<IService>().ImplementedBy<ConstructionSpy>().AsSingleton());
            container.Resolve<Lazy<IService>>(out _);
            Assert.That(ConstructionSpy.WasConstructed, Is.False);
        }

        [Test]
        public void TransientServiceFactoryDoesNotConstructServiceWhenUnused()
        {
            var container = new Container(r => r.RegisterService<IService>().ImplementedBy<ConstructionSpy>());
            container.Resolve<Func<IService>>(out _);
            Assert.That(ConstructionSpy.WasConstructed, Is.False);
        }

        [Test]
        public void SingletonServiceFactoryDoesNotConstructServiceWhenUnused()
        {
            var container = new Container(r =>
                r.RegisterService<IService>().ImplementedBy<ConstructionSpy>().AsSingleton());
            container.Resolve<Func<IService>>(out _);
            Assert.That(ConstructionSpy.WasConstructed, Is.False);
        }

        [Test]
        public void TransientLazyDependencyDoesNotConstructServiceWhenUnused()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IDependency>().ImplementedBy<ConstructionSpy>();
                r.RegisterService<IService>().ImplementedBy<LazyDependencyConsumer>();
            });
            container.Resolve<IService>(out _);
            Assert.That(ConstructionSpy.WasConstructed, Is.False);
        }

        [Test]
        public void SingletonLazyDependencyDoesNotConstructServiceWhenUnused()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IDependency>().ImplementedBy<ConstructionSpy>().AsSingleton();
                r.RegisterService<IService>().ImplementedBy<LazyDependencyConsumer>();
            });
            container.Resolve<IService>(out _);
            Assert.That(ConstructionSpy.WasConstructed, Is.False);
        }

        [Test]
        public void TransientDependencyFactoryDoesNotConstructServiceWhenUnused()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IDependency>().ImplementedBy<ConstructionSpy>();
                r.RegisterService<IService>().ImplementedBy<DependencyFactoryConsumer>();
            });
            container.Resolve<IService>(out _);
            Assert.That(ConstructionSpy.WasConstructed, Is.False);
        }

        [Test]
        public void SingletonDependencyFactoryDoesNotConstructServiceWhenUnused()
        {
            var container = new Container(r =>
            {
                r.RegisterService<IDependency>().ImplementedBy<ConstructionSpy>().AsSingleton();
                r.RegisterService<IService>().ImplementedBy<DependencyFactoryConsumer>();
            });
            container.Resolve<IService>(out _);
            Assert.That(ConstructionSpy.WasConstructed, Is.False);
        }

        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        private class ConstructionSpy : IService, IDependency
        {
            public static bool WasConstructed { get; set; }

            public ConstructionSpy()
            {
                WasConstructed = true;
            }
        }

        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private class LazyDependencyConsumer : IService
        {
            public LazyDependencyConsumer(Lazy<IDependency> dependency)
            {
            }
        }

        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private class DependencyFactoryConsumer : IService
        {
            public DependencyFactoryConsumer(Func<IDependency> dependency)
            {
            }
        }

        private interface IService
        {
        }

        private interface IDependency
        {
        }
    }
}