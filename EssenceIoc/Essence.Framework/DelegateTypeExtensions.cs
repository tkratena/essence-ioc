using System;
using System.Reflection;

namespace Essence.Framework
{
    public static class DelegateTypeExtensions
    {
        public static bool IsDelegate(this Type type)
        {
            return typeof(Delegate).GetTypeInfo().IsAssignableFrom(type);
        }

        public static IDelegateInfo AsDelegate(this Type delegateType)
        {
            return delegateType.IsDelegate() ? new DelegateInfo(delegateType) : null;
        }

        private class DelegateInfo : IDelegateInfo
        {
            public Type Type { get; }
            public MethodInfo InvokeMethod { get; }

            public DelegateInfo(Type type)
            {
                Type = type;
                InvokeMethod = type.GetTypeInfo().GetMethod("Invoke");
            }

            public override string ToString()
            {
                return Type.ToString();
            }
        }
    }
}