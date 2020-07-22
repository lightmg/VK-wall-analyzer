using System.Collections.Generic;
using System.Text;

namespace WallStats.Helpers
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);

        public static string JoinStrings(this IEnumerable<string> enumerable, string separator) =>
            string.Join(separator, enumerable);

        public static string EmptyIfNull(this string str) => str ?? string.Empty;

        public static string ChangeEncoding(this string str, Encoding newEncoding, Encoding sourceEncoding = null)
        {
            sourceEncoding ??= Encoding.UTF8;
            var sourceBytes = sourceEncoding.GetBytes(str);
            var convertedBytes = Encoding.Convert(sourceEncoding, newEncoding, sourceBytes);
            return newEncoding.GetString(convertedBytes);
        }
    }
}