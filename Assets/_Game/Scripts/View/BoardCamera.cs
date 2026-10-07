using UnityEngine;

namespace ColorBlocks.View
{
    public class BoardCamera : MonoBehaviour
    {
        [SerializeField] private float distancePerCell = 1.6f;
        [SerializeField] private float extraDistance = 3f;

        public void Fit(int rowCount, int colCount)
        {
            int biggestSide = Mathf.Max(rowCount, colCount);
            float distance = biggestSide * distancePerCell + extraDistance;

            // The board is centered on the world origin, so the camera backs away from it.
            transform.position = -transform.forward * distance;
        }
    }
}