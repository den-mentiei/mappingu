using System.Collections.Generic;
using Contract;

namespace AvlTreePlugin
{
    internal class MappedIntervalComparer<T> : IComparer<MappedInterval<T>>
    {
        public int Compare(MappedInterval<T> x, MappedInterval<T> y)
        {
            if (x.IntervalStart < y.IntervalStart && x.IntervalEnd <= y.IntervalStart)
            {
                return -1;
            }

            if (y.IntervalStart < x.IntervalStart && y.IntervalEnd <= x.IntervalStart)
            {
                return 1;
            }
            
            return 0;
        }
    }
}