using UnityEngine;

namespace Utils
{
    [CreateAssetMenu(fileName = "MaterialArray", menuName = "Scriptable Objects/MaterialArray")]
    public class MaterialArray : ScriptableObject
    {
        [SerializeField] private Material[] _materials;

        public Material[] Materials => _materials;
    }
}