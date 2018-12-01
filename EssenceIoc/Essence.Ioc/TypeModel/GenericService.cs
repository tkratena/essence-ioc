using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Essence.Ioc.Expressions;
using Essence.Ioc.Registration.RegistrationExceptions;
using Essence.Ioc.Resolution;

namespace Essence.Ioc.TypeModel
{
    internal sealed class GenericService
    {
        private readonly Type _type;

        public GenericService(Type type)
        {
            _type = type;
        }

        public IFactoryExpression Resolve(IFactoryFinder factoryFinder)
        {
            if (!factoryFinder.TryGetGenericType(_type.GetGenericTypeDefinition(), out var implementationTypeDefinition))
            {
                if (IsGenericSequence(_type))
                {
                    throw new NotRegisteredSequenceDependencyException(_type);
                }
                
                throw new NotRegisteredDependencyException(_type);
            }

            var implementationType = implementationTypeDefinition.MakeGenericType(_type.GetTypeInfo().GetGenericArguments());
            return new Implementation(implementationType).Resolve(factoryFinder);
        }

        private bool IsGenericSequence(Type type)
        {
            var genericArguments = type.GetTypeInfo().GetGenericArguments();
            if (genericArguments.Length != 1)
            {
                return false;
            }

            var sequenceType = typeof(IEnumerable<>).MakeGenericType(genericArguments.Single());
            return sequenceType.GetTypeInfo().IsAssignableFrom(type);
        }
    }
}