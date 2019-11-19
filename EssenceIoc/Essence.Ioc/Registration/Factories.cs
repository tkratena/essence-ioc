using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Essence.Ioc.Expressions;
using Essence.Ioc.Resolution;

namespace Essence.Ioc.Registration
{
    internal class Factories : IFactoryFinder
    {
        private readonly IDictionary<Type, IFactoryExpression> _factories = new ConcurrentDictionary<Type, IFactoryExpression>();
        private readonly IDictionary<Type, Type> _genericImplementations = new ConcurrentDictionary<Type, Type>();

        public void AddFactory(Type serviceType, IFactoryExpression factoryExpression)
        {
            _factories.Add(serviceType, factoryExpression);
        }

        public void AddGenericImplementation(Type serviceType, Type implementationType)
        {
            _genericImplementations.Add(serviceType, implementationType);
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