using System.Collections.Generic;
using Contract;

namespace Tests
{
    internal static class CollectionFactories
    {
        private static readonly List<IStorageFactory> Storage = new List<IStorageFactory>();

        public static void RegisterFactory(IStorageFactory factory)
        {
            Storage.Add(factory);
        }

        public static void Cleanup()
        {
            Storage.Clear();
        }

        public static IEnumerable<IStorageFactory> Factories => Storage;
    }
}