using System.Collections.Generic;
using System.Linq;
using ColorBlocks.Data;
using UnityEngine;

namespace ColorBlocks.Core
{
    public class Block
    {
        public Vector2Int Position { get; private set; } // x = column, y = row

        public int Length { get; }
        public bool IsVertical { get; }
        public int Color { get; }
        
        private IReadOnlyList<Direction> _directions;
        
        public Block(MovableData data)
        {
            Color = data.Colors;
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
        
        public Vector2Int GetFrontCell(Direction direction)
        {
            bool movesTowardsLastCell = direction == Direction.Down || direction == Direction.Right;

            if (movesTowardsLastCell)
                return GetCell(Length - 1);

            return GetCell(0);
        }

        public void Move(Direction direction, int steps)
        {
            Position += direction.ToOffset() * steps;
        }

        public bool CanMove(Direction direction)
        {
            return _directions.Contains(direction);
        }
        
        public bool Occupies(Vector2Int cell)
        {
            for (int i = 0; i < Length; i++)
            {
                if (GetCell(i) == cell)
                    return true;
            }

            return false;
        }
    }
}
