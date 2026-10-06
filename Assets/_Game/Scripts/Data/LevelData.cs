using System;
using System.Collections.Generic;

namespace ColorBlocks.Data
{
    [Serializable]
    public class LevelData
    {
        public int MoveLimit;
        public int RowCount;
        public int ColCount;
        public List<CellData> CellInfo;
        public List<MovableData> MovableInfo;
        public List<ExitData> ExitInfo;
    }
    
    [Serializable]
    public class CellData
    {
        public int Row;
        public int Col;
    }

    [Serializable]
    public class MovableData
    {
        public int Row;
        public int Col;
        public List<Direction> Direction;
        public int Length;
        public int Colors;
    }

    [Serializable]
    public class ExitData
    {
        public int Row;
        public int Col;
        public Direction Direction;
        public int Colors;
    }
}