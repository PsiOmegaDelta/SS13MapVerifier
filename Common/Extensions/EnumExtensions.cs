using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<Enum> GetFlags(this Enum source)
        {
            return Enum.GetValues(source.GetType()).Cast<Enum>().Where(source.HasFlag);
        }
    }
}
