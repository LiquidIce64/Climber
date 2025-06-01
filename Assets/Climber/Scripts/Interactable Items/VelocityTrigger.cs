using Character;
using Movement;
using UnityEngine;

namespace Interactables
{
    [RequireComponent(typeof(Collider))]
    public class VelocityTrigger : BaseToggleable, ITriggerVolume
    {
        [SerializeField] protected Vector3 targetVelocity = Vector3.up;
        [SerializeField] protected float minVelocity = 0f;
        protected Collider triggerCollider;

        protected void OnDrawGizmosSelected()
        {
            // Simulate velocity arc with gravity
            Gizmos.color = Color.green;
            Player player = FindAnyObjectByType<Player>();
            float gravity = (player != null) ? player.moveConfig.gravity : 20f;
            const float timeStep = 0.1f;
            const float timeLimit = 5f;
            float time = 0f;
            Vector3 vel = targetVelocity;
            Vector3 pos = transform.position;
            RaycastHit hit;

            while (!Physics.Raycast(
                pos,
                vel.normalized,
                out hit,
                vel.magnitude * timeStep,
                MovementPhysics.groundLayerMask
            ) && time <= timeLimit)
            {
                Vector3 newPos = pos + vel * timeStep;
                Gizmos.DrawLine(pos, newPos);

                vel += gravity * timeStep * Vector3.down;
                pos = newPos;
                time += timeStep;
            }
            if (hit.collider != null)
                Gizmos.DrawLine(pos, hit.point);

            // Draw targetVelocity vector
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(
                transform.position,
                transform.position + targetVelocity
            );

            // Draw minVelocity vector
            Gizmos.color = Color.red;
            Gizmos.DrawLine(
                transform.position,
                transform.position + targetVelocity.normalized * minVelocity
            );
        }

        protected new void Awake()
        {
            base.Awake();
            triggerCollider = GetComponent<Collider>();
        }

        protected override void Enabled() => triggerCollider.enabled = true;
        protected override void Disabled() => triggerCollider.enabled = false;

        public void TriggerAction(Player player)
        {
            // Get player's velocity in targetVelocity's direction
            float playerVel = Vector3.Dot(player.moveData.velocity, targetVelocity.normalized);
            if (playerVel < minVelocity) return;
            player.moveData.velocity = targetVelocity;
        }
    }
}