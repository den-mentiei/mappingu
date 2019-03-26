using System.Collections.Generic;

namespace AvlTreeCore
{
    public static class TreeTraversals
    {
        /// <summary>
        /// Depth First traversal: Left -> Root -> Right.
        /// </summary>
        /// <returns>An ordered sequence of tree contents.</returns>
        public static IEnumerable<INode<T>> InOrder<T>(this AvlTree<T> tree)
        {
            return InOrder(tree.Root);
        }

        /// <summary>
        /// Depth First traversal: Left -> Root -> Right.
        /// </summary>
        /// <returns>An ordered sequence of tree contents.</returns>
        private static IEnumerable<INode<T>> InOrder<T>(INode<T> node)
        {
            if (node == null)
            {
                yield break;
            }

            foreach (var left in InOrder(node.Left))
            {
                yield return left;
            }

            yield return node;

            foreach (var right in InOrder(node.Right))
            {
                yield return right;
            }
        }

        /// <summary>
        /// Depth First traversal: Root -> Left -> Right.
        /// </summary>
        /// <returns>An ordered sequence of tree contents.</returns>
        public static IEnumerable<INode<T>> PreOrder<T>(this AvlTree<T> tree)
        {
            return PreOrder(tree.Root);
        }

        private static IEnumerable<INode<T>> PreOrder<T>(INode<T> node)
        {
            if (node == null)
            {
                yield break;
            }

            yield return node;

            foreach (var left in PreOrder(node.Left))
            {
                yield return left;
            }

            foreach (var right in PreOrder(node.Right))
            {
                yield return right;
            }
        }

        /// <summary>
        /// Depth First traversal: Left -> Right -> Root.
        /// </summary>
        /// <returns>An ordered sequence of tree contents.</returns>
        public static IEnumerable<INode<T>> PostOrder<T>(this AvlTree<T> tree)
        {
            return PostOrder(tree.Root);
        }

        private static IEnumerable<INode<T>> PostOrder<T>(INode<T> node)
        {
            if (node == null)
            {
                yield break;
            }

            foreach (var left in PostOrder(node.Left))
            {
                yield return left;
            }

            foreach (var right in PostOrder(node.Right))
            {
                yield return right;
            }

            yield return node;
        }

        /// <summary>
        /// Breadth First traversal (or Level Order traversal).
        /// </summary>
        /// <returns>An ordered sequence of tree contents.</returns>
        public static IEnumerable<INode<T>> LevelOrder<T>(this AvlTree<T> tree)
        {
            return LevelOrder(tree.Root);
        }

        private static IEnumerable<INode<T>> LevelOrder<T>(INode<T> node)
        {
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

                yield return current;
            }
        }
    }
}