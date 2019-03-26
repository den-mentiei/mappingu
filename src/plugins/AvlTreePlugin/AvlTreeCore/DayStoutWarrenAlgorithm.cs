using System;

namespace AvlTreeCore
{
    /// <summary>
    /// A simple algorithm is given which takes an arbitrary binary search tree and rebalances it to form
    /// another of optimal shape, using time linear in the number of nodes and only a constant amount of space
    /// (beyond that used to store the initial tree). This algorithm is therefore optimal in its use of both time and space.
    /// Previous algorithms were optimal in at most one of these two measures, or were not applicable to all binary search
    /// trees. When the nodes of the tree are stored in an array, a simple addition to this algorithm results in the nodes
    /// being stored in sorted order in the initial portion of the array, again using linear time and constant space.
    /// </summary>
    /// <remarks>Origin: http://web.eecs.umich.edu/~qstout/pap/CACM86.pdf </remarks>
    /// <remarks>Wiki: https://en.wikipedia.org/wiki/Day%E2%80%93Stout%E2%80%93Warren_algorithm </remarks>
    internal static class DayStoutWarrenAlgorithm
    {
        public static void Balance<T>(AvlTree<T> tree)
        {
            // 1. Allocate a node, the "pseudo-root", and make the tree's actual root the right child of the pseudo-root.
            // 2. Call tree-to-vine with the pseudo-root as its argument.
            // 3. Call vine-to-tree on the pseudo-root and the size (number of elements) of the tree.
            // 4. Make the tree's actual root equal to the pseudo-root's right child.
            // 5. Dispose of the pseudo-root.

            var pseudoRoot = new Node<T>(default(T)) {Right = tree.Root};
            TreeToVine(pseudoRoot);
            VineToTree(pseudoRoot, tree.Count);
            tree.Root = pseudoRoot.Right;
            // pseudoRoot = null;
        }

        private static void TreeToVine<T>(INode<T> root)
        {
            // Convert tree to a "vine", i.e., a sorted linked list,
            // using the right pointers to point to the next node in the list

            var tail = root;
            var rest = tail.Right;

            while (rest != null)
            {
                if (rest.Left == null)
                {
                    tail = rest;
                    rest = rest.Right;
                }
                else
                {
                    var temp = rest.Left;
                    rest.Left = temp.Right;
                    temp.Right = rest;
                    rest = temp;
                    tail.Right = temp;
                }
            }
        }

        private static void VineToTree<T>(INode<T> root, int size)
        {
            var leaves = size + 1 - (1 << Log2(size + 1));

            Compress(root, leaves);
            size -= leaves;

            while (size > 1)
            {
                Compress(root, size / 2);
                size /= 2;
            }
        }

        private static void Compress<T>(INode<T> root, int count)
        {
            var scanner = root;

            for (var i = 1; i < count; i++)
            {
                var child = scanner.Right;
                scanner.Right = child.Right;
                scanner = scanner.Right;
                child.Right = scanner.Left;
                scanner.Left = child;
            }
        }

        private static int Log2(int value)
        {
            switch (value)
            {
                case 0:
                    throw new ArgumentException("Attempted to calculate logarithm of zero.");
                case 1:
                    return 0;
                default:
                    var result = 0;
                    while (value > 1)
                    {
                        value >>= 1;
                        result++;
                    }

                    return result;
            }
        }
    }
}