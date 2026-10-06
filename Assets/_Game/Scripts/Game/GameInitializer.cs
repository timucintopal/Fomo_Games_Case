using System;
using ColorBlocks.Core;
using ColorBlocks.Data;
using ColorBlocks.Input;
using ColorBlocks.View;
using UnityEngine;

namespace ColorBlocks.Game
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private TextAsset[] levelFiles;
        [SerializeField] private int levelIndex = 0;
        [SerializeField] private BoardView boardView;
        [SerializeField] private SwipeInput swipeInput;
        private Board _board;

        private void OnEnable()
        {
            swipeInput.OnSwipe += HandleSwipe;
        }
        
        private void OnDisable()
        {
            swipeInput.OnSwipe -= HandleSwipe;
        }

        private void Awake()
        {
            LoadLevel();
        }

        private void LoadLevel()
        {
            var levelLoader = new LevelLoader(levelFiles);
            var level = levelLoader.Load(levelIndex);

            _board = new Board(level);
            boardView.Build(level);
        }
        
        private void HandleSwipe(BlockView blockView, Direction direction)
        {
            Block block = blockView.Block;
            if(!block.CanMove(direction)) return;

            int steps = _board.CountFreeSteps(block, direction);
            
            block.Move(direction, steps);
            
            Vector2Int offset = direction.ToOffset();
            Vector3 step = new Vector3(offset.x, 0f, -offset.y);

            blockView.transform.position += step * steps;
        }
    }
}
