namespace AvlTreeCore
{
    internal static class NodeAvlOperations
    {
        public static INode<T> GetBalanced<T>(this INode<T> node)
        {
            var balance = GetBalance(node);

            // Left Left  
            if (balance > 1 && GetBalance(node.Left) >= 0)
            {
                return node.RotateRight();
            }

            // Left Right  
            if (balance > 1 && GetBalance(node.Left) < 0)
            {
                node.Left = node.Left.RotateLeft();
                return node.RotateRight();
            }

            // Right Right  
            if (balance < -1 && GetBalance(node.Right) <= 0)
            {
                return node.RotateLeft();
            }

            // Right Left  
            if (balance < -1 && GetBalance(node.Right) > 0)
            {
                node.Right = node.Right.RotateRight();
                return node.RotateLeft();
            }

            node.UpdateHeight();
            return node;
        }

        private static int GetBalance<T>(INode<T> node)
        {
            return node == null ? 0 : node.Left.GetHeightOrZero() - node.Right.GetHeightOrZero();
        }
    }
}