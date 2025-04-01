using UnityEngine;

namespace Interactables
{
    public class DebugToggleMaterial : BaseToggleable
    {
        [SerializeField] protected Material onMaterial;
        [SerializeField] protected Material offMaterial;
        protected MeshRenderer meshRenderer;

        public bool IsEnabled { get { return _toggled; } }

        protected new void Awake()
        {
            base.Awake();
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
        }

        override protected void _Enabled()
        {
            meshRenderer.material = onMaterial;
        }

        override protected void _Disabled()
        {
            meshRenderer.material = offMaterial;
        }
    }
}