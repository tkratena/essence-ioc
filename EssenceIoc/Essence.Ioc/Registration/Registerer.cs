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
        private readonly RegisteredServices _registeredServices = new RegisteredServices();
        private readonly RegisteredServices _registeredGenericServices = new RegisteredServices();
        private readonly Factories _factories;
        private readonly Resolver _resolver;
        private readonly ILifeScope _singletonLifeScope;

        public Registerer(Factories factories, Resolver resolver, ILifeScope singletonLifeScope)
        {
            _factories = factories;
            _resolver = resolver;
            _singletonLifeScope = singletonLifeScope;
        }

        public void RegisterTransient(Type implementationType, IEnumerable<Type> serviceTypes)
        {
            foreach (var serviceType in serviceTypes)
            {
                RegisterTransient(implementationType, serviceType);
            }
        }

        private void RegisterTransient(Type implementationType, Type serviceType)
        {
            _registeredServices.MarkRegistered(serviceType);

            var factoryExpression = CreateFactoryExpression(implementationType);
            _factories.AddFactory(serviceType, factoryExpression);
        }

        private IFactoryExpression CreateFactoryExpression(Type implementationType)
        {
            return new Implementation(implementationType).Resolve(_factories);
        }

        public void RegisterSingleton(Type implementationType, IEnumerable<Type> serviceTypes)
        {
            var factoryExpression = CreateFactoryExpression(implementationType);
            var nestedLifeScope = _singletonLifeScope.CreateNestedScope();
            
            var singleton = new Lazy<object>(() =>
            {
                var factory = factoryExpression.Compile<object>();
                return factory.Invoke(nestedLifeScope);
            });
            
            RegisterFactory(transientLifeScope => singleton.Value, serviceTypes);
        }

        public void RegisterFactorySingleton<TImplementation>(
            Func<IContainer, TImplementation> factory,
            IEnumerable<Type> serviceTypes)
            where TImplementation : class
        {
            var container = CreateContainer(_singletonLifeScope);
            RegisterFactorySingleton(() => factory.Invoke(container), serviceTypes);
        }

        private IContainer CreateContainer(ILifeScope lifeScope)
        {
            return new LifeScopedResolver(lifeScope, _resolver);
        }

        public void RegisterFactorySingleton<TImplementation>(
            Func<TImplementation> factory,
            IEnumerable<Type> serviceTypes)
            where TImplementation : class
        {
            var scopedFactory = factory.WithTracking(_singletonLifeScope);
            var singleton = new Lazy<object>(scopedFactory);
            
            RegisterFactory(transientLifeScope => singleton.Value, serviceTypes);
        }

        public void RegisterFactoryTransient<TImplementation>(
            Func<IContainer, TImplementation> factory,
            IEnumerable<Type> serviceTypes)
            where TImplementation : class
        {
            RegisterFactoryTransient(lifeScope => factory.Invoke(CreateContainer(lifeScope)), serviceTypes);
        }

        public void RegisterFactoryTransient<TImplementation>(
            Func<TImplementation> factory,
            IEnumerable<Type> serviceTypes)
            where TImplementation : class
        {
            RegisterFactoryTransient((ILifeScope _) => factory.Invoke(), serviceTypes);
        }
        
        private void RegisterFactoryTransient<TImplementation>(
            Func<ILifeScope, TImplementation> factory,
            IEnumerable<Type> serviceTypes)
            where TImplementation : class
        {
            RegisterFactory(factory.ConstructWithTracking, serviceTypes);
        }

        private void RegisterFactory(Func<ILifeScope, object> factory, IEnumerable<Type> serviceTypes)
        {
            foreach (var serviceType in serviceTypes)
            {
                RegisterFactory(factory, serviceType);
            }
        }

        private void RegisterFactory(Func<ILifeScope, object> factory, Type serviceType)
        {
            _registeredServices.MarkRegistered(serviceType);
            _factories.AddFactory(serviceType, new CompiledFactoryExpression(factory, serviceType));
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

            _registeredGenericServices.MarkRegistered(serviceGenericTypeDefinition);
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