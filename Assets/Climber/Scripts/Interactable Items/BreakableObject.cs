using UnityEngine;

namespace Interactables
{
    public class BreakableObject : MonoBehaviour, IDamageable
    {
        [SerializeField] protected float health = 100f;

        public void ApplyDamage(float damage)
        {
            health -= damage;
            if (health <= 0) Destroy(gameObject);
        }
    }
}
