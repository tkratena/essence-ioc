using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Essence.Framework.Linq
{
    public static class Collection
    {
        [Pure]
        public static IEnumerable<T> Lazy<T>(Func<IEnumerable<T>> collectionFactory)
        {
            return System.Lazy.From(collectionFactory).AsCollection();
        }

        [Pure]
        public static IReadOnlyCollection<T> Lazy<T>(Func<IReadOnlyCollection<T>> collectionFactory)
        {
            return System.Lazy.From(collectionFactory).AsCollection();
        }

        [Pure]
        public static IReadOnlyList<T> Lazy<T>(Func<IReadOnlyList<T>> collectionFactory)
        {
            return System.Lazy.From(collectionFactory).AsCollection();
        }
    }
}