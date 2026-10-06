using System.Collections.Generic;
using ColorBlocks.Data;
using UnityEngine;

namespace ColorBlocks.Core
{
    public class Board
    {
        private readonly HashSet<Vector2Int> _cells = new HashSet<Vector2Int>();
        public IReadOnlyList<Block> Blocks => _blocks;
        
        private readonly List<Block> _blocks = new List<Block>();
        private readonly List<ExitData> _exits;
        
        public bool IsCleared => _blocks.Count == 0;

        public Board(LevelData level)
        {
            _exits = level.ExitInfo;
            _cells.Clear();
            _blocks.Clear();
            
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
        
        public bool TryExit(Block block, Direction direction)
        {
            Vector2Int frontCell = block.GetFrontCell(direction);

            foreach (var exit in _exits)
            {
                bool sameCell = exit.Col == frontCell.x && exit.Row == frontCell.y;
                bool sameDirection = exit.Direction == direction;
                bool sameColor = exit.Colors == block.Color;

                if (sameCell && sameDirection && sameColor)
                {
                    _blocks.Remove(block);
                    return true;
                }
            }

            return false;
        }
    }
}