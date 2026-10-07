using ColorBlocks.Core;
using DG.Tweening;
using UnityEngine;

namespace ColorBlocks.View
{
    public class BlockView : MonoBehaviour
    {
        private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");

        [SerializeField] private Renderer blockRenderer;
        [SerializeField] private ParticleSystem exitParticlePrefab;
        [SerializeField] private float secondsPerCell = 0.1f;
        [SerializeField] private float bumpDistance = 0.1f;
        [SerializeField] private float bumpDuration = 0.2f;
        [SerializeField] private float exitSecondsPerCell = 0.3f;
        [SerializeField] private float exitShakeAngle = 3f;
        [SerializeField] private int exitShakeVibrato = 30;
        
        private Quaternion _modelRotation;
        private Color _color;
        
        public bool IsMoving { get; private set; }
        
        public Block Block { get; private set; }
        
        private void Awake()
        {
            _modelRotation = blockRenderer.transform.localRotation;
        }
        
        public void Init(Block block, Texture texture, Color color)
        {
            IsMoving = false;
            Block = block;
            _color = color;
            blockRenderer.transform.localRotation = _modelRotation;

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
        
        // Slides to the gate, then grinds through it.
        public void SlideOut(Vector3 step, int cellsToGate)
        {
            IsMoving = true;

            Vector3 gatePosition = transform.position + step * cellsToGate;
            float duration = cellsToGate * secondsPerCell;

            transform.DOMove(gatePosition, duration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => GrindThroughGate(step));
        }

        // Moves slowly through the gate while shaking, then hides the block.
        private void GrindThroughGate(Vector3 step)
        {
            Vector3 outsidePosition = transform.position + step * Block.Length;
            float duration = Block.Length * exitSecondsPerCell;

            PlayExitParticle(step, duration);

            blockRenderer.transform.DOShakeRotation(duration, exitShakeAngle, exitShakeVibrato, 90f, false);

            transform.DOMove(outsidePosition, duration)
                .SetEase(Ease.Linear)
                .OnComplete(() => gameObject.SetActive(false));
        }

        public void Bump(Vector3 direction)
        {
            IsMoving = true;

            transform.DOPunchPosition(direction * bumpDistance, bumpDuration)
                .OnComplete(() => IsMoving = false);
        }
        
        private void PlayExitParticle(Vector3 step, float duration)
        {
            Vector3 gatePoint = transform.position + step * (Block.Length / 2f);
            Quaternion rotation = Quaternion.LookRotation(step);
            ParticleSystem particle = Instantiate(exitParticlePrefab, gatePoint, rotation);

            var main = particle.main;
            main.startColor = _color;
            main.duration = duration;

            particle.Play();
        }

    }
}