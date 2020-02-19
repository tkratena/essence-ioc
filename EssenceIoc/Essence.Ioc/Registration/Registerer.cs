using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Essence.Framework.System;
using Essence.Ioc.Expressions;
using Essence.Ioc.ExtendableRegistration;
using Essence.Ioc.LifeCycleManagement;
using Essence.Ioc.Registration.RegistrationExceptions;
using Essence.Ioc.Resolution;
using Essence.Ioc.TypeModel;

namespace Essence.Ioc.Registration
{
    internal class Registerer : IRegisterer
    {
        private readonly Factories _factories;
        private readonly Resolver _resolver;
        private readonly SingletonFactory _singletonFactory;

        public Registerer(Factories factories, Resolver resolver, ILifeScope singletonLifeScope)
        {
            _factories = factories;
            _resolver = resolver;
            _singletonFactory = new SingletonFactory(singletonLifeScope);
        }

        public void RegisterTransient(Type implementationType, IEnumerable<Type> serviceTypes)
        {
            Register(Resolve(implementationType), serviceTypes);
        }

        public void RegisterFactoryTransient<TImplementation>(
            Func<IContainer, TImplementation> factory,
            IEnumerable<Type> serviceTypes)
            where TImplementation : class
        {
            Register(Resolve(factory), serviceTypes);
        }

        public void RegisterFactoryTransient<TImplementation>(
            Func<TImplementation> factory,
            IEnumerable<Type> serviceTypes)
            where TImplementation : class
        {
            Register(Resolve(factory), serviceTypes);
        }

        private void Register(IFactoryExpression factoryExpression, IEnumerable<Type> serviceTypes)
        {
            foreach (var serviceType in serviceTypes)
            {
                AssertServiceIsNotDisposable(serviceType);
                _factories.AddFactory(serviceType, factoryExpression);
            }
        }
        
        private static void AssertServiceIsNotDisposable(Type serviceType)
        {
            if (typeof(IDisposable).GetTypeInfo().IsAssignableFrom(serviceType))
            {
                throw new DisposableServiceException(serviceType);
            }
        }

        public void RegisterSingleton(Type implementationType, IEnumerable<Type> serviceTypes)
        {
            RegisterSingleton(Resolve(implementationType), implementationType, serviceTypes);
        }

        public void RegisterFactorySingleton<TImplementation>(
            Func<IContainer, TImplementation> factory,
            IEnumerable<Type> serviceTypes)
            where TImplementation : class
        {
            RegisterSingleton(Resolve(factory), typeof(TImplementation), serviceTypes);
        }

        public void RegisterFactorySingleton<TImplementation>(
            Func<TImplementation> factory,
            IEnumerable<Type> serviceTypes)
            where TImplementation : class
        {
            RegisterSingleton(Resolve(factory), typeof(TImplementation), serviceTypes);
        }

        private void RegisterSingleton(
            IFactoryExpression factoryExpression,
            Type implementationType,
            IEnumerable<Type> serviceTypes)
        {
            var singleton = _singletonFactory.MakeSingleton(factoryExpression, implementationType);
            Register(singleton, serviceTypes);
        }

        private IFactoryExpression Resolve(Type implementationType)
        {
            return new Implementation(implementationType).Resolve(_factories);
        }

        private IFactoryExpression Resolve<TImplementation>(Func<IContainer, TImplementation> factory)
            where TImplementation : class
        {
            return Resolve(lifeScope => ConstructWithTracking(factory, lifeScope));
        }

        private T ConstructWithTracking<T>(Func<IContainer, T> factory, ILifeScope lifeScope)
        {
            var container = new LifeScopedResolver(lifeScope, _resolver);
            Func<T> factoryWithContainer = () => factory.Invoke(container);
            return factoryWithContainer.ConstructWithTracking(lifeScope);
        }

        private static IFactoryExpression Resolve<TImplementation>(Func<TImplementation> factory)
            where TImplementation : class
        {
            return Resolve(factory.ConstructWithTracking);
        }

        private static IFactoryExpression Resolve<TImplementation>(Func<ILifeScope, TImplementation> factory)
            where TImplementation : class
        {
            return new CompiledFactoryExpression(factory, typeof(TImplementation));
        }

        public void RegisterGeneric(
            Type implementationGenericTypeDefinition,
            IEnumerable<Type> serviceGenericTypeDefinitions)
        {
            if (!implementationGenericTypeDefinition.GetTypeInfo().IsGenericTypeDefinition)
            {
                throw new ImplementationTypeNotGenericTypeDefinitionException(implementationGenericTypeDefinition);
            }

            foreach (var serviceGenericTypeDefinition in serviceGenericTypeDefinitions)
            {
                RegisterGeneric(implementationGenericTypeDefinition, serviceGenericTypeDefinition);
            }
        }

        private void RegisterGeneric(Type implementationGenericTypeDefinition, Type serviceGenericTypeDefinition)
        {
            if (!serviceGenericTypeDefinition.GetTypeInfo().IsGenericTypeDefinition)
            {
                throw new ServiceTypeNotGenericTypeDefinitionException();
            }

            if (!IsGenericServiceAssignableFromImplementation(
                serviceGenericTypeDefinition,
                implementationGenericTypeDefinition))
            {
                throw new ImplementationTypeNotImplementingGenericService(
                    implementationGenericTypeDefinition,
                    serviceGenericTypeDefinition);
            }
            
            AssertServiceIsNotDisposable(serviceGenericTypeDefinition);
            _factories.AddGenericImplementation(serviceGenericTypeDefinition, implementationGenericTypeDefinition);
        }

        private static bool IsGenericServiceAssignableFromImplementation(
            Type serviceGenericTypeDefinition,
            Type implementationGenericTypeDefinition)
        {
            var serviceTypes = implementationGenericTypeDefinition.GetSuperTypes()
                .Where(t => t.GetTypeInfo().IsGenericType)
                .Where(t => t.GetGenericTypeDefinition() == serviceGenericTypeDefinition);

            return serviceTypes.Any(i => AreGenericArgumentsMatching(i, implementationGenericTypeDefinition));
        }

        private static bool AreGenericArgumentsMatching(Type a, Type b)
        {
            return a.GetTypeInfo().GetGenericArguments().SequenceEqual(b.GetTypeInfo().GetGenericArguments());
        }
    }
}