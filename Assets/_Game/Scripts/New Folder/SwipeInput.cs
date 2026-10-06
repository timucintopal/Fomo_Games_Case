using System;
using ColorBlocks.Data;
using ColorBlocks.View;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ColorBlocks.Input
{
    public class SwipeInput : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private float minSwipePixels = 30f;
        [SerializeField] private LayerMask blockLayer;

        [SerializeField] private BlockView _selected;
        [SerializeField] private Vector2 _startPosition;
        

        private void Update()
        {
            var pointer = Pointer.current;
            if (pointer == null)
                return;

            if (pointer.press.wasPressedThisFrame)
                Select(pointer.position.ReadValue());
        }

        private void Select(Vector2 screenPosition)
        {
            _startPosition = screenPosition;

            var ray = cam.ScreenPointToRay(screenPosition);
            
            if(Physics.Raycast(ray, out var hit, Mathf.Infinity, blockLayer))
            {
                Debug.Log("SELECT " + hit.collider.name);
                _selected = hit.collider.GetComponentInParent<BlockView>();
            }
            else
            {
                Debug.Log("SELECT FAIL");
            }
        }
    }
}