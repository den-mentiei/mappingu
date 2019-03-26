using System;
using System.Linq;
using NUnit.Framework;

namespace AvlTreeCore.Tests
{
    [TestFixture]
    internal sealed class AvlTreeTests
    {
        [Test]
        public void InsertThrows()
        {
            var tree = new AvlTree<int>();
            tree.Insert(10);
            Assert.Throws<ArgumentException>(() => tree.Insert(10));
        }

        [Test]
        public void Insert()
        {
            var tree = new AvlTree<int>();
            tree.Insert(10);
            tree.Insert(20);

            Assert.IsTrue(tree.ContainsKey(10));
            Assert.IsTrue(tree.ContainsKey(20));
            Assert.IsFalse(tree.ContainsKey(30));
        }

        [Test]
        public void TreeIsBalanced()
        {
            var tree = new AvlTree<int>();
            tree.Insert(5);
            tree.Insert(10);
            tree.Insert(20);
            tree.Insert(30);
            tree.Insert(60);
            tree.Insert(70);
            tree.Insert(500);
            tree.Insert(210);
            tree.Insert(300);
            tree.Insert(2);
            tree.Insert(1);

            AssertTreeIsBalanced(tree);
        }

        [Test]
        public void InsertAndDelete()
        {
            var tree = new AvlTree<int>();

            tree.Insert(10);
            Assert.IsTrue(tree.ContainsKey(10));

            tree.Insert(20);
            Assert.IsTrue(tree.ContainsKey(20));

            tree.Insert(30);
            Assert.IsTrue(tree.ContainsKey(30));

            tree.Delete(30);
            Assert.IsFalse(tree.ContainsKey(30));
        }

        [Test]
        public void InsertWithRebalance()
        {
            var tree = new AvlTree<int>();
            tree.Insert(10);
            tree.Insert(20);
            tree.Insert(30);
            tree.Insert(5);
            tree.Insert(60);

            tree.TraversalMode = TraversalMode.InOrder;
            var actual = tree;

            CollectionAssert.AreEqual(new[] {5, 10, 20, 30, 60}, actual);
        }

        [Test]
        public void Delete()
        {
            // The constructed AVL Tree would be  
            //     30  
            //    /  \  
            //   20   40  
            //  /  \   \  
            // 10  25   50

            var tree = new AvlTree<int>();
            tree.Insert(10);
            tree.Insert(20);
            tree.Insert(30);
            tree.Insert(40);
            tree.Insert(50);
            tree.Insert(25);

            tree.Delete(10);
            tree.Delete(50);

            tree.TraversalMode = TraversalMode.InOrder;
            var actual = tree;

            CollectionAssert.AreEqual(new[] { 20, 25, 30, 40 }, actual);
        }

        [Test]
        public void RemoveRange()
        {
            // The constructed AVL Tree would be  
            //     30  
            //    /  \  
            //   20   40  
            //  /  \   \  
            // 10  25   50

            var tree = new AvlTree<int>();
            tree.Insert(10);
            tree.Insert(20);
            tree.Insert(25);
            tree.Insert(30);
            tree.Insert(40);
            tree.Insert(50);

            var removedCount = tree.RemoveFirstOrderedSubsequence(node => node.Key >= 20 && node.Key < 35);
            Assert.That(removedCount, Is.EqualTo(3));

            tree.TraversalMode = TraversalMode.InOrder;
            var actual = tree;

            CollectionAssert.AreEqual(new[] {10, 40, 50}, actual);
        }

        [Test]
        public void BatchDelete()
        {
            // The constructed AVL Tree would be  
            //     30  
            //    /  \  
            //   20   40  
            //  /  \   \  
            // 10  25   50

            var tree = new AvlTree<int>();
            tree.Insert(10);
            tree.Insert(20);
            tree.Insert(25);
            tree.Insert(30);
            tree.Insert(40);
            tree.Insert(50);
            tree.Insert(60);
            tree.Insert(70);
            tree.Insert(80);
            tree.Insert(90);
            tree.Insert(100);

            var toRemove = tree.FirstOrderedSubsequence(node => node.Key >= 20 && node.Key < 35);
            var removedCount = tree.BatchDelete(toRemove.Select(node => node.Key));
            Assert.That(removedCount, Is.EqualTo(3));

            tree.TraversalMode = TraversalMode.InOrder;
            var actual = tree;

            CollectionAssert.AreEqual(new[] {10, 40, 50, 60, 70, 80, 90, 100}, actual);

            AssertTreeIsBalanced(tree);
        }

        [Test]
        public void DeleteWithRebalance()
        {
            var tree = new AvlTree<int>();
            tree.Insert(10);
            tree.Insert(20);
            tree.Insert(30);
            tree.Insert(40);
            tree.Insert(50);
            tree.Insert(25);

            tree.Delete(10);
            tree.Delete(30);

            tree.TraversalMode = TraversalMode.InOrder;
            var actual = tree;

            CollectionAssert.AreEqual(new[] { 20, 25, 40, 50 }, actual);
        }

        [TestCase(1, 0)]
        [TestCase(2, 1)]
        [TestCase(3, 1)]
        [TestCase(10, 3)]
        [TestCase(100, 6)]
        [TestCase(666, 9)]
        [TestCase(5100, 12)]
        public void Log2Tests(int value, int expected)
        {
            Assert.That(Log2(value), Is.EqualTo(expected));
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

        private static void AssertTreeIsBalanced<T>(AvlTree<T> tree)
        {
            //var uniqueLeafHeights = tree.Where(node => node.Left == null && node.Right == null).Select(leaf => leaf.Height).Distinct();
            foreach (var node in tree.LevelOrder())
            {
                if (node.Left != null && node.Right != null)
                {
                    Assert.IsTrue(Math.Abs(node.Left.Height - node.Right.Height) < 2, 
                        $"The difference between heights of left and right subtrees cannot be more than one for all nodes. " +
                        $"Current node ({node}), left: {node.Left.Height}, right: {node.Right.Height}");
                }
            }
        }
    }
}
