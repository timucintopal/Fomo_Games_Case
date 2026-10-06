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
        [SerializeField] private HudView hudView;

        private GameController _gameController;

        private void Awake()
        {
            var levelLoader = new LevelLoader(levelFiles);

            _gameController = new GameController(levelLoader, boardView, hudView);
            _gameController.StartLevel(levelIndex);
            
        }

        private void OnEnable()
        {
            swipeInput.OnSwipe += _gameController.HandleSwipe;
        }

        private void OnDisable()
        {
            swipeInput.OnSwipe -= _gameController.HandleSwipe;
        }
    }
}
