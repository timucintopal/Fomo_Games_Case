using ColorBlocks.Core;
using UnityEngine;

namespace ColorBlocks.Data
{
    public class PlayerData
    {
        public int LevelIndex
        {
            get => PlayerPrefs.GetInt(Constants.LevelIndexKey, 0);
            set => PlayerPrefs.SetInt(Constants.LevelIndexKey, value);
        }
    }
}