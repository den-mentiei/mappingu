using System;
using System.Collections.Generic;

namespace AvlTreeCore
{
    internal static class NodeOperations
    {
        public static T Find<T>(this INode<T> current, T key)
        {
            var node = current.FindChild(key);
            return node.Key;
        }

        public static T Find<T>(this INode<T> current, T key, Comparer<T> comparer)
        {
            var node = current.FindChild(key, comparer);
            return node.Key;
        }

        public static INode<T> FindChild<T>(this INode<T> current, T key)
        {
            return FindChild(current, key, Comparer<T>.Default);
        }

        public static INode<T> FindChild<T>(this INode<T> current, T key, IComparer<T> comparer)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            while (true)
            {
                if (current == null)
                {
                    return null;
                }

                if (comparer.Compare(current.Key, key) < 0)
                {
                    current = current.Right;
                    continue;
                }

                if (comparer.Compare(current.Key, key) > 0)
                {
                    current = current.Left;
                    continue;
                }

                if (comparer.Compare(current.Key, key) == 0)
                {
                    return current;
                }
            }
        }

        internal static INode<T> RotateLeft<T>(this INode<T> node)
        {
            var rightNode = node.Right;
            var temp = rightNode.Left;

            rightNode.Left = node;
            node.Right = temp;

            node.UpdateHeight();
            rightNode.UpdateHeight();

            return rightNode;
        }

        internal static INode<T> RotateRight<T>(this INode<T> node)
        {
            var leftNode = node.Left;
            var temp = leftNode.Right;

            leftNode.Right = node;
            node.Left = temp;

            node.UpdateHeight();
            leftNode.UpdateHeight();

            return leftNode;
        }

        internal static void UpdateHeight<T>(this INode<T> node)
        {
            if (node == null)
            {
                return;
            }

            node.Height = Math.Max(node.Left.GetHeightOrZero(), node.Right.GetHeightOrZero()) + 1;
        }

        internal static int GetHeightOrZero<T>(this INode<T> node)
        {
            return node?.Height ?? 0;
        }
    }
}