using ColorBlocks.Data;
using UnityEngine;

namespace ColorBlocks.View
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private GameObject cellPrefab;
        private Vector3 _origin;

        public void Build(LevelData level)
        {
            _origin = new Vector3(-(level.ColCount - 1) / 2f, 0f, (level.RowCount - 1) / 2f);
            foreach (var cell in level.CellInfo)
                Instantiate(cellPrefab, GridToWorld(cell.Row, cell.Col), Quaternion.identity, transform);
        }

        private Vector3 GridToWorld(int row, int col)
        {
            return _origin + new Vector3(col, 0f, -row);
        }
    }
}
