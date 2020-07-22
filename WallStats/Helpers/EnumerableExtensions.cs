using System;
using System.Collections.Generic;
using System.Linq;

namespace WallStats.Helpers
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> enumerable) =>
            enumerable ?? Enumerable.Empty<T>();

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) =>
            enumerable == null || !enumerable.Any();

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            return enumerable.Select(x =>
            {
                action?.Invoke(x);
                return x;
            });
        }
    }
}