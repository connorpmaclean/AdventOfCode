using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
    }
}
