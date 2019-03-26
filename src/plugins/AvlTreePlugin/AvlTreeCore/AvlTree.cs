using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace AvlTreeCore
{
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class AvlTree<T> : IReadOnlyCollection<T>
    {
        private readonly IComparer<T> _keyComparer;

        public AvlTree() : this(Comparer<T>.Default)
        {
        }

        public AvlTree(IComparer<T> keyComparer)
        {
            _keyComparer = keyComparer;
        }

        public int Count { get; private set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal INode<T> Root { get; set; }

        public TraversalMode TraversalMode { get; set; }

        public void Insert(T key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            Root = Insert(Root, new Node<T>(key));
            Count++;
        }

        public void Delete(T key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            Root = Delete(Root, key);
            Count--;
        }

        protected INode<T> Insert(INode<T> current, INode<T> node)
        {
            if (current == null)
            {
                return node;
            }

            if (_keyComparer.Compare(node.Key, current.Key) < 0)
            {
                current.Left = Insert(current.Left, node);
            }
            else if (_keyComparer.Compare(node.Key, current.Key) > 0)
            {
                current.Right = Insert(current.Right, node);
            }
            else
            {
                HandleDuplicate(current, node);
            }

            return current.GetBalanced();
        }

        protected virtual void HandleDuplicate(INode<T> current, INode<T> node)
        {
            throw new ArgumentException("An item with the same key has already been added.");
        }

        private INode<T> Delete(INode<T> current, T key)
        {
            var node = RawDelete(current, key);

            return node.GetBalanced();
        }

        private INode<T> RawDelete(INode<T> current, T key)
        {
            if (current == null)
            {
                return null;
            }

            if (_keyComparer.Compare(key, current.Key) < 0)
            {
                current.Left = Delete(current.Left, key);
            }
            else if (_keyComparer.Compare(key, current.Key) > 0)
            {
                current.Right = Delete(current.Right, key);
            }
            else
            {
                if (current.Left == null || current.Right == null)
                {
                    // No child case
                    current = current.Left ?? current.Right;
                }
                else
                {
                    // Get the inorder successor (smallest in the right subtree)  
                    var temp = MinimalNode(current.Right);

                    var left = current.Left;

                    // Delete the inorder successor  
                    var right = Delete(current.Right, temp.Key);

                    current = temp;

                    // Update children
                    current.Left = left;
                    current.Right = right;
                }
            }

            return current;
        }
        
        public bool ContainsKey(T key)
        {
            return Root.FindChild(key) != null;
        }

        private static INode<T> MinimalNode(INode<T> node)
        {
            var current = node;

            while (current.Left != null)
            {
                current = current.Left;
            }

            return current;
        }
       
        public IEnumerator<T> GetEnumerator()
        {
            switch (TraversalMode)
            {
                case TraversalMode.InOrder:
                    return this.InOrder().Select(node => node.Key).GetEnumerator();

                case TraversalMode.PreOrder:
                    return this.PreOrder().Select(node => node.Key).GetEnumerator();

                case TraversalMode.PostOrder:
                    return this.PostOrder().Select(node => node.Key).GetEnumerator();

                case TraversalMode.LevelOrder:
                    return this.LevelOrder().Select(node => node.Key).GetEnumerator();

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal int BatchDelete(IEnumerable<T> keys)
        {
            var deletions = BatchRawDelete(keys);
            DayStoutWarrenAlgorithm.Balance(this);
            
            return deletions;
        }

        internal int BatchRawDelete(IEnumerable<T> keys)
        {
            var removedCount = 0;

            foreach (var key in keys.ToArray())
            {
                Root = RawDelete(Root, key);
                Count--;
                removedCount++;
            }

            return removedCount;
        }
    }
}