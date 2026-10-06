using UnityEngine;

namespace ColorBlocks.Data
{
    public class LevelLoader
    {
        private readonly TextAsset[] _levelFiles;
        public int LevelCount => _levelFiles.Length;

        public LevelLoader(TextAsset[] levelFiles)
        {
            _levelFiles = levelFiles;
        }

        public LevelData Load(int levelIndex)
        {
            return JsonUtility.FromJson<LevelData>(_levelFiles[levelIndex].text);
        }
    }
}