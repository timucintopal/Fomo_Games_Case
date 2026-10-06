using ColorBlocks.Core;
using DG.Tweening;
using UnityEngine;

namespace ColorBlocks.View
{
    public class BlockView : MonoBehaviour
    {
        public Block Block { get; private set; }
        
        private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");

        [SerializeField] private Renderer blockRenderer;
        [SerializeField] private float secondsPerCell = 0.1f;
        
        public bool IsMoving => DOTween.IsTweening(transform);
        private Tween _tween;
        
        public void Init(Block block, Texture texture)
        {
            Block = block;
            SetTexture(texture);
        }

        private void SetTexture(Texture texture)
        {
            var properties = new MaterialPropertyBlock();
            properties.SetTexture(BaseMap, texture);
            blockRenderer.SetPropertyBlock(properties);
        }
        
        public void Slide(Vector3 distance, int cellCount, bool removeAtEnd)
        {
            Vector3 target = transform.position + distance;
            float duration = cellCount * secondsPerCell;

            _tween = transform.DOMove(target, duration).SetEase(Ease.OutQuad);

            if (removeAtEnd)
                _tween.OnComplete(() => Destroy(gameObject));
        }

    }
}