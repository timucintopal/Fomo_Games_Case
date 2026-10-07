using ColorBlocks.Core;
using DG.Tweening;
using UnityEngine;

namespace ColorBlocks.View
{
    public class BlockView : MonoBehaviour
    {
        private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");

        [SerializeField] private Renderer blockRenderer;
        [SerializeField] private float secondsPerCell = 0.1f;
        [SerializeField] private float bumpDistance = 0.1f;
        [SerializeField] private float bumpDuration = 0.2f;
        private Tween _tween;
        
        public bool IsMoving => DOTween.IsTweening(transform);
        
        public Block Block { get; private set; }
        
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

            Ease ease = removeAtEnd ? Ease.OutQuad : Ease.OutBack;
            _tween = transform.DOMove(target, duration).SetEase(ease);

            if (removeAtEnd)
                _tween.OnComplete(() => Destroy(gameObject));
        }
        
        public void Bump(Vector3 direction)
        {
            transform.DOPunchPosition(direction * bumpDistance, bumpDuration);
        }

    }
}