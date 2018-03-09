using System;
using System.Collections.Generic;

namespace Essence.Ioc.Resolution
{
    internal class Factories : IFactoryFinder
    {
        private readonly IDictionary<Type, Delegate> _factories = new Dictionary<Type, Delegate>();
        private readonly IDictionary<Type, IFactoryExpression> _factoryExpressions =
            new Dictionary<Type, IFactoryExpression>();
        private readonly IDictionary<Type, Type> _genericImplementations = new Dictionary<Type, Type>();

        public void AddFactory(Type serviceType, Delegate factory)
        {
            _factories.Add(serviceType, factory);
        }

        public void AddFactoryExpression(Type serviceType, IFactoryExpression factoryExpression)
        {
            _factoryExpressions.Add(serviceType, factoryExpression);
        }

        public void AddGenericImplementation(Type servicecType, Type implementationType)
        {
            _genericImplementations.Add(servicecType, implementationType);
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