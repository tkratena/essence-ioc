using System;
using Essence.Ioc.Expressions;

namespace Essence.Ioc.Resolution
{
    internal interface IFactoryFinder
    {
        bool TryGetFactory(Type constructedType, out Delegate factory);
        bool TryGetFactoryExpression(Type constructedType, out IFactoryExpression factoryExpression);
        bool TryGetGenericType(Type genericServiceTypeDefinition, out Type genericImplementationTypeDefinition);
    }
}