using UnityEngine;

namespace ColorBlocks.View
{
    public class BoardCamera : MonoBehaviour
    {
        [SerializeField] private float minDistance = 9f;
        [SerializeField] private float distancePerCell = 1.6f;
        [SerializeField] private float extraDistance = 3f;
        
        [SerializeField] private Camera mainCamera;
        public Camera MainCamera => mainCamera;

        public void Fit(int rowCount, int colCount)
        {
            int biggestSide = Mathf.Max(rowCount, colCount);
            float distance = biggestSide * distancePerCell + extraDistance;

            if (distance < minDistance)
                distance = minDistance;

            // The board is centered on the world origin, so the camera backs away from it.
            transform.position = -transform.forward * distance;
        }
    }
}