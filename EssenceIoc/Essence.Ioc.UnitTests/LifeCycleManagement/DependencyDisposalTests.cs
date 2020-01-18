using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Essence.Ioc.FluentRegistration;
using NUnit.Framework;

namespace Essence.Ioc.LifeCycleManagement
{
    [TestFixture]
    [SuppressMessage("ReSharper", "AssignmentIsFullyDiscarded")]
    public class DependencyDisposalTests
    {
        private static IEnumerable<TestRegistrationType> DependentServiceRegistrationTypes => new[]
        {
            new TestRegistrationType("Dependent on service", r =>
                r.RegisterService<IDependencySpyService>().ImplementedBy<ClassDependentOnService>()),

            new TestRegistrationType("Dependent on lazy service", r =>
                r.RegisterService<IDependencySpyService>().ImplementedBy<ClassDependentOnLazyService>()),

            new TestRegistrationType("Dependent on service factory", r =>
                r.RegisterService<IDependencySpyService>().ImplementedBy<ClassDependentOnServiceFactory>()),

            new TestRegistrationType("Dependent on custom service resolved by container", r =>
                r.RegisterService<IDependencySpyService>().ConstructedBy(c =>
                    new ClassDependentOnService(c.Resolve<IDependency>()))),

            new TestRegistrationType("Dependent on custom lazy service resolved by container", r =>
                r.RegisterService<IDependencySpyService>().ConstructedBy(c =>
                    new ClassDependentOnLazyService(c.Resolve<Lazy<IDependency>>()))),

            new TestRegistrationType("Dependent on custom service factory resolved by container", r =>
                r.RegisterService<IDependencySpyService>().ConstructedBy(c =>
                    new ClassDependentOnServiceFactory(c.Resolve<Func<IDependency>>()))),

            new TestRegistrationType("Dependent on custom lazy service using container", r =>
                r.RegisterService<IDependencySpyService>().ConstructedBy(c =>
                    new ClassDependentOnLazyService(new Lazy<IDependency>(c.Resolve<IDependency>)))),

            new TestRegistrationType("Dependent on custom service factory using container", r =>
                r.RegisterService<IDependencySpyService>().ConstructedBy(c =>
                    new ClassDependentOnServiceFactory(c.Resolve<IDependency>)))
        };

        private static IEnumerable<TestRegistrationType> DependencyRegistrationTypes => new[]
        {
            new TestRegistrationType("Implementation", r =>
                r.RegisterService<IDependency>().ImplementedBy<DisposableSpy>()),

            new TestRegistrationType("Custom factory", r =>
                r.RegisterService<IDependency>().ConstructedBy(() => new DisposableSpy())),

            new TestRegistrationType("Custom factory using container", r =>
                r.RegisterService<IDependency>().ConstructedBy(_ => new DisposableSpy())),

            new TestRegistrationType("Custom factory not returning the instance as disposable", r =>
                r.RegisterService<IDependency>().ConstructedBy(() => (NonDisposable) new DisposableSpy()))
        };

        [Test]
        public void TransientDependencyIsDisposedWithTransientLifeScope(
            [ValueSource(nameof(DependentServiceRegistrationTypes))] TestRegistrationType dependentServiceRegistration,
            [ValueSource(nameof(DependencyRegistrationTypes))] TestRegistrationType dependencyRegistration)
        {
            var container = new Container(r =>
            {
                dependencyRegistration.Invoke(r);
                dependentServiceRegistration.Invoke(r);
            });
            var transientLifeScope = container.Resolve<IDependencySpyService>(out var dependencySpy);
            var disposableSpy = dependencySpy.ActualDependency;

            transientLifeScope.Dispose();

            Assert.That(disposableSpy, Has.Property(nameof(DisposableSpy.IsDisposed)).True);
        }

        [Test]
        public void TransientDependencyIsNotDisposedWithContainer(
            [ValueSource(nameof(DependentServiceRegistrationTypes))] TestRegistrationType dependentServiceRegistration,
            [ValueSource(nameof(DependencyRegistrationTypes))] TestRegistrationType dependencyRegistration)
        {
            var container = new Container(r =>
            {
                dependencyRegistration.Invoke(r);
                dependentServiceRegistration.Invoke(r);
            });
            _ = container.Resolve<IDependencySpyService>(out var dependencySpy);
            var disposableSpy = dependencySpy.ActualDependency;

            container.Dispose();

            Assert.That(disposableSpy, Has.Property(nameof(DisposableSpy.IsDisposed)).False);
        }

        [Test]
        public void TransientDependencyIsDisposedOnlyOnce(
            [ValueSource(nameof(DependentServiceRegistrationTypes))] TestRegistrationType dependentServiceRegistration,
            [ValueSource(nameof(DependencyRegistrationTypes))] TestRegistrationType dependencyRegistration)
        {
            var container = new Container(r =>
            {
                dependencyRegistration.Invoke(r);
                dependentServiceRegistration.Invoke(r);
            });
            var transientLifeScope = container.Resolve<IDependencySpyService>(out var dependencySpy);
            var disposableSpy = dependencySpy.ActualDependency;

            transientLifeScope.Dispose();
            transientLifeScope.Dispose();

            Assert.That(disposableSpy, Has.Property(nameof(DisposableSpy.DisposeCount)).EqualTo(1));
        }

        private static IEnumerable<TestDependencyLifeStyleRegistration> SingletonRegistrationTypes => new[]
        {
            new TestDependencyLifeStyleRegistration(
                "Singleton dependency of a transient service",
                (dependency, dependentService) =>
                {
                    dependency().AsSingleton();
                    dependentService();
                }),

            new TestDependencyLifeStyleRegistration(
                "Transient dependency of a singleton service",
                (dependency, dependentService) =>
                {
                    dependency();
                    dependentService().AsSingleton();
                }),

            new TestDependencyLifeStyleRegistration(
                "Singleton dependency of a singleton service",
                (dependency, dependentService) =>
                {
                    dependency().AsSingleton();
                    dependentService().AsSingleton();
                })
        };

        [Test]
        public void SingletonDependencyIsNotDisposedWithTransientLifeScope(
            [ValueSource(nameof(SingletonRegistrationTypes))] TestDependencyLifeStyleRegistration singletonRegistration,
            [ValueSource(nameof(DependentServiceRegistrationTypes))] TestRegistrationType dependentServiceRegistration,
            [ValueSource(nameof(DependencyRegistrationTypes))] TestRegistrationType dependencyRegistration)
        {
            var container = new Container(r => singletonRegistration.Invoke(
                () => dependencyRegistration.Invoke(r),
                () => dependentServiceRegistration.Invoke(r)));
            var transientLifeScope = container.Resolve<IDependencySpyService>(out _);
            transientLifeScope.Dispose();

            container.Resolve<IDependencySpyService>(out var dependencySpy);
            var disposableSpy = dependencySpy.ActualDependency;

            Assert.That(disposableSpy, Has.Property(nameof(DisposableSpy.IsDisposed)).False);
        }

        [Test]
        public void SingletonDependencyIsDisposedWithContainer(
            [ValueSource(nameof(SingletonRegistrationTypes))] TestDependencyLifeStyleRegistration singletonRegistration,
            [ValueSource(nameof(DependentServiceRegistrationTypes))] TestRegistrationType dependentServiceRegistration,
            [ValueSource(nameof(DependencyRegistrationTypes))] TestRegistrationType dependencyRegistration)
        {
            var container = new Container(r => singletonRegistration.Invoke(
                () => dependencyRegistration.Invoke(r),
                () => dependentServiceRegistration.Invoke(r)));
            var transientLifeScope = container.Resolve<IDependencySpyService>(out _);
            transientLifeScope.Dispose();
            transientLifeScope = container.Resolve<IDependencySpyService>(out var dependencySpy);
            var disposableSpy = dependencySpy.ActualDependency;
            transientLifeScope.Dispose();

            container.Dispose();

            Assert.That(disposableSpy, Has.Property(nameof(DisposableSpy.IsDisposed)).True);
        }

        [Test]
        public void SingletonDependencyIsDisposedOnlyOnce(
            [ValueSource(nameof(SingletonRegistrationTypes))] TestDependencyLifeStyleRegistration singletonRegistration,
            [ValueSource(nameof(DependentServiceRegistrationTypes))] TestRegistrationType dependentServiceRegistration,
            [ValueSource(nameof(DependencyRegistrationTypes))] TestRegistrationType dependencyRegistration)
        {
            var container = new Container(r => singletonRegistration.Invoke(
                () => dependencyRegistration.Invoke(r),
                () => dependentServiceRegistration.Invoke(r)));
            var transientLifeScope = container.Resolve<IDependencySpyService>(out var dependencySpy);
            var disposableSpy = dependencySpy.ActualDependency;
            transientLifeScope.Dispose();

            container.Dispose();
            container.Dispose();

            Assert.That(disposableSpy, Has.Property(nameof(DisposableSpy.DisposeCount)).EqualTo(1));
        }

        private static IEnumerable<TestRegistrationType> DisposableConsumerRegistrationTypes => new[]
        {
            new TestRegistrationType("Dependent on service", r =>
                r.RegisterService<IDependencySpyService>().ImplementedBy<DisposableClassDependentOnService>()),

            new TestRegistrationType("Dependent on lazy service", r =>
                r.RegisterService<IDependencySpyService>().ImplementedBy<DisposableClassDependentOnLazyService>()),

            new TestRegistrationType("Dependent on service factory", r =>
                r.RegisterService<IDependencySpyService>().ImplementedBy<DisposableClassDependentOnServiceFactory>()),

            new TestRegistrationType("Dependent on custom service resolved by container", r =>
                r.RegisterService<IDependencySpyService>().ConstructedBy(c =>
                    new DisposableClassDependentOnService(c.Resolve<IDependency>()))),

            new TestRegistrationType("Dependent on custom lazy service resolved by container", r =>
                r.RegisterService<IDependencySpyService>().ConstructedBy(c =>
                    new DisposableClassDependentOnLazyService(c.Resolve<Lazy<IDependency>>()))),

            new TestRegistrationType("Dependent on custom service factory resolved by container", r =>
                r.RegisterService<IDependencySpyService>().ConstructedBy(c =>
                    new DisposableClassDependentOnServiceFactory(c.Resolve<Func<IDependency>>()))),

            new TestRegistrationType("Dependent on custom lazy service using container", r =>
                r.RegisterService<IDependencySpyService>().ConstructedBy(c =>
                    new DisposableClassDependentOnLazyService(new Lazy<IDependency>(c.Resolve<IDependency>)))),

            new TestRegistrationType("Dependent on custom service factory using container", r =>
                r.RegisterService<IDependencySpyService>().ConstructedBy(c =>
                    new DisposableClassDependentOnServiceFactory(c.Resolve<IDependency>)))
        };

        [Test]
        public void TransientDependencyIsDisposedAfterItsConsumer(
            [ValueSource(nameof(DisposableConsumerRegistrationTypes))] TestRegistrationType consumerRegistration,
            [ValueSource(nameof(DependencyRegistrationTypes))] TestRegistrationType dependencyRegistration)
        {
            var container = new Container(r =>
            {
                dependencyRegistration.Invoke(r);
                consumerRegistration.Invoke(r);
            });
            var transientLifeScope = container.Resolve<IDependencySpyService>(out var disposalOrderSpy);
            _ = disposalOrderSpy.ActualDependency;

            transientLifeScope.Dispose();

            Assert.That(disposalOrderSpy, Has.Property(nameof(DisposalOrderSpy.WasDependencyDisposedBeforeThis)).False);
        }

        [Test]
        public void SingletonDependencyIsDisposedAfterItsConsumer(
            [ValueSource(nameof(SingletonRegistrationTypes))] TestDependencyLifeStyleRegistration singletonRegistration,
            [ValueSource(nameof(DisposableConsumerRegistrationTypes))] TestRegistrationType consumerRegistration,
            [ValueSource(nameof(DependencyRegistrationTypes))] TestRegistrationType dependencyRegistration)
        {
            var container = new Container(r => singletonRegistration.Invoke(
                () => dependencyRegistration.Invoke(r),
                () => consumerRegistration.Invoke(r)));
            var transientLifeScope = container.Resolve<IDependencySpyService>(out var disposalOrderSpy);
            _ = disposalOrderSpy.ActualDependency;

            transientLifeScope.Dispose();
            container.Dispose();

            Assert.That(disposalOrderSpy, Has.Property(nameof(DisposalOrderSpy.WasDependencyDisposedBeforeThis)).False);
        }

        private class ClassDependentOnService : IDependencySpyService
        {
            public IDependency ActualDependency { get; }
            public ClassDependentOnService(IDependency dependency) => ActualDependency = dependency;
        }

        private class ClassDependentOnLazyService : IDependencySpyService
        {
            private readonly Lazy<IDependency> _actualDependency;

            public ClassDependentOnLazyService(Lazy<IDependency> lazyDependency)
                => _actualDependency = lazyDependency;

            public IDependency ActualDependency => _actualDependency.Value;
        }

        private class ClassDependentOnServiceFactory : IDependencySpyService
        {
            private readonly Lazy<IDependency> _actualDependency;

            public ClassDependentOnServiceFactory(Func<IDependency> dependencyFactory)
                => _actualDependency = new Lazy<IDependency>(dependencyFactory);

            public IDependency ActualDependency => _actualDependency.Value;
        }

        private class DisposableClassDependentOnService : DisposalOrderSpy
        {
            public DisposableClassDependentOnService(IDependency dependency)
                : base(new ClassDependentOnService(dependency))
            {
            }
        }

        private class DisposableClassDependentOnLazyService : DisposalOrderSpy
        {
            public DisposableClassDependentOnLazyService(Lazy<IDependency> lazyDependency)
                : base(new ClassDependentOnLazyService(lazyDependency))
            {
            }
        }

        private class DisposableClassDependentOnServiceFactory : DisposalOrderSpy
        {
            public DisposableClassDependentOnServiceFactory(Func<IDependency> dependencyFactory)
                : base(new ClassDependentOnServiceFactory(dependencyFactory))
            {
            }
        }

        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
        private abstract class DisposalOrderSpy : IDependencySpyService, IDisposable
        {
            private readonly IDependencySpyService _dependencySpyService;

            public bool? WasDependencyDisposedBeforeThis { get; private set; }

            protected DisposalOrderSpy(IDependencySpyService dependencySpyService)
            {
                _dependencySpyService = dependencySpyService;
            }

            public IDependency ActualDependency => _dependencySpyService.ActualDependency;

            public void Dispose()
            {
                WasDependencyDisposedBeforeThis = (ActualDependency as DisposableSpy)?.IsDisposed;
            }
        }

        private class DisposableSpy : NonDisposable, IDisposable
        {
            public int DisposeCount { get; private set; }

            public bool IsDisposed => DisposeCount > 0;

            public void Dispose() => DisposeCount++;
        }

        private abstract class NonDisposable : IDependency
        {
        }

        private interface IDependencySpyService
        {
            IDependency ActualDependency { get; }
        }

        public interface IDependency
        {
        }

        public class TestDependencyLifeStyleRegistration : TestCase
        {
            private readonly Registration _registration;

            public delegate void Registration(
                Func<ILifeStyle> dependencyRegistration,
                Func<ILifeStyle> dependentServiceRegistration);

            public TestDependencyLifeStyleRegistration(string description, Registration registration)
                : base(description)
            {
                _registration = registration ?? throw new ArgumentNullException(nameof(registration));
            }

            public void Invoke(Func<ILifeStyle> dependencyRegistration, Func<ILifeStyle> dependentServiceRegistration)
            {
                _registration.Invoke(dependencyRegistration, dependentServiceRegistration);
            }
        }
    }
}