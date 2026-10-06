using System.Collections.Generic;
using System.Linq;
using ColorBlocks.Data;

namespace ColorBlocks.Core
{
    public class Block
    {
        private IReadOnlyList<Direction> _directions;

        public Block(MovableData data)
        {
            _directions = data.Direction;
        }

        public bool CanMove(Direction direction)
        {
            return _directions.Contains(direction);
        }
    }
}
