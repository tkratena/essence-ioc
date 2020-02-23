using System;
using System.Collections.Concurrent;
using Essence.Ioc.Expressions;
using Essence.Ioc.Registration.RegistrationExceptions;
using Essence.Ioc.Resolution;

namespace Essence.Ioc.Registration
{
    internal class Factories : IFactoryFinder
    {
        private readonly ConcurrentDictionary<Type, IFactoryExpression> _factories =
            new ConcurrentDictionary<Type, IFactoryExpression>();

        private readonly ConcurrentDictionary<Type, Type> _genericImplementations =
            new ConcurrentDictionary<Type, Type>();

        public void AddFactory(Type serviceType, IFactoryExpression factoryExpression)
        {
            Add(_factories, serviceType, factoryExpression);
        }

        public void AddGenericImplementation(Type serviceType, Type implementationType)
        {
            Add(_genericImplementations, serviceType, implementationType);
        }

        private static void Add<T>(ConcurrentDictionary<Type, T> dictionary, Type serviceType, T value)
        {
            if (!dictionary.TryAdd(serviceType, value))
            {
                throw new AlreadyRegisteredException(serviceType);
            }
        }

        public bool TryGetFactory(Type constructedType, out IFactoryExpression factoryExpression)
        {
            return _factories.TryGetValue(constructedType, out factoryExpression);
        }

        public bool TryGetGenericType(Type genericServiceTypeDefinition, out Type genericImplementationTypeDefinition)
        {
            return _genericImplementations.TryGetValue(
                genericServiceTypeDefinition,
                out genericImplementationTypeDefinition);
        }
    }
}