﻿using System;
using System.Linq.Expressions;
using System.Reflection;
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
        
        public IFactoryExpression Resolve(IFactoryFinder factoryFinder)
        {
            if (factoryFinder.TryGetFactory(_type, out var factory))
            {
                var expression = Expression.Convert(Expression.Invoke(Expression.Constant(factory)), _type);
                
                return FactoryExpression.Create(expression);
            }

            if (factoryFinder.TryGetFactoryExpression(_type, out var factoryExpression))
            {
                return factoryExpression;
            }
            
            if (_type.GetTypeInfo().IsGenericType)
            {
                return new GenericService(_type).Resolve(factoryFinder);
            }
            
            throw new NotRegisteredDependencyException(_type);
        }
    }
}