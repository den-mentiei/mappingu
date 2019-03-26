using System;
using System.Collections.Generic;

namespace AvlTreeCore
{
    internal static class TreeBatchExtensions
    {
        public static void BatchInsert<T, TValue>(this AvlTree<T> tree, IReadOnlyList<KeyValuePair<T, TValue>> items)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<KeyValuePair<T, TValue>> OptimalAvlSequence<T, TValue>(IReadOnlyList<KeyValuePair<T, TValue>> items)
        {
            throw new NotImplementedException();
        }
    }
}
