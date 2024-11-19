using UnityEngine;

namespace Movement
{
    public static class MovementPhysics
    {
        public static int groundLayerMask = LayerMask.GetMask(new string[]
        {
            "Default",
            "Ground",
            "Player clip"
        });

        private static Collider[] colliders = new Collider[maxCollisions];
        private static Vector3[] planes = new Vector3[maxClipPlanes];
        private const int maxCollisions = 128;
        private const int maxClipPlanes = 5;
        private const int maxBumps = 10;


        public static void ResolveCollisions(
            Collider collider,
            ref Vector3 origin,
            ref Vector3 velocity,
            float rigidbodyPushForce,
            float velocityMultiplier = 1f)
        {
            // manual collision resolving
            int numOverlaps = 0;
            if (collider is CapsuleCollider)
            {
                var capc = collider as CapsuleCollider;
                PhysicsUtil.GetCapsulePoints(capc, origin, out Vector3 point1, out Vector3 point2);

                numOverlaps = Physics.OverlapCapsuleNonAlloc(point1, point2, capc.radius,
                    colliders, groundLayerMask, QueryTriggerInteraction.Ignore);
            }
            else if (collider is BoxCollider)
            {
                numOverlaps = Physics.OverlapBoxNonAlloc(origin, collider.bounds.extents, colliders,
                    Quaternion.identity, groundLayerMask, QueryTriggerInteraction.Ignore);
            }

            Vector3 horizVel = velocity;
            horizVel.y = 0f;

            for (int i = 0; i < numOverlaps; i++)
            {
                if (Physics.ComputePenetration(collider, origin,
                    Quaternion.identity, colliders[i], colliders[i].transform.position,
                    colliders[i].transform.rotation, out Vector3 direction, out float distance))
                {
                    // Handle collision
                    direction.Normalize();
                    Vector3 penetrationVector = direction * distance;
                    Vector3 velocityProjected = Vector3.Project(velocity, -direction);
                    origin += penetrationVector;
                    velocity -= velocityProjected * velocityMultiplier;

                    Rigidbody rb = colliders[i].GetComponentInParent<Rigidbody>();
                    if (rb != null && !rb.isKinematic)
                        rb.AddForceAtPosition(rigidbodyPushForce * velocityMultiplier * velocityProjected, origin, ForceMode.Impulse);
                }
            }
        }

        public static Vector3 Friction(Vector3 velocity, float frictionMultiplier, float deltaTime, float minDeceleration = 0.1f, bool useHorizVel = false)
        {
            float speed;
            if (useHorizVel)
            {
                // Get horizontal speed
                float yVel = velocity.y;
                velocity.y = 0f;
                speed = velocity.magnitude;
                velocity.y = yVel;
            }
            else speed = velocity.magnitude;

            // No friction when not moving
            if (speed <= 0f) return Vector3.zero;

            // Calculate speed loss due to friction
            float control = Mathf.Max(speed, minDeceleration);
            float speedLoss = control * frictionMultiplier * deltaTime;

            // Return change in velocity
            return -Mathf.Min(speedLoss / speed, 1f) * velocity;
        }

        public static Vector3 Accelerate(Vector3 velocity, Vector3 desiredDirection, float desiredSpeed, float acceleration, float deltaTime)
        {
            // Calculate current velocity in the desired direction and how much to add
            float velInDesiredDir = Vector3.Dot(velocity, desiredDirection);
            float velToAdd = desiredSpeed - velInDesiredDir;

            // Don't reduce velocity
            if (velToAdd <= 0) return Vector3.zero;

            // Clamp acceleration to not go over desired speed
            float deltaVel = Mathf.Min(acceleration * deltaTime, velToAdd);

            // Return change in velocity
            return desiredDirection * deltaVel;
        }

        public static void Reflect(ref Vector3 velocity, Collider collider, Vector3 origin, float deltaTime, float overbounce = 1.001f)
        {
            var numplanes = 0;
            var totalFraction = 0f;
            var initialVelocity = velocity;
            var lastVelocity = velocity;

            for (int bumpcount = 0; bumpcount < maxBumps; bumpcount++)
            {
                if (velocity.magnitude == 0f) break; // Not moving

                // Try to move and check for collisions
                var trace = PhysicsUtil.TraceCollider(collider, origin, origin + deltaTime * velocity, groundLayerMask);
                totalFraction += trace.fraction;

                if (trace.fraction > 0) // Some distance covered
                {
                    lastVelocity = velocity;
                    numplanes = 0;
                }
                if (trace.fraction == 1) break; // Moved the entire distance

                // Advance time
                deltaTime *= 1f - trace.fraction;

                // Stop all movement if ran out of clip plane buffer
                if (numplanes > maxClipPlanes)
                {
                    velocity = Vector3.zero;
                    break;
                }

                // Add clipping plane
                planes[numplanes] = trace.planeNormal;
                numplanes++;

                // modify velocity so it parallels all of the clip planes
                if (numplanes == 1) ClipVelocity(lastVelocity, planes[0], ref velocity, overbounce);
                else
                {
                    // Find a plane to reflect off of to move away or parallel to all clipping planes
                    int i, j;
                    for (i = 0; i < numplanes; i++)
                    {
                        ClipVelocity(lastVelocity, planes[i], ref velocity, overbounce);

                        for (j = 0; j < numplanes; j++)
                            if (j != i && Vector3.Dot(velocity, planes[j]) < 0) break; // Moving towards another clipping plane

                        if (j == numplanes) break; // Plane found
                    }

                    if (i == numplanes) // Didn't find anything
                    {
                        if (numplanes != 2) // Hit a corner, stop all movement
                        {
                            velocity = Vector3.zero;
                            break;
                        }

                        // Slide along the crease
                        var dir = Vector3.Cross(planes[0], planes[1]).normalized;
                        velocity = Vector3.Project(velocity, dir);
                    }

                    // If trying to move backwards, stop all movement
                    if (Vector3.Dot(velocity, initialVelocity) <= 0f)
                    {
                        velocity = Vector3.zero;
                        break;
                    }
                }
            }
            
            if (totalFraction == 0f) velocity = Vector3.zero; // Couldn't move

            return;
        }

        private static void ClipVelocity(Vector3 input, Vector3 normal, ref Vector3 output, float overbounce)
        {
            // Slide along the plane
            output = input - overbounce * Vector3.Project(input, normal);

            // Small adjustment to prevent floating point inaccuracy
            float adjust = Vector3.Dot(output, normal);
            if (adjust < 0.0f) output -= adjust * normal;
        }

    }
}