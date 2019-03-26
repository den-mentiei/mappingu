using NUnit.Framework;

namespace AvlTreeCore.Tests
{
    [TestFixture]
    internal sealed class TreeTraversalsTests
    {
        private static AvlTree<int> CreateTree()
        {
            // The constructed AVL-Tree would be  
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

            return tree;
        }

        [Test]
        public void InOrderTraversal()
        {
            var tree = CreateTree();
            tree.TraversalMode = TraversalMode.InOrder;
            var actual = tree;
            CollectionAssert.AreEqual(new[] {10, 20, 25, 30, 40, 50}, actual);
        }

        [Test]
        public void PreOrderTraversal()
        {
            var tree = CreateTree();
            tree.TraversalMode = TraversalMode.PreOrder;
            var actual = tree;
            CollectionAssert.AreEqual(new[] {30, 20, 10, 25, 40, 50}, actual);
        }

        [Test]
        public void PostOrderTraversal()
        {
            var tree = CreateTree();
            tree.TraversalMode = TraversalMode.PostOrder;
            var actual = tree;
            CollectionAssert.AreEqual(new[] {10, 25, 20, 50, 40, 30}, actual);
        }

        [Test]
        public void LevelOrderTraversal()
        {
            var tree = CreateTree();
            tree.TraversalMode = TraversalMode.LevelOrder;
            var actual = tree;
            CollectionAssert.AreEqual(new[] {30, 20, 40, 10, 25, 50}, actual);
        }
    }
}