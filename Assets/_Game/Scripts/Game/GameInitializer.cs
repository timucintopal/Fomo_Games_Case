using System;
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
            boardView.Build(levelLoader.Load(levelIndex));
        }
        
        private void HandleSwipe(BlockView blockView, Direction direction)
        {
            if(!blockView.Block.CanMove(direction)) return;
            
            var (rowOffset, colOffset) = direction.ToOffset();
            Vector3 step = new Vector3(colOffset, 0f, -rowOffset);

            blockView.transform.position += step;
        }
    }
}
