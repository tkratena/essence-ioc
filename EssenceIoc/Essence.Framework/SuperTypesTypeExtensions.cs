using System;
using System.Collections.Generic;

namespace Essence.Framework
{
    public static class SuperTypesTypeExtensions
    {
        public static IEnumerable<Type> GetSuperTypes(this Type type)
        {
            foreach (var interfaceType in type.GetInterfaces())
            {
                yield return interfaceType;
            }

            var baseType = type.BaseType;
            while (baseType != null)
            {
                yield return baseType;
                baseType = baseType.BaseType;
            }
        }
    }
}