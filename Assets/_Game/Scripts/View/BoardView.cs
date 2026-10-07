using System;
using System.Collections.Generic;
using ColorBlocks.Core;
using ColorBlocks.Data;
using UnityEngine;

namespace ColorBlocks.View
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private ExitView exitPrefab;
        [SerializeField] private BlockView[] blockPrefabs; // index = block length - 1
        [SerializeField] private ColorPalette palette;

        private Vector3 _origin;
        private LevelData _levelData;
        private Board _board;
        private Pooler<Transform> _cellPool;
        private Pooler<ExitView> _exitPool;

        private static readonly int BoardBounds = Shader.PropertyToID("_BoardBounds");
        
        private readonly List<Transform> _cells = new List<Transform>();
        private readonly List<ExitView> _exits = new List<ExitView>();
        private readonly List<BlockView> _blockViews = new List<BlockView>(); // not pooled, destroyed on clear
        

        private void Awake()
        {
            _cellPool = new Pooler<Transform>(cellPrefab.transform, transform);
            _exitPool = new Pooler<ExitView>(exitPrefab, transform);
        }

        public void Build(LevelData level, Board board)
        {
            Clear();
            _levelData = level;
            _board = board;
            _origin = new Vector3(-(_levelData.ColCount - 1) / 2f, 0f, (_levelData.RowCount - 1) / 2f);
            SetClipBounds();
            
            BuildCells();
            BuildBlocks();
            BuildExits();
        }
        
        // Blocks are not drawn outside these bounds: (minX, minZ, maxX, maxZ).
        private void SetClipBounds()
        {
            // An exit sits half a cell outside the board edge; its clip point is shifted from there.
            float clipMargin = 0.5f + exitPrefab.ClipOffset;

            float halfWidth = _levelData.ColCount / 2f + clipMargin;
            float halfDepth = _levelData.RowCount / 2f + clipMargin;

            Shader.SetGlobalVector(BoardBounds, new Vector4(-halfWidth, -halfDepth, halfWidth, halfDepth));
        }
        
        private void Clear()
        {
            foreach (var cellView in _cells)
                _cellPool.Release(cellView);
            _cells.Clear();

            foreach (var exitView in _exits)
                _exitPool.Release(exitView);
            _exits.Clear();

            foreach (var blockView in _blockViews)
            {
                if (blockView != null)
                    Destroy(blockView.gameObject);
            }
            _blockViews.Clear();
        }
        
        private void BuildCells()
        {
            foreach (var cell in _levelData.CellInfo)
            {
                Transform cellView = _cellPool.Get();
                cellView.position = GridToWorld(new Vector2Int(cell.Col, cell.Row));
                _cells.Add(cellView);
            }
        }

        private void BuildBlocks()
        {
            foreach (var block in _board.Blocks)
            {
                Vector3 firstCell = GridToWorld(block.GetCell(0));
                Vector3 lastCell = GridToWorld(block.GetCell(block.Length - 1));
                
                Vector3 position = (firstCell + lastCell) / 2f;
                Quaternion rotation = block.IsVertical ? Quaternion.identity : Quaternion.Euler(0f, 90f, 0f);

                var blockView = Instantiate(blockPrefabs[block.Length - 1], position, rotation, transform);
                var texture = palette.GetBlockTexture(block.Color, block.Length, block.IsVertical);
                var color = palette.GetColor(block.Color);
                
                blockView.Init(block, texture, color);
                _blockViews.Add(blockView);
            }
        }
        
        private void BuildExits()
        {
            foreach (var exit in _levelData.ExitInfo)
            {
                // An exit sits just outside its cell, on the side it opens to.
                Vector2Int exitCell = new Vector2Int(exit.Col, exit.Row);
                Vector2Int outsideCell = exitCell + exit.Direction.ToOffset();
                Vector3 position = GridToWorld(outsideCell);
                Quaternion rotation = Quaternion.Euler(0f, 90f * (int)exit.Direction, 0f);

                ExitView exitView = _exitPool.Get();
                exitView.transform.SetPositionAndRotation(position, rotation);
                exitView.SetColor(palette.GetColor(exit.Colors));

                _exits.Add(exitView);
            }
        }

        private Vector3 GridToWorld(Vector2Int cell)
        {
            return _origin + new Vector3(cell.x, 0f, -cell.y);
        }
    }
}
