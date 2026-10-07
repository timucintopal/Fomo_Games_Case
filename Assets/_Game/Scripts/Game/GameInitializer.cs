using ColorBlocks.Data;
using ColorBlocks.Input;
using ColorBlocks.View;
using UnityEngine;

namespace ColorBlocks.Game
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private BoardView boardPrefab;
        [SerializeField] private SwipeInput swipePrefab;
        [SerializeField] private HudView hudPrefab;
        [SerializeField] private BoardCamera boardCam;
        [SerializeField] private LevelCatalog levelCatalog;
        
        private BoardView _boardView;
        private SwipeInput _swipeInput;
        private HudView _hudView;

        private GameController _gameController;

        private void Awake()
        {
            Init();

            _gameController = new GameController(levelCatalog, _boardView, _hudView, boardCam, new PlayerData());
            _gameController.Start();
        }

        private void Init()
        {
            _boardView = Instantiate(boardPrefab);
            _swipeInput = Instantiate(swipePrefab);
            _swipeInput.Init(boardCam.MainCamera);
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
