using System.Collections.Generic;
using System.Linq;
using AvlTreeCore;
using Contract;

namespace AvlTreePlugin
{
    internal static class TimeIntervalTreeExtensions
    {
        public static IEnumerable<MappedInterval<T>> TimeSubset<T>(this AvlTree<MappedInterval<T>> tree, long from, long to)
        {
            var subsequence = tree.FirstOrderedSubsequence(node => AcceptNode(node.Key, from, to));
            return subsequence.Select(node => node.Key);
        }

        public static int RemoveTimeSubset<T>(this AvlTree<MappedInterval<T>> tree, long from, long to)
        {
            var removedNodesCount = 0;
            INode<MappedInterval<T>> first = null;
            INode<MappedInterval<T>> last = null;

            // TODO Remove .ToArray()
            foreach (var node in tree.FirstOrderedSubsequence(node => AcceptNode(node.Key, from, to)).ToArray())
            {
                if (first == null)
                {
                    first = node;
                }
                
                last = node;

                tree.Delete(node.Key);
                removedNodesCount++;
            }

            if (first != null && first.Key.IntervalStart < from)
            {
                tree.Insert(new MappedInterval<T>(first.Key.IntervalStart, from, first.Key.Payload));
            }

            if (last != null && to < last.Key.IntervalEnd)
            {
                tree.Insert(new MappedInterval<T>(to, last.Key.IntervalEnd, last.Key.Payload));
            }

            return removedNodesCount;
        }
        
        private static bool AcceptNode<T>(MappedInterval<T> node, long from, long to)
        {
            return node.IntervalEnd >= from && node.IntervalStart <= to;
        }
    }
}