using System.Collections.Generic;
using System.Linq;
using AvlTreeCore;
using Contract;

namespace AvlTreePlugin
{
    internal sealed class MappedIntervalTree<T> : AvlTree<MappedInterval<T>>, IMappedIntervalsCollection<T>
    {
        public MappedIntervalTree(IComparer<MappedInterval<T>> comparer) 
            : base(comparer)
        {
        }

        IEnumerator<MappedInterval<T>> IEnumerable<MappedInterval<T>>.GetEnumerator()
        {
            return this.InOrder().Select(node => node.Key).GetEnumerator();
        }

        void IMappedIntervalsCollection<T>.Put(IReadOnlyList<MappedInterval<T>> newIntervals)
        {
            foreach (var interval in newIntervals)
            {
                Insert(interval);
            }
        }

        void IMappedIntervalsCollection<T>.Delete(long from, long to)
        {
            this.RemoveTimeSubset(from, to);
        }

        IEnumerator<MappedInterval<T>> IMappedIntervalsCollection<T>.GetEnumerator(long from)
        {
            return this.InOrder().SkipWhile(node => node.Key.IntervalEnd < from).Select(node => node.Key).GetEnumerator();
        }

        protected override void HandleDuplicate(INode<MappedInterval<T>> current, INode<MappedInterval<T>> node)
        {
            current.Key = new MappedInterval<T>(current.Key.IntervalStart, node.Key.IntervalStart, current.Key.Payload);
            Insert(current, node);
        }
    }
}