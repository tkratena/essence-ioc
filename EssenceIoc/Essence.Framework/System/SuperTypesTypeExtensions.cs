using System;
using System.Collections.Generic;
using System.Reflection;

namespace Essence.Framework.System
{
    public static class SuperTypesTypeExtensions
    {
        public static IEnumerable<Type> GetSuperTypes(this Type type)
        {
            foreach (var interfaceType in type.GetTypeInfo().GetInterfaces())
            {
                yield return interfaceType;
            }

            var baseType = type.GetTypeInfo().BaseType;
            while (baseType != null)
            {
                yield return baseType;
                baseType = baseType.GetTypeInfo().BaseType;
            }
        }
    }
}