using Character;
using UnityEngine;

namespace Equipment
{
    abstract public class BaseEquipment : MonoBehaviour
    {
        [SerializeField] protected GameObject characterObject;
        protected BaseCharacter character;
        [SerializeField] protected GameObject raycastOrigin;
        [SerializeField] protected float cooldown;
        protected float lastUsed = -1000f;

        protected void Awake()
        {
            character = characterObject.GetComponent<BaseCharacter>();
        }

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
