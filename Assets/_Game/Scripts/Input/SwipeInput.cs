using System;
using ColorBlocks.Data;
using ColorBlocks.View;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ColorBlocks.Input
{
    public class SwipeInput : MonoBehaviour
    {
        [SerializeField] private float minSwipePixels = 70f;
        [SerializeField] private LayerMask blockLayer;

        private BlockView _selected;
        private Vector2 _startPosition;
        private Camera _camera;
        
        public event Action<BlockView, Direction> OnSwipe;

        public void Init(Camera camera)
        {
            _camera = camera;
        }

        private void Update()
        {
            var pointer = Pointer.current;
            if (pointer == null)
                return;

            if (pointer.press.wasPressedThisFrame)
                Select(pointer.position.ReadValue());
            else if (pointer.press.isPressed && _selected != null)
                TrySwipe(pointer.position.ReadValue());
        }

        private void Select(Vector2 screenPosition)
        {
            _selected = null;
            _startPosition = screenPosition;

            var ray = _camera.ScreenPointToRay(screenPosition);
            
            if(Physics.Raycast(ray, out var hit, Mathf.Infinity, blockLayer))
                _selected = hit.collider.GetComponentInParent<BlockView>();
        }
        
        private void TrySwipe(Vector2 screenPosition)
        {
            Vector2 delta = screenPosition - _startPosition;
            if (delta.magnitude < minSwipePixels)
                return;

            Direction direction = ToDirection(delta);
            OnSwipe?.Invoke(_selected, direction);
            _selected = null;
        }
        
        private static Direction ToDirection(Vector2 delta)
        {
            bool isHorizontal = Mathf.Abs(delta.x) > Mathf.Abs(delta.y);

            if (isHorizontal)
                return delta.x > 0 ? Direction.Right : Direction.Left;
            return delta.y > 0 ? Direction.Up : Direction.Down;
        }
    }
}