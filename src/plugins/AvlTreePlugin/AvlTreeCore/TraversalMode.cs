namespace AvlTreeCore
{
    public enum TraversalMode
    {
        /// <summary>
        /// Depth First traversal: Left -> Root -> Right.
        /// </summary>
        InOrder = 0,

        /// <summary>
        /// Depth First traversal: Root -> Left -> Right.
        /// </summary>
        PreOrder = 1,

        /// <summary>
        /// Depth First traversal: Left -> Right -> Root.
        /// </summary>
        PostOrder = 2,

        /// <summary>
        /// Breadth First traversal (or Level Order traversal).
        /// </summary>
        LevelOrder = 3
    }
}