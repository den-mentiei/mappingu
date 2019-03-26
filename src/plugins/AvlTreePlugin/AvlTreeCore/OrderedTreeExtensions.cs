using System;
using System.Collections.Generic;
using System.Linq;

namespace AvlTreeCore
{
    public static class OrderedTreeExtensions
    {
        public static IEnumerable<INode<T>> FirstOrderedSubsequence<T>(this AvlTree<T> tree, Func<INode<T>, bool> predicate)
        {
            var sequenceContinues = false;

            foreach (var node in tree.InOrder())
            {
                var predicateResult = predicate(node);
                if (predicateResult)
                {
                    sequenceContinues = true;
                }

                if (sequenceContinues)
                {
                    if (!predicateResult)
                    {
                        yield break;
                    }
                }

                if (predicateResult)
                {
                    yield return node;
                }
            }
        }

        public static int RemoveFirstOrderedSubsequence<T>(this AvlTree<T> tree, Func<INode<T>, bool> predicate)
        {
            var sequenceContinues = false;
            var removedNodesCount = 0;

            // TODO Remove .ToArray()
            foreach (var node in tree.FirstOrderedSubsequence(predicate).ToArray())
            {
                var predicateResult = predicate(node);
                if (predicateResult)
                {
                    sequenceContinues = true;
                }

                if (sequenceContinues)
                {
                    if (!predicateResult)
                    {
                        break;
                    }
                }

                if (predicateResult)
                {
                    tree.Delete(node.Key);
                    removedNodesCount++;
                }
            }

            return removedNodesCount;
        }
    }
}