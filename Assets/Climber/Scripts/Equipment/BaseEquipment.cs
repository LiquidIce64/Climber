using UnityEngine;

namespace Equipment
{
    abstract public class BaseEquipment : MonoBehaviour
    {
        [SerializeField] protected GameObject raycastOrigin;

        abstract public void Use();
    }
}
