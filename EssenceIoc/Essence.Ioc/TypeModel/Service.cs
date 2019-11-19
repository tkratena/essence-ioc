using System;
using System.Linq.Expressions;
using System.Reflection;
using Essence.Ioc.Expressions;
using Essence.Ioc.LifeCycleManagement;
using Essence.Ioc.Registration.RegistrationExceptions;
using Essence.Ioc.Resolution;

namespace Essence.Ioc.TypeModel
{
    internal sealed class Service
    {
        private readonly Type _type;

        public Service(Type type)
        {
            _type = type;
        }
        
        public IFactoryExpression Resolve(IFactoryFinder factoryFinder, InstanceTracker tracker)
        {
            if (factoryFinder.TryGetFactory(_type, out var factoryExpression))
            {
                return factoryExpression;
            }
            
            if (_type.GetTypeInfo().IsGenericType)
            {
                return new GenericService(_type).Resolve(factoryFinder, tracker);
            }
            
            throw new NotRegisteredDependencyException(_type);
        }
    }
}