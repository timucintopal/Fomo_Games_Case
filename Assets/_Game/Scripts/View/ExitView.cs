using UnityEngine;

namespace ColorBlocks.View
{
    public class ExitView : MonoBehaviour
    {
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

        [SerializeField] private Renderer[] renderers;

        public void SetColor(Color color)
        {
            var mat = new MaterialPropertyBlock();
            mat.SetColor(BaseColor, color);

            foreach (var gateRenderer in renderers)
                gateRenderer.SetPropertyBlock(mat);
        }
    }
}