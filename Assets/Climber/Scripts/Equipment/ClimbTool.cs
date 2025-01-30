using Character;
using UnityEngine;

namespace Equipment
{
    public class ClimbTool : BaseEquipment
    {
        public float rayDist = 2f;
        [Range(60f, 90f)] public float minWallAngle = 75f;
        [SerializeField] protected Transform[] rayOrigins;
        [SerializeField] protected GameObject energyRay;
        [SerializeField] protected GameObject sparkParticles;

        protected new void Awake()
        {
            base.Awake();
            player = (Player)character;
        }

        override public void Use()
        {
            if (Time.time - lastUsed < cooldown) return;

            // If already moving upwards fast, no reason to climb
            if (character.moveData.velocity.y >= character.moveConfig.climbVelocity) return;

            // Check if ray hit a wall
            if (!Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out RaycastHit hit, rayDist)) return;
            float a = Vector3.Angle(hit.normal, Vector3.up);
            if (minWallAngle <= a && a <= 180f - minWallAngle)
            {
                if (player != null && player.TakeEnergy(energyConsumption) == 0f) return;
                character.moveData.desiredClimb = true;

                foreach (Transform origin in rayOrigins) {
                    Vector3 rayVec = hit.point - origin.position;
                    var ray = Instantiate(energyRay);
                    ray.layer = gameObject.layer;
                    ray.transform.SetPositionAndRotation(origin.position, Quaternion.LookRotation(rayVec.normalized));
                    ray.transform.localScale = new Vector3(1f, 1f, rayVec.magnitude);
                    var rayComp = ray.GetComponent<EnergyRay>();
                    rayComp.duration /= 2f;
                    rayComp.start_width *= origin.localScale.x;
                }
                var particles = Instantiate(sparkParticles, hit.point, Quaternion.LookRotation(hit.normal));
                particles.layer = gameObject.layer;

                audioSource.Play();

                lastUsed = Time.time;
            }
        }

    }
}
