using ColorBlocks.Data;
using ColorBlocks.View;
using UnityEngine;

namespace ColorBlocks.Game
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private TextAsset[] levelFiles;
        [SerializeField] private int levelIndex = 0;
        [SerializeField] private BoardView boardView;

        private void Awake()
        {
            LoadLevel();
        }

        private void LoadLevel()
        {
            var levelLoader = new LevelLoader(levelFiles);
            boardView.Build(levelLoader.Load(levelIndex));
        }
    }
}
