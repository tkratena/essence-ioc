using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using Essence.Ioc.ExtendableRegistration;
using Essence.Ioc.FluentRegistration;
using NUnit.Framework;

namespace Essence.Ioc
{
    [TestFixture]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    [SuppressMessage("ReSharper", "UnusedTypeParameter")]
    public class MultipleServiceResolutionTests
    {
        [Test]
        public void TenServicesImplementedBySameClass()
        {
            var container = new Container(r =>
                r.RegisterService<IService1>()
                    .AndService<IService2>()
                    .AndService<IService3>()
                    .AndService<IService4>()
                    .AndService<IService5>()
                    .AndService<IService6>()
                    .AndService<IService7>()
                    .AndService<IService8>()
                    .AndService<IService9>()
                    .AndService<IService10>()
                    .ImplementedBy<MultipleServiceImplementation>());

            var resolvedServices = new object[]
            {
                container.Resolve<IService1>(),
                container.Resolve<IService2>(),
                container.Resolve<IService3>(),
                container.Resolve<IService4>(),
                container.Resolve<IService5>(),
                container.Resolve<IService6>(),
                container.Resolve<IService7>(),
                container.Resolve<IService8>(),
                container.Resolve<IService9>(),
                container.Resolve<IService10>()
            };

            Assert.That(resolvedServices, Is.Unique.And.All.InstanceOf<MultipleServiceImplementation>());
        }

        [Test]
        public void MultipleGenericServicesImplementedBySameClass()
        {
            var container = new Container(r =>
                r.GenericallyRegisterService(typeof(IGenericService1<>))
                    .AndService(typeof(IGenericService2<>))
                    .ImplementedBy(typeof(MultipleGenericServiceImplementation<>)));

            var resolvedServices = new object[]
            {
                container.Resolve<IGenericService1<IActualGenericArgument>>(),
                container.Resolve<IGenericService2<IActualGenericArgument>>()
            };

            Assert.That(
                resolvedServices,
                Is.Unique.And.All.InstanceOf<MultipleGenericServiceImplementation<IActualGenericArgument>>());
        }

        private static readonly IReadOnlyCollection<Type> AllServiceInterfaces = new[]
        {
            typeof(IService1),
            typeof(IService2),
            typeof(IService3),
            typeof(IService4),
            typeof(IService5),
            typeof(IService6),
            typeof(IService7),
            typeof(IService8),
            typeof(IService9),
            typeof(IService10)
        };

        public static IEnumerable<IReadOnlyCollection<Type>> MultipleServiceCases => Enumerable
            .Range(1, 10)
            .Select(serviceCount => new MultipleServices(AllServiceInterfaces.Take(serviceCount).ToList()));

        [Test]
        public void MultipleServicesImplementedBySameClass(
            [ValueSource(nameof(MultipleServiceCases))] IReadOnlyCollection<Type> serviceInterfaces)
        {
            var resolvedServices = ResolveUsingContainerWithMultipleServiceRegistration(
                serviceInterfaces,
                callTarget =>
                    Expression.Call(
                        callTarget,
                        nameof(IService<object>.ImplementedBy),
                        new[] {typeof(MultipleServiceImplementation)}));

            Assert.That(resolvedServices, Is.Unique.And.All.InstanceOf<MultipleServiceImplementation>());
        }
        
        [Test]
        public void MultipleServicesImplementedBySameClassAsSingleton(
            [ValueSource(nameof(MultipleServiceCases))] IReadOnlyCollection<Type> serviceInterfaces)
        {
            var resolvedServices = ResolveUsingContainerWithMultipleServiceRegistration(
                serviceInterfaces,
                callTarget =>
                    Expression.Call(
                        Expression.Call(
                            callTarget,
                            nameof(IService<object>.ImplementedBy),
                            new[] {typeof(MultipleServiceImplementation)}),
                        nameof(ILifeScope.AsSingleton),
                        new Type[0]));

            var first = resolvedServices.First();
            Assert.That(resolvedServices, Is.All.InstanceOf<MultipleServiceImplementation>().And.SameAs(first));
        }
        
        [Test]
        public void MultipleServicesConstructedBySameCustomFactory(
            [ValueSource(nameof(MultipleServiceCases))] IReadOnlyCollection<Type> serviceInterfaces)
        {
            var resolvedServices = ResolveUsingContainerWithMultipleServiceRegistration(
                serviceInterfaces,
                callTarget =>
                    Expression.Call(
                        callTarget,
                        nameof(IService<object>.ConstructedBy),
                        new[] { typeof(MultipleServiceImplementation) },
                        Expression.Constant((Func<MultipleServiceImplementation>) (() =>
                            new MultipleServiceImplementation()))));

            Assert.That(resolvedServices, Is.Unique.And.All.InstanceOf<MultipleServiceImplementation>());
        }
        
        [Test]
        public void MultipleServicesConstructedBySameCustomFactoryAsSingleton(
            [ValueSource(nameof(MultipleServiceCases))] IReadOnlyCollection<Type> serviceInterfaces)
        {
            var resolvedServices = ResolveUsingContainerWithMultipleServiceRegistration(
                serviceInterfaces,
                callTarget =>
                    Expression.Call(
                        Expression.Call(
                            callTarget,
                            nameof(IService<object>.ConstructedBy),
                            new[] { typeof(MultipleServiceImplementation) },
                            Expression.Constant((Func<MultipleServiceImplementation>) (() =>
                                new MultipleServiceImplementation()))),
                        nameof(ILifeScope.AsSingleton),
                        new Type[0]));

            var first = resolvedServices.First();
            Assert.That(resolvedServices, Is.All.InstanceOf<MultipleServiceImplementation>().And.SameAs(first));
        }
        
        [Test]
        public void MultipleServicesConstructedBySameCustomFactoryThatUsesContainer(
            [ValueSource(nameof(MultipleServiceCases))] IReadOnlyCollection<Type> serviceInterfaces)
        {
            var resolvedServices = ResolveUsingContainerWithMultipleServiceRegistration(
                serviceInterfaces,
                callTarget =>
                    Expression.Call(
                        callTarget,
                        nameof(IService<object>.ConstructedBy),
                        new[] { typeof(MultipleServiceImplementation) },
                        Expression.Constant((Func<IContainer, MultipleServiceImplementation>) (_ => 
                            new MultipleServiceImplementation()))));

            Assert.That(resolvedServices, Is.Unique.And.All.InstanceOf<MultipleServiceImplementation>());
        }
        
        [Test]
        public void MultipleServicesConstructedBySameCustomFactoryThatUsesContainerAsSingleton(
            [ValueSource(nameof(MultipleServiceCases))] IReadOnlyCollection<Type> serviceInterfaces)
        {
            var resolvedServices = ResolveUsingContainerWithMultipleServiceRegistration(
                serviceInterfaces,
                callTarget =>
                    Expression.Call(
                        Expression.Call(
                            callTarget,
                            nameof(IService<object>.ConstructedBy),
                            new[] { typeof(MultipleServiceImplementation) },
                            Expression.Constant((Func<IContainer, MultipleServiceImplementation>) (_ => 
                                new MultipleServiceImplementation()))),
                        nameof(ILifeScope.AsSingleton),
                        new Type[0]));

            var first = resolvedServices.First();
            Assert.That(resolvedServices, Is.All.InstanceOf<MultipleServiceImplementation>().And.SameAs(first));
        }

        private static IReadOnlyCollection<object> ResolveUsingContainerWithMultipleServiceRegistration(
            IReadOnlyCollection<Type> serviceInterfaces,
            Func<Expression, Expression> implementedByCallExpressionProvider)
        {
            var registererParameter = Expression.Parameter(typeof(Registerer));

            Expression registerServiceCall = registererParameter;
            
            registerServiceCall = Expression.Call(
                typeof(FluentRegistererExtensions),
                nameof(FluentRegistererExtensions.RegisterService),
                new[] {serviceInterfaces.First()},
                registerServiceCall);
            
            foreach (var serviceInterface in serviceInterfaces.Skip(1))
            {
                var methodName = nameof(IService<object>.AndService);
                registerServiceCall = Expression.Call(registerServiceCall, methodName, new[] {serviceInterface});
            }

            var implementedByCall = implementedByCallExpressionProvider.Invoke(registerServiceCall);

            var serviceRegistration = Expression.Lambda<Action<Registerer>>(implementedByCall, registererParameter);
            var container = new Container(serviceRegistration.Compile());

            var resolvedServices = serviceInterfaces.Select(serviceInterface =>
                Expression
                    .Lambda<Func<object>>(Expression.Call(
                        Expression.Constant(container),
                        nameof(Container.Resolve),
                        new[] {serviceInterface}))
                    .Compile()
                    .Invoke());
            
            return resolvedServices.ToList();
        }

        private class MultipleServiceImplementation :
            IService1,
            IService2,
            IService3,
            IService4,
            IService5,
            IService6,
            IService7,
            IService8,
            IService9,
            IService10
        {
        }

        private interface IService1
        {
        }

        private interface IService2
        {
        }

        private interface IService3
        {
        }

        private interface IService4
        {
        }

        private interface IService5
        {
        }

        private interface IService6
        {
        }

        private interface IService7
        {
        }

        private interface IService8
        {
        }

        private interface IService9
        {
        }

        private interface IService10
        {
        }

        private class MultipleGenericServiceImplementation<T> : IGenericService1<T>, IGenericService2<T>
        {
        }

        private interface IGenericService1<T>
        {
        }

        private interface IGenericService2<T>
        {
        }

        private interface IActualGenericArgument
        {
        }

        private class MultipleServices : IReadOnlyCollection<Type>
        {
            private readonly IReadOnlyCollection<Type> _services;

            public MultipleServices(IReadOnlyCollection<Type> services)
            {
                _services = services;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<Type> GetEnumerator()
            {
                return _services.GetEnumerator();
            }

            public int Count => _services.Count;

            public override string ToString()
            {
                return _services.Count.ToString();
            }
        }
    }
}