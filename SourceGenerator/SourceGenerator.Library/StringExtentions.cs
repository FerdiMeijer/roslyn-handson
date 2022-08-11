using System.Collections.Generic;
using System.Linq;

namespace SourceGenerator.Library
{
    public static class StringExtentions
    {
        public static string ToCsv<T>(this IEnumerable<T> list, string separator = ", ")
        {
            return list?.Aggregate(string.Empty, (a, b) =>
            {
                if (string.IsNullOrEmpty(a))
                {
                    return b.ToString();
                }

                return string.Concat(a, separator, b);
            });
        }
    }
}