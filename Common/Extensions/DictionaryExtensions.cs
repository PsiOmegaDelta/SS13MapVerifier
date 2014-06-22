using System;
using System.Collections.Generic;

namespace Common.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue SafeGetValue<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TValue> defaultValue)
        {
            TValue result;
            if (source.TryGetValue(key, out result))
            {
                return result;
            }

            result = defaultValue();
            source.Add(key, result);

            return result;
        }
    }
}
