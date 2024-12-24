using UnityEngine;

namespace Equipment
{
    abstract public class BaseEquipment : MonoBehaviour
    {
        [SerializeField] protected GameObject raycastOrigin;
        [SerializeField] protected float cooldown;
        protected float lastUsed = 0f;

        abstract public void Use();

        virtual public void OnEquipped()
        {
            gameObject.SetActive(true);
        }

        virtual public void OnUnequipped()
        {
            gameObject.SetActive(false);
        }
    }
}
