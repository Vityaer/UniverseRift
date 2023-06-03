using System.Collections.Generic;

namespace Utils
{
    public static class IReadOnlyListExtensions
    {
        public static bool Contains<TSource>(this IReadOnlyList<TSource> source, TSource value)
        {
            foreach (var item in source)
            {
                if (item.Equals(value))
                {
                    return true;
                }
            }
            return false;
        }
    }
}