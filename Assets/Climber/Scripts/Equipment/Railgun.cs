using Character;
using UnityEngine;

namespace Equipment
{
    public class Railgun : BaseEquipment
    {
        [SerializeField] private GameObject energyRay;
        [SerializeField] private float knockback;
        [SerializeField] private float damage;

        override public void Use()
        {
            if (Time.time - lastUsed < cooldown) return;
            if (player != null && player.TakeEnergy(energyConsumption) == 0f) return;

            float length;

            if (Physics.Raycast(raycastOrigin.transform.position, raycastOrigin.transform.forward, out RaycastHit hit))
            {
                length = hit.distance;
                if (hit.collider.gameObject.TryGetComponent<BaseCharacter>(out var character))
                    character.ApplyDamage(damage);
            }
            else length = 1000f;

            var ray = Instantiate(energyRay, raycastOrigin.transform.position, raycastOrigin.transform.rotation);
            ray.transform.localScale = new Vector3(1f, 1f, length);

            character.moveData.velocity -= transform.forward * knockback;

            lastUsed = Time.time;
        }

    }
}
