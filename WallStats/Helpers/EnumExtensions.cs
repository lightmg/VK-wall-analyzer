using System;
using System.Collections.Generic;
using System.Linq;

namespace WallStats.Helpers
{
    public static class EnumHelpers
    {
        public static IEnumerable<T> GetValues<T>() where T : struct, Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}