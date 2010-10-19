using System;
using System.Collections.Generic;
using System.Linq;

namespace MVPtoMVVM.utility
{
    public static class Iterating
    {
        public static void each<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items ?? Enumerable.Empty<T>()) action(item);
        }
    }
}