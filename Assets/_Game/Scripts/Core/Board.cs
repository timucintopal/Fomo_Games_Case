using System.Collections.Generic;
using ColorBlocks.Data;
using UnityEngine;

namespace ColorBlocks.Core
{
    public class Board
    {
        private readonly HashSet<Vector2Int> _cells = new HashSet<Vector2Int>();
        
        private readonly List<Block> _blocks = new List<Block>();

        public IReadOnlyList<Block> Blocks => _blocks;

        public Board(LevelData level)
        {
            foreach (var cell in level.CellInfo)
                _cells.Add(new Vector2Int(cell.Col, cell.Row));
            
            foreach (var movable in level.MovableInfo)
                _blocks.Add(new Block(movable));
        }

        public int CountFreeSteps(Block block, Direction direction)
        {
            Vector2Int offset = direction.ToOffset();
            int steps = 0;

            while (CanStandAt(block, offset * (steps + 1)))
                steps++;

            return steps;
        }

        private bool CanStandAt(Block block, Vector2Int shift)
        {
            for (int i = 0; i < block.Length; i++)
            {
                Vector2Int cell = block.GetCell(i) + shift;

                if (!_cells.Contains(cell))
                    return false;
                if (IsOccupiedByOther(block, cell))
                    return false;
            }

            return true;
        }
        private bool IsOccupiedByOther(Block block, Vector2Int cell)
        {
            foreach (var other in _blocks)
            {
                if (other == block)
                    continue;

                if (other.Occupies(cell))
                    return true;
            }
            return false;
        }
    }
}