using System.Collections.Generic;
using System.Linq;
using ColorBlocks.Data;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace ColorBlocks.Core
{
    public class Block
    {
        public Vector2Int Position { get; private set; } // x = column, y = row
        public int Length { get; }
        public bool IsVertical { get; }
        private IReadOnlyList<Direction> _directions;
        

        public Block(MovableData data)
        {
            _directions = data.Direction;
            Position = new Vector2Int(data.Col, data.Row);
            Length = data.Length;
            IsVertical = CanMove(Direction.Up) || CanMove(Direction.Down);
        }

        public Vector2Int GetCell(int index)
        {
            if (IsVertical)
                return Position + new Vector2Int(0, index);

            return Position + new Vector2Int(index, 0);
        }

        public void Move(Direction direction, int steps)
        {
            Position += direction.ToOffset() * steps;
        }

        public bool CanMove(Direction direction)
        {
            return _directions.Contains(direction);
        }
    }
}
