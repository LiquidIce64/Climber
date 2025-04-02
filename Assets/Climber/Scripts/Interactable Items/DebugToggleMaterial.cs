using UnityEngine;

namespace Interactables
{
    public class DebugToggleMaterial : BaseToggleable
    {
        [SerializeField] protected Material onMaterial;
        [SerializeField] protected Material offMaterial;
        protected MeshRenderer meshRenderer;

        protected new void Awake()
        {
            base.Awake();
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
        }

        override protected void Enabled()
        {
            meshRenderer.material = onMaterial;
        }

        override protected void Disabled()
        {
            meshRenderer.material = offMaterial;
        }
    }
}