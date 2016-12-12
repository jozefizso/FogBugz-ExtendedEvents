using System;
using System.Collections.Generic;
using System.Text;

namespace FBExtendedEvents
{
    public static class EnumerableEx
    {
        public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
        {
            if (comparer == null)
            {
                comparer = EqualityComparer<TSource>.Default;
            }

            if (source == null)
            {
                throw new ArgumentException("Argument cannot be null.", nameof(source));
            }

            foreach (TSource element in source)
            {
                if (comparer.Equals(element, value))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
