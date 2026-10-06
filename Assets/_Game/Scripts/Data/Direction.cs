namespace ColorBlocks.Data
{
    public enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }

    public static class DirectionExtensions
    {
        public static (int row, int col) ToOffset(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Up: return (-1, 0);
                case Direction.Right: return (0, 1);
                case Direction.Down: return (1, 0);
                default: return (0, -1);
            }
        }
    }
}
