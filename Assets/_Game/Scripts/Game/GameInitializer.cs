using _Game.Data;
using UnityEngine;

namespace _Game.Scripts.Game
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private TextAsset[] levelFiles;

        [SerializeField] private int levelIndex = 0;

        private void Awake()
        {
            
        }

        [ContextMenu("Load Level")]
        void LoadLevel()
        {
            var levelLoader = new LevelLoader(levelFiles);
            var level = levelLoader.Load(levelIndex);

            Debug.Log($"Level loaded: {level.RowCount}x{level.ColCount}, " +
                      $"{level.MovableInfo.Count} blocks, {level.ExitInfo.Count} exits");
        }
    }
}
