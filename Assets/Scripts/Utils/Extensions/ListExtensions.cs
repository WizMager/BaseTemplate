using System;
using System.Collections.Generic;

namespace Utils.Extensions
{
    public static class ListExtensions
    {
        public static int RemoveAllWithSwap<T>(this IList<T> list, Func<T, bool> condition)
        {
            var count = 0;
            var index = 0;
            while (index < list.Count)
            {
                var item = list[index];
                if (!condition(item))
                {
                    index++;
                    continue;
                }
 
                var lastIndex = list.Count - 1;
                list[index] = list[lastIndex];
                list.RemoveAt(lastIndex);
                count++;
            }
 
            return count;
        }
    }
}