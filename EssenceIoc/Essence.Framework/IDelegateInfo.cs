using System;
using System.Reflection;

namespace Essence.Framework
{
    public interface IDelegateInfo
    {
        Type Type { get; }
        MethodInfo InvokeMethod { get; }
    }
}