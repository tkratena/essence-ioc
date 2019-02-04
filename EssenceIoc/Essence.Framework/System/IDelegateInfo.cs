using System;
using System.Reflection;

namespace Essence.Framework.System
{
    public interface IDelegateInfo
    {
        Type Type { get; }
        MethodInfo InvokeMethod { get; }
    }
}