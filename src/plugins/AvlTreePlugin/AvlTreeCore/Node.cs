using System.Diagnostics;

namespace AvlTreeCore
{
    [DebuggerDisplay("{" + nameof(Key) + "}")]
    internal sealed class Node<T> : INode<T>
    {
        public Node(T key)
        {
            Key = key;
            Height = 1;
        }

        public T Key { get; set; }

        public INode<T> Left { get; set; }

        public INode<T> Right { get; set; }
        
        public int Height { get; set; }
    }
}