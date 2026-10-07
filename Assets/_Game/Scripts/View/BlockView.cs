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
        [SerializeField] private float exitSecondsPerCell = 0.3f;
        [SerializeField] private float exitShakeStrength = 0.1f;
        [SerializeField] private float exitShakeAngle = 3f;
        [SerializeField] private int exitShakeVibrato = 30;
        
        private Tween _tween;
        
        public bool IsMoving { get; private set; }
        
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
        
        public void Slide(Vector3 distance, int cellCount)
        {
            IsMoving = true;

            Vector3 target = transform.position + distance;
            float duration = cellCount * secondsPerCell;

            transform.DOMove(target, duration)
                .SetEase(Ease.OutBack)
                .OnComplete(() => IsMoving = false);
        }
        
        // Slides to the gate, then grinds through it slowly and is removed.
        public void SlideOut(Vector3 step, int cellsToGate)
        {
            IsMoving = true;

            Vector3 gatePosition = transform.position + step * cellsToGate;
            float duration = cellsToGate * secondsPerCell;

            transform.DOMove(gatePosition, duration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => GrindThroughGate(step));
        }

        // Moves slowly through the gate while shaking, then removes the block.
        private void GrindThroughGate(Vector3 step)
        {
            Vector3 outsidePosition = transform.position + step * Block.Length;
            float duration = Block.Length * exitSecondsPerCell;

            blockRenderer.transform.DOShakeRotation(duration, exitShakeAngle, exitShakeVibrato, 90f, false);

            transform.DOMove(outsidePosition, duration)
                .SetEase(Ease.Linear)
                .OnComplete(() => Destroy(gameObject));
        }

        public void Bump(Vector3 direction)
        {
            IsMoving = true;

            transform.DOPunchPosition(direction * bumpDistance, bumpDuration)
                .OnComplete(() => IsMoving = false);
        }

    }
}