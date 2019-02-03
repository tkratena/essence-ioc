using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Essence.Framework;
using Essence.Ioc.Expressions;
using Essence.Ioc.ExtendableRegistration;
using Essence.Ioc.Registration.RegistrationExceptions;
using Essence.Ioc.TypeModel;

namespace Essence.Ioc.Registration
{
    internal class Registerer : IRegisterer
    {
        private readonly RegisteredServices _registeredServices = new RegisteredServices();
        private readonly RegisteredServices _registeredGenericServices = new RegisteredServices();
        private readonly Factories _factories;
        private readonly IContainer _container;

        public Registerer(Factories factories, IContainer container)
        {
            _factories = factories;
            _container = container;
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
            _factories.AddFactoryExpression(serviceType, factoryExpression);
        }

        public void RegisterSingleton(Type implementationType, IEnumerable<Type> serviceTypes)
        {
            var factoryExpression = CreateFactoryExpression(implementationType);
            var factory = factoryExpression.Compile<object>();
            
            RegisterFactorySingleton(factory, serviceTypes);
        }
        
        public void RegisterFactorySingleton<TImplementation>(
            Func<IContainer, TImplementation> factory, 
            IEnumerable<Type> serviceTypes)
            where TImplementation : class
        {
            RegisterFactorySingleton(() => factory.Invoke(_container), serviceTypes);
        }

        public void RegisterFactorySingleton<TImplementation>(
            Func<TImplementation> factory,
            IEnumerable<Type> serviceTypes)
            where TImplementation : class
        {
            var singleton = new Lazy<TImplementation>(factory.Invoke);

            RegisterFactoryTransient(() => singleton.Value, serviceTypes);
        }
        
        private IFactoryExpression CreateFactoryExpression(Type implementationType)
        {
            return new Implementation(implementationType).Resolve(_factories);
        }
        
        public void RegisterFactoryTransient<TImplementation>(
            Func<IContainer, TImplementation> factory, 
            IEnumerable<Type> serviceTypes)
            where TImplementation : class
        {
            RegisterFactoryTransient(() => factory.Invoke(_container), serviceTypes);
        }

        public void RegisterFactoryTransient<TImplementation>(
            Func<TImplementation> factory,
            IEnumerable<Type> serviceTypes)
            where TImplementation : class
        {
            foreach (var serviceType in serviceTypes)
            {
                RegisterFactoryTransient(factory, serviceType);
            }
        }

        private void RegisterFactoryTransient(Delegate factory, Type serviceType)
        {
            if (typeof(IDisposable).GetTypeInfo().IsAssignableFrom(serviceType))
            {
                throw new DisposableClassException(serviceType);
            }

            _registeredServices.MarkRegistered(serviceType);
            _factories.AddFactory(serviceType, factory);
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