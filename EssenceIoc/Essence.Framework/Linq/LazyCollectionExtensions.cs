using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Essence.Framework.System;

namespace Essence.Framework.Linq
{
    public static class LazyCollectionExtensions
    {
        [Pure]
        public static IEnumerable<T> AsCollection<T>(this ILazy<IEnumerable<T>> lazy)
        {
            return new LazyEnumerable<IEnumerable<T>, T>(lazy ?? throw new ArgumentNullException(nameof(lazy)));
        }

        [Pure]
        public static IReadOnlyCollection<T> AsCollection<T>(this ILazy<IReadOnlyCollection<T>> lazy)
        {
            return new LazyReadOnlyCollection<IReadOnlyCollection<T>, T>(lazy ?? throw new ArgumentNullException(nameof(lazy)));
        }

        [Pure]
        public static IReadOnlyList<T> AsCollection<T>(this ILazy<IReadOnlyList<T>> lazy)
        {
            return new LazyReadOnlyList<IReadOnlyList<T>, T>(lazy ?? throw new ArgumentNullException(nameof(lazy)));
        }

        private class LazyEnumerable<TCollection, T> : IEnumerable<T>
            where TCollection : IEnumerable<T>
        {
            private readonly ILazy<TCollection> _lazy;

            protected TCollection Collection => _lazy.Value;

            public LazyEnumerable(ILazy<TCollection> lazy)
            {
                _lazy = lazy ?? throw new ArgumentNullException(nameof(lazy));
            }

            public IEnumerator<T> GetEnumerator() => Collection.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) Collection).GetEnumerator();
        }

        private class LazyReadOnlyCollection<TCollection, T> : LazyEnumerable<TCollection, T>, IReadOnlyCollection<T>
            where TCollection : IReadOnlyCollection<T>
        {
            public LazyReadOnlyCollection(ILazy<TCollection> lazy) : base(lazy)
            {
            }

            public int Count => Collection.Count;
        }

        private class LazyReadOnlyList<TCollection, T> : LazyReadOnlyCollection<TCollection, T>, IReadOnlyList<T>
            where TCollection : IReadOnlyList<T>
        {
            public LazyReadOnlyList(ILazy<TCollection> lazy) : base(lazy)
            {
            }

            public T this[int index] => Collection[index];
        }
    }
}