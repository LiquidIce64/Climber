using Character;
using UnityEngine;

namespace Equipment
{
    public class BasePickupItem: MonoBehaviour, ITriggerItem
    {
        [SerializeField] protected float rotationSpeed = 45f;

        protected void Update()
        {
            gameObject.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        virtual protected bool OnPickUp(Player player) { return true; }

        public void TriggerAction(Player player)
        {
            if (OnPickUp(player)) Destroy(gameObject);
        }
    }
}
