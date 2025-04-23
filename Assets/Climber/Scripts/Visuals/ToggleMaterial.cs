using UnityEngine;
using Utils;

namespace Visuals
{
    [RequireComponent (typeof (MeshRenderer))]
    public class ToggleMaterial : MonoBehaviour
    {
        [SerializeField] MaterialArray _materialArray;
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

        protected Material GetMaterial(bool toggled, bool reversed)
        {
            return _materialArray.Materials[toggled ? (reversed ? 2 : 1) : 0];
        }

        public void UpdateMaterial(bool toggled, bool reversed = false)
        {
            var materials = _meshRenderer.materials;
            materials[_materialIndex] = GetMaterial(toggled, reversed);
            _meshRenderer.materials = materials;
        }

        public void UpdateMaterialInEditor(bool toggled, bool reversed = false)
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            var materials = _meshRenderer.sharedMaterials;
            materials[_materialIndex] = GetMaterial(toggled, reversed);
            _meshRenderer.materials = materials;
        }
    }
}