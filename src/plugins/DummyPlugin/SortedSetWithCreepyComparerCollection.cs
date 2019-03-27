using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Contract;

namespace DummyPlugin
{
    internal sealed class SortedSetWithCreepyComparerCollection<T> : IMappedIntervalsCollection<T>
    {
        private readonly SortedSet<Hueta> _sortedSet;

        public SortedSetWithCreepyComparerCollection(IReadOnlyCollection<MappedInterval<T>> collection)
        {
            ThrowIfNotOrdered(collection);

            _sortedSet = new SortedSet<Hueta>(collection.Select(interval => new Hueta(interval, null)), new HuetaComparer());
        }

        public IEnumerator<MappedInterval<T>> GetEnumerator()
        {
            return _sortedSet.Select(hueta => hueta.Interval).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _sortedSet.Count;

        public void Put(IReadOnlyList<MappedInterval<T>> newIntervals)
        {
            ThrowIfNotOrdered(newIntervals);

            foreach (var mappedInterval in newIntervals)
            {
                Delete(mappedInterval.IntervalStart, mappedInterval.IntervalEnd);
                _sortedSet.Add(new Hueta(mappedInterval, null));
            }
        }

        public void Delete(long from, long to)
        {
            var ololol = _sortedSet.GetViewBetween(new Hueta(new MappedInterval<T>(from, from, default(T)), true), new Hueta(new MappedInterval<T>(to, to, default(T)), false));
            if (ololol.Count > 0)
            {
                var olololMin = ololol.Min;
                var olololMax = ololol.Max;
                var toAdd = new List<Hueta>(2);

                if (olololMin.Start < from)
                {
                    _sortedSet.Remove(olololMin);
                    toAdd.Add(new Hueta(new MappedInterval<T>(olololMin.Start, from, olololMin.Interval.Payload), null));
                }
                if (olololMax.End > to)
                {
                    _sortedSet.Remove(olololMax);
                    toAdd.Add(new Hueta(new MappedInterval<T>(to, olololMax.End, olololMax.Interval.Payload), null));
                }

                var toRemove = ololol.ToArray();
                _sortedSet.ExceptWith(toRemove);
                foreach (var hueta in toAdd)
                {
                    _sortedSet.Add(hueta);
                }
            }
        }

        public IEnumerator<MappedInterval<T>> GetEnumerator(long from)
        {
            return _sortedSet.GetViewBetween(new Hueta(new MappedInterval<T>(from, from, default(T)), true), new Hueta(_sortedSet.Max.Interval, false)).Select(hueta => hueta.Interval).GetEnumerator();
        }

        private static void ThrowIfNotOrdered(IReadOnlyCollection<MappedInterval<T>> intervals)
        {
            if (intervals.Count > 1)
            {
                var previous = intervals.First();
                foreach (var current in intervals.Skip(1))
                {
                    if (current.IntervalStart < previous.IntervalEnd)
                    {
                        throw new ArgumentException();
                    }

                    previous = current;
                }
            }
        }

        private struct Hueta
        {
            public Hueta(MappedInterval<T> interval, bool? isStart)
            {
                Interval = interval;
                if (isStart.HasValue)
                {
                    IsStart = isStart.Value;
                }
                else
                {
                    IsStart = null;
                }
            }

            public MappedInterval<T> Interval { get; }

            public long Start => Interval.IntervalStart;

            public long End => Interval.IntervalEnd;

            public bool? IsStart { get; }
        }

        class HuetaComparer : IComparer<Hueta>
        {
            public int Compare(Hueta interval1, Hueta interval2)
            {
                if (IsCompletelyWithin(interval1, interval2))
                {
                    return 0;
                }

                if (Overlaps(interval1, interval2))
                {
                    if (interval1.IsStart.HasValue && interval1.IsStart.Value)
                    {
                        return -1;
                    }

                    if (interval2.IsStart.HasValue && interval2.IsStart.Value)
                    {
                        return 1;
                    }
                    
                    if (interval1.IsStart.HasValue && !interval1.IsStart.Value)
                    {
                        return 1;
                    }

                    if (interval2.IsStart.HasValue && !interval2.IsStart.Value)
                    {
                        return -1;
                    }
                }

                return interval1.Start.CompareTo(interval2.Start);
            }

            private static bool IsCompletelyWithin(Hueta interval1, Hueta interval2)
            {
                return interval1.Start >= interval2.Start && interval1.End <= interval2.End;
            }

            private static bool Overlaps(Hueta interval1, Hueta interval2)
            {
                return interval1.Start <= interval2.End && interval1.End >= interval2.Start;
            }
        }
    }
}
