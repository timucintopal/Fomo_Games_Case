using UnityEngine;

namespace ColorBlocks.Data
{
    [CreateAssetMenu(menuName = "ColorBlocks/Level Catalog")]
    public class LevelCatalog : ScriptableObject
    {
        [SerializeField] private TextAsset[] levelFiles;

        public LevelData Load(int levelIndex)
        {
            int fileIndex = levelIndex % levelFiles.Length;
            return JsonUtility.FromJson<LevelData>(levelFiles[fileIndex].text);
        }
    }
}