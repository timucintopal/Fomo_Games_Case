using ColorBlocks.Core;
using ColorBlocks.Data;
using ColorBlocks.View;
using DG.Tweening;
using UnityEngine;

namespace ColorBlocks.Game
{
    public class GameController
    {
        private readonly LevelLoader _levelLoader;
        private readonly BoardView _boardView;

        private Board _board;
        
        private readonly HudView _hudView;

        private LevelData _level;
        private int _levelIndex;
        private int _movesLeft;

        private bool HasMoveLimit => _level.MoveLimit > 0;
        
        private const float LevelEndDelay = 1f;

        private bool _isLevelOver;
        

        public GameController(LevelLoader levelLoader, BoardView boardView, HudView hudView)
        {
            _hudView = hudView;
            _levelLoader = levelLoader;
            _boardView = boardView;
        }

        public void StartLevel(int levelIndex)
        {
            _hudView.HideResult();
            _isLevelOver = false;
            _levelIndex = levelIndex;
            _level = _levelLoader.Load(levelIndex);
            _movesLeft = _level.MoveLimit;

            _board = new Board(_level);
            _boardView.Build(_level, _board);

            UpdateHud();
        }

        private void UpdateHud()
        {
            _hudView.SetLevel(_levelIndex + 1);

            if (HasMoveLimit)
                _hudView.SetMoves(_movesLeft);
            else
                _hudView.HideMoves();
        }

        public void HandleSwipe(BlockView blockView, Direction direction)
        {   
            if (_isLevelOver)
                return;
            
            if(blockView.IsMoving) return;
            
            Block block = blockView.Block;

            if (!block.CanMove(direction))
                return;

            int steps = _board.CountFreeSteps(block, direction);
            block.Move(direction, steps);

            bool exited = _board.TryExit(block, direction);

            if (steps == 0 && !exited)
                return;

            int viewSteps = steps;
            if (exited)
                viewSteps += block.Length;

            Vector2Int offset = direction.ToOffset();
            Vector3 step = new Vector3(offset.x, 0f, -offset.y);
            
            blockView.Slide(step * viewSteps, viewSteps, exited);

            if (HasMoveLimit)
                _movesLeft--;
            
            UpdateHud();
            CheckLevelEnd();
        }
        
        private void CheckLevelEnd()
        {
            if (_board.IsCleared)
            {
                Win();
                return;
            }

            bool isOutOfMoves = HasMoveLimit && _movesLeft == 0;

            if (isOutOfMoves)
                Fail();
        }

        private void Win()
        {
            _hudView.ShowSuccess();
            int nextLevelIndex = _levelIndex + 1;

            if (nextLevelIndex >= _levelLoader.LevelCount)
                nextLevelIndex = 0;

            LoadLevelAfterDelay(nextLevelIndex);
        }

        private void Fail()
        {
            _hudView.ShowFail();
            LoadLevelAfterDelay(_levelIndex);
        }

        private void LoadLevelAfterDelay(int levelIndex)
        {
            _isLevelOver = true;
            DOVirtual.DelayedCall(LevelEndDelay, () => StartLevel(levelIndex));
        }
    }
}