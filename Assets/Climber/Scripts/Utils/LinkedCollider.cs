using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(Collider))]
    public class LinkedCollider : MonoBehaviour
    {
        [SerializeField] protected GameObject _parentObject;

        public GameObject ParentObject => _parentObject;
    }
}
