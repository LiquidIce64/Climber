using UnityEngine;
using Utils;

namespace Visuals
{
    public class EnergyMaterial : MonoBehaviour
    {
        [SerializeField] protected MaterialArray _materialArray;
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

        protected Material GetMaterial(EnergyColor color)
        {
            return _materialArray.Materials[(int)color + 1];
        }

        public void UpdateMaterial(EnergyColor color)
        {
            var materials = _meshRenderer.materials;
            materials[_materialIndex] = GetMaterial(color);
            _meshRenderer.materials = materials;
        }

        public void UpdateMaterialInEditor(EnergyColor color)
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            var materials = _meshRenderer.sharedMaterials;
            materials[_materialIndex] = GetMaterial(color);
            _meshRenderer.materials = materials;
        }
    }
}