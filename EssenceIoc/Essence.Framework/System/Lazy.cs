using System;
using System.Diagnostics.Contracts;
using System.Threading;

namespace Essence.Framework.System
{
    public static class Lazy
    {
        [Pure]
        public static ILazy<T> Cast<T>(this Lazy<T> lazy)
        {
            if (lazy == null)
            {
                return null;
            }

            return new LazyAdapter<T>(lazy);
        }
        
        [Pure]
        public static ILazy<T> Of<T>()
        {
            return new Lazy<T>().Cast();
        }
        
        [Pure]
        public static ILazy<T> Of<T>(bool isThreadSafe)
        {
            return new Lazy<T>(isThreadSafe).Cast();
        }
        
        [Pure]
        public static ILazy<T> Of<T>(LazyThreadSafetyMode mode)
        {
            return new Lazy<T>(mode).Cast();
        }
        
        [Pure]
        public static ILazy<T> From<T>(Func<T> valueFactory)
        {
            return new Lazy<T>(valueFactory).Cast();
        }
        
        [Pure]
        public static ILazy<T> From<T>(Func<T> valueFactory, bool isThreadSafe)
        {
            return new Lazy<T>(valueFactory, isThreadSafe).Cast();
        }
        
        [Pure]
        public static ILazy<T> From<T>(Func<T> valueFactory, LazyThreadSafetyMode mode)
        {
            return new Lazy<T>(valueFactory, mode).Cast();
        }

        private struct LazyAdapter<T> : ILazy<T>
        {
            private readonly Lazy<T> _lazy;

            public LazyAdapter(Lazy<T> lazy)
            {
                _lazy = lazy ?? throw new ArgumentNullException(nameof(lazy));
            }

            public T Value => _lazy.Value;
            public bool IsValueCreated => _lazy.IsValueCreated;
        }
    }
}