using System;
using System.Collections.Generic;

namespace Essence.Framework.Linq
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> source, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
            {
                yield break;
            }
            
            var items = new List<T>();
            foreach (var item in source)
            {
                items.Add(item);
                yield return item;
            }

            if (items.Count == 0)
            {
                yield break;
            }

            for (var i = 1; i < count; i++)
            {
                foreach (var item in items)
                {
                    yield return item;
                }
            }
        }
        
        public static IEnumerable<T> RepeatInfinitely<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            
            var items = new List<T>();
            foreach (var item in source)
            {
                items.Add(item);
                yield return item;
            }

            if (items.Count == 0)
            {
                yield break;
            }

            while(true)
            {
                foreach (var item in items)
                {
                    yield return item;
                }
            }
        }
    }
}