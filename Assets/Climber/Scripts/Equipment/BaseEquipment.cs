using Character;
using UnityEngine;

namespace Equipment
{
    abstract public class BaseEquipment : MonoBehaviour
    {
        [SerializeField] protected BaseCharacter character;
        protected Player player = null;
        [SerializeField] protected Transform raycastOrigin;
        [SerializeField] protected float cooldown;
        [SerializeField] protected float energyConsumption;
        protected float lastUsed = -1000f;
        [SerializeField] protected AudioSource audioSource;

        public float CooldownFraction { get {
            if (cooldown == 0f) return 0f;
            return Mathf.Clamp(1f - (Time.time - lastUsed) / cooldown, 0f, 1f);
        } }

        protected void Awake()
        {
            if (character is Player player) this.player = player;
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
