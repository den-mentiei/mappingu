using System;
using System.Collections.Generic;

namespace AvlTreeCore
{
    internal static class TreePartialTraversals
    {
        /// <summary>
        /// Depth First traversal: Left -> Root -> Right.
        /// </summary>
        /// <returns>An ordered sequence of tree contents.</returns>
        public static IEnumerable<T> LevelOrderKeys<T>(this AvlTree<T> tree, Func<T, bool> keySelector)
        {
            var node = tree.Root;
            if (node == null)
            {
                yield break;
            }

            var queue = new Queue<INode<T>>();
            queue.Enqueue(node);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (current == null)
                {
                    continue;
                }

                queue.Enqueue(current.Left);
                queue.Enqueue(current.Right);

                if (keySelector(current.Key))
                {
                    yield return current.Key;
                }
            }
        }
    }
}