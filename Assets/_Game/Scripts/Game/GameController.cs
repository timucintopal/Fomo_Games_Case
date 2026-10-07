using ColorBlocks.Core;
using ColorBlocks.Data;
using ColorBlocks.View;
using DG.Tweening;
using UnityEngine;

namespace ColorBlocks.Game
{
    public class GameController
    {
        private const float LevelEndDelay = 1.5f;
        
        private readonly LevelLoader _levelLoader;
        private readonly BoardView _boardView;
        private readonly BoardCamera _boardCamera;
        private readonly HudView _hudView;

        private Board _board;
        private LevelData _level;
        private int _levelIndex;
        private int _movesLeft;
        private bool _isLevelOver;

        private bool HasMoveLimit => _level.MoveLimit > 0;

        public GameController(LevelLoader levelLoader, BoardView boardView, HudView hudView, BoardCamera boardCamera)
        {
            _hudView = hudView;
            _levelLoader = levelLoader;
            _boardView = boardView;
            _boardCamera = boardCamera;
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
            _boardCamera.Fit(_level.RowCount, _level.ColCount);

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

            if (blockView.IsMoving)
                return;

            Block block = blockView.Block;

            Vector2Int offset = direction.ToOffset();
            Vector3 step = new Vector3(offset.x, 0f, -offset.y);

            if (!block.CanMove(direction))
            {
                blockView.Bump(step);
                return;
            }

            int steps = _board.CountFreeSteps(block, direction);
            block.Move(direction, steps);

            bool exited = _board.TryExit(block, direction);

            if (steps == 0 && !exited)
            {
                blockView.Bump(step);
                return;
            }

            if (exited)
                blockView.SlideOut(step, steps);
            else
                blockView.Slide(step * steps, steps);

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