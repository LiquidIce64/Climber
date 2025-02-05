using Character;
using Interactables;
using UnityEngine;

namespace Equipment
{
    public class Railgun : BaseEquipment
    {
        [SerializeField] private float knockback;
        [SerializeField] private float damage;
        [SerializeField] protected Transform rayOrigin;
        [SerializeField] protected GameObject energyRay;
        [SerializeField] protected GameObject sparkParticles;

        override public void Use()
        {
            if (Time.time - lastUsed < cooldown) return;
            if (player != null && player.TakeEnergy(energyConsumption) == 0f) return;

            var ray = Instantiate(energyRay);
            ray.layer = gameObject.layer;
            if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out RaycastHit hit))
            {
                if (hit.collider.gameObject.TryGetComponent<IDamageable>(out var damageable))
                    damageable.ApplyDamage(damage);

                if (hit.distance < 3f)
                {
                    ray.transform.SetPositionAndRotation(rayOrigin.position, rayOrigin.rotation);
                    ray.transform.localScale = new Vector3(1f, 1f, Mathf.Max(1f, hit.distance));
                }
                else
                {
                    Vector3 rayVec = hit.point - rayOrigin.position;
                    ray.transform.SetPositionAndRotation(rayOrigin.position, Quaternion.LookRotation(rayVec.normalized));
                    ray.transform.localScale = new Vector3(1f, 1f, rayVec.magnitude);
                }

                var particles = Instantiate(sparkParticles, hit.point, Quaternion.LookRotation(hit.normal));
                particles.layer = gameObject.layer;
            }
            else
            {
                ray.transform.SetPositionAndRotation(rayOrigin.position, rayOrigin.rotation);
                ray.transform.localScale = new Vector3(1f, 1f, 1000f);
            }

            character.moveData.velocity -= transform.forward * knockback;

            audioSource.Play();

            lastUsed = Time.time;
        }

    }
}
