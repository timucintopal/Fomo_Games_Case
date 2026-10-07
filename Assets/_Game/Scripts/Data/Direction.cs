using UnityEngine;

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
        // x = column, y = row. Row 0 is the top row, so moving up decreases y.
        public static Vector2Int ToOffset(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Up: return new Vector2Int(0, -1);
                case Direction.Right: return new Vector2Int(1, 0);
                case Direction.Down: return new Vector2Int(0, 1);
                default: return new Vector2Int(-1, 0);
            }
        }
    }
}
