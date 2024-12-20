using Character;
using UnityEngine;

namespace Equipment
{
    public class ClimbTool : BaseEquipment
    {
        public float rayDist = 2f;
        [Range(60f, 90f)] public float minWallAngle = 75f;

        private BaseCharacter character;

        protected void Awake()
        {
            character = GetComponent<BaseCharacter>();
        }

        override public void Use()
        {
            if (Time.time - lastUsed < cooldown) return;

            // If already moving upwards fast, no reason to climb
            if (character.moveData.velocity.y > character.moveConfig.climbVelocity) return;

            // Check if ray hit a wall
            if (!Physics.Raycast(raycastOrigin.transform.position, raycastOrigin.transform.forward, out RaycastHit hit, rayDist)) return;
            float a = Vector3.Angle(hit.normal, Vector3.up);
            if (minWallAngle <= a && a <= 180f - minWallAngle)
            {
                character.moveData.desiredClimb = true;
                lastUsed = Time.time;
            }
        }

    }
}
