using ColorBlocks.Data;
using UnityEngine;

namespace ColorBlocks.View
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private BlockView[] blockPrefabs; // index = block length - 1
        [SerializeField] private ColorPalette palette;
        private Vector3 _origin;
        private LevelData _levelData;

        public void Build(LevelData level)
        {
            _levelData = level;
            _origin = new Vector3(-(_levelData.ColCount - 1) / 2f, 0f, (_levelData.RowCount - 1) / 2f);
            
            BuildCells();
            BuildBlocks();
        }

        private void BuildCells()
        {
            foreach (var cell in _levelData.CellInfo)
                Instantiate(cellPrefab, GridToWorld(cell.Row, cell.Col), Quaternion.identity, transform);
        }

        private void BuildBlocks()
        {
            foreach (var movable in _levelData.MovableInfo)
            {
                bool isVertical = movable.Direction[0] == Direction.Up || movable.Direction[0] == Direction.Down;

                // A block starts at (Row, Col) and extends down if vertical, right if horizontal.
                int lastRow = isVertical ? movable.Row + movable.Length - 1 : movable.Row;
                int lastCol = isVertical ? movable.Col : movable.Col + movable.Length - 1;

                Vector3 position = (GridToWorld(movable.Row, movable.Col) + GridToWorld(lastRow, lastCol)) / 2f;
                Quaternion rotation = isVertical ? Quaternion.identity : Quaternion.Euler(0f, 90f, 0f);

                Debug.Log("MOVABLE LENGTH " + movable.Length);
                var block = Instantiate(blockPrefabs[movable.Length - 1], position, rotation, transform);
                block.SetTexture(palette.GetBlockTexture(movable.Colors, movable.Length, isVertical));
            }
        }

        private Vector3 GridToWorld(int row, int col)
        {
            return _origin + new Vector3(col, 0f, -row);
        }
    }
}
