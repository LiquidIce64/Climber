using UnityEngine;

namespace Equipment
{
    abstract public class BaseEquipment : MonoBehaviour
    {
        [SerializeField] protected GameObject raycastOrigin;
        [SerializeField] protected float cooldown;
        protected float lastUsed = 0f;

        abstract public void Use();
    }
}
