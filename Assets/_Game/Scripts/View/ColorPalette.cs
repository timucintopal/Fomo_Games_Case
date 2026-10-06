using System;
using UnityEngine;

namespace ColorBlocks.View
{
    [CreateAssetMenu(menuName = "ColorBlocks/Color Palette")]
    public class ColorPalette : ScriptableObject
    {
        [SerializeField] private BlockColor[] colors; // index = color id in level json

        public Texture GetBlockTexture(int colorId, int length, bool isVertical)
        {
            var textures = colors[colorId].texturesByLength[length - 1];
            return isVertical ? textures.up : textures.parallel;
        }

        [Serializable]
        private class BlockColor
        {
            public string name;
            public Color color;
            public BlockTextures[] texturesByLength; // index = block length - 1
        }

        [Serializable]
        private class BlockTextures
        {
            public Texture up;
            public Texture parallel;
        }
        
        public Color GetColor(int colorId)
        {
            return colors[colorId].color;
        }
        
    }
}