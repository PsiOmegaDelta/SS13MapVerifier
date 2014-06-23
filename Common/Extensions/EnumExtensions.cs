using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Extensions
{
    public static class EnumExtensions
    {
        private static readonly IDictionary<Enum, IEnumerable<Enum>> CachedResults = new Dictionary<Enum, IEnumerable<Enum>>();

        public static IEnumerable<Enum> GetFlags(this Enum source)
        {
            return CachedResults.SafeGetValue(
                source,
                () => Enum.GetValues(source.GetType()).OfType<Enum>().Where(source.HasFlag).ToArray());
        }
    }
}
