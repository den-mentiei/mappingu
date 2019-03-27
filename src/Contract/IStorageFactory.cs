using System.Collections.Generic;

namespace Contract
{
    public interface IStorageFactory
    {
        IMappedIntervalsCollection<T> Create<T>();

        IMappedIntervalsCollection<T> Create<T>(IReadOnlyList<MappedInterval<T>> intervals);
    }
}