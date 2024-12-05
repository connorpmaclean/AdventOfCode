using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AocHelper
{
    public static class EnumerableExtensions
    {

        public static IEnumerable<(T, T)> GetDoubleIterator<T>(this IList<T> input)
        {
            for (int i = 0; i < input.Count - 1; i++) 
            {
                yield return (input[i], input[i + 1]);
            }
        }

        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, Func<TValue> func)
        {
            if (dict.TryGetValue(key, out TValue value))
            {
                return value;
            }

            var addValue = func();
            dict.Add(key, addValue);
            return addValue;
        }
    }
}
