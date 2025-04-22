using UnityEngine;

namespace Visuals
{
    [RequireComponent (typeof (MeshRenderer))]
    public class ToggleMaterial : MonoBehaviour
    {
        [SerializeField] protected Material _reverseMaterial;
        [SerializeField] protected Material _onMaterial;
        [SerializeField] protected Material _offMaterial;
        [SerializeField] protected int _materialIndex;
        protected MeshRenderer _meshRenderer;

        protected void OnValidate()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _materialIndex = Mathf.Clamp(_materialIndex, 0, _meshRenderer.sharedMaterials.Length - 1);
        }

        protected void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void UpdateMaterial(bool toggled, bool reversed = false)
        {
            var materials = _meshRenderer.materials;
            materials[_materialIndex] =
                toggled ? (
                    reversed ? _reverseMaterial : _onMaterial
                ) : _offMaterial;
            _meshRenderer.materials = materials;
        }

        public void UpdateMaterialInEditor(bool toggled, bool reversed = false)
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            var materials = _meshRenderer.sharedMaterials;
            materials[_materialIndex] =
                toggled ? (
                    reversed ? _reverseMaterial : _onMaterial
                ) : _offMaterial;
            _meshRenderer.materials = materials;
        }
    }
}