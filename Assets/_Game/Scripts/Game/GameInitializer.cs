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
        [SerializeField] private BoardView boardPrefab;
        [SerializeField] private SwipeInput swipePrefab;
        [SerializeField] private HudView hudPrefab;
        [SerializeField] private BoardCamera boardCam;
        [SerializeField] private Camera mainCamera;

        private BoardView _boardView;
        private SwipeInput _swipeInput;
        private HudView _hudView;

        private GameController _gameController;

        private void Awake()
        {
            var levelLoader = new LevelLoader(levelFiles);
            
            Init();

            _gameController = new GameController(levelLoader, _boardView, _hudView, boardCam);
            _gameController.StartLevel(levelIndex);
        }

        private void Init()
        {
            _boardView = Instantiate(boardPrefab);
            _swipeInput = Instantiate(swipePrefab);
            _swipeInput.Init(mainCamera);
            _hudView = Instantiate(hudPrefab);
        }

        private void OnEnable()
        {
            _swipeInput.OnSwipe += _gameController.HandleSwipe;
        }

        private void OnDisable()
        {
            _swipeInput.OnSwipe -= _gameController.HandleSwipe;
        }
    }
}
