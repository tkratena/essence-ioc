using System.Collections.Generic;

namespace Essence.Framework.Linq
{
    public static class ObjectExtensions
    {
        public static IReadOnlyCollection<T> UnfoldToEnumerable<T>(this T o)
        {
            return new[] {o};
        }
    }
}