using System;
using Essence.Ioc.Expressions;

namespace Essence.Ioc.Resolution
{
    internal interface IFactoryFinder
    {
        bool TryGetFactory(Type constructedType, out IFactoryExpression factoryExpression);
        bool TryGetGenericType(Type genericServiceTypeDefinition, out Type genericImplementationTypeDefinition);
    }
}