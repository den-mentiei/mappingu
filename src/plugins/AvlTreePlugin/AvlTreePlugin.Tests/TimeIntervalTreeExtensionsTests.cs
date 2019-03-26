using AvlTreeCore;
using Contract;
using NUnit.Framework;

namespace AvlTreePlugin.Tests
{
    [TestFixture]
    internal sealed class TimeIntervalTreeExtensionsTests
    {
        [Test]
        public void RemoveFirstOrderedSubsequence()
        {
            var tree = CreateTree<int>(0, 50, 10, 10);
            Assert.That(tree.Count, Is.EqualTo(5));

            tree.RemoveFirstOrderedSubsequence(node => AcceptNode(node.Key, 5, 25));
            var actual = tree;

            CollectionAssert.AreEqual(new[]
            {
                new MappedInterval<int>(30, 40, 0), 
                new MappedInterval<int>(40, 50, 0)
            }, actual);
        }

        [Test]
        public void RemoveTimeSubset()
        {
            var tree = CreateTree<int>(0, 500, 5, 10);
            Assert.That(tree.Count, Is.EqualTo(50));

            var deletions = tree.RemoveTimeSubset(6, 1000);
            var actual = tree;

            CollectionAssert.AreEqual(new[]
            {
                new MappedInterval<int>(0, 5, 0)
            }, actual);
        }

        private static bool AcceptNode<T>(MappedInterval<T> node, int from, int to)
        {
            return node.IntervalEnd >= from && node.IntervalStart <= to;
        }
       
        private static AvlTree<MappedInterval<T>> CreateTree<T>(int from, int to, int length, int step)
        {
            var tree = new AvlTree<MappedInterval<T>>(new MappedIntervalComparer<T>()) {TraversalMode = TraversalMode.InOrder};

            for (var i = from; i < to; i += step)
            {
                tree.Insert(new MappedInterval<T>(i, i + length, default(T)));
            }

            return tree;
        }
    }
}