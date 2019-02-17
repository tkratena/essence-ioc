using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Essence.Ioc.Expressions;
using Essence.Ioc.Resolution;

namespace Essence.Ioc.Registration
{
    internal class Factories : IFactoryFinder
    {
        private readonly IDictionary<Type, Delegate> _factories = new ConcurrentDictionary<Type, Delegate>();
        private readonly IDictionary<Type, IFactoryExpression> _factoryExpressions = 
            new ConcurrentDictionary<Type, IFactoryExpression>();
        private readonly IDictionary<Type, Type> _genericImplementations = new ConcurrentDictionary<Type, Type>();

        public void AddFactory(Type serviceType, Delegate factory)
        {
            _factories.Add(serviceType, factory);
        }

        public void AddFactoryExpression(Type serviceType, IFactoryExpression factoryExpression)
        {
            _factoryExpressions.Add(serviceType, factoryExpression);
        }

        public void AddGenericImplementation(Type serviceType, Type implementationType)
        {
            _genericImplementations.Add(serviceType, implementationType);
        }

        public bool TryGetFactory(Type constructedType, out Delegate factory)
        {
            return _factories.TryGetValue(constructedType, out factory);
        }
        
        public bool TryGetFactoryExpression(Type constructedType, out IFactoryExpression factoryExpression)
        {
            return _factoryExpressions.TryGetValue(constructedType, out factoryExpression);
        }

        public bool TryGetGenericType(Type genericServiceTypeDefinition, out Type genericImplementationTypeDefinition)
        {
            return _genericImplementations.TryGetValue(
                genericServiceTypeDefinition,
                out genericImplementationTypeDefinition);
        }
    }
}