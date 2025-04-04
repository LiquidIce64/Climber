using Unity.VisualScripting;
using UnityEngine;

namespace Movement
{
    public class MovementController
    {
        [HideInInspector] public Transform playerTransform;
        private IMovementControllable player;
        private MovementConfig config;

        public bool jumping = false;

        Vector3 groundNormal = Vector3.up;

        public MovementController(IMovementControllable player, MovementConfig config)
        {
            this.player = player;
            this.config = config;
        }

        public void ProcessMovement(float deltaTime)
        {
            // Apply gravity
            if (player.groundObject == null)
            {
                player.moveData.velocity.y -= (player.moveData.gravityFactor * config.gravity * deltaTime);
                player.moveData.velocity.y += player.baseVelocity.y * deltaTime;
            }


            CheckForGround();
            CalculateMovementVelocity(deltaTime);


            // Clamp horizontal velocity
            float yVel = player.moveData.velocity.y;
            player.moveData.velocity.y = 0f;
            player.moveData.velocity = Vector3.ClampMagnitude(player.moveData.velocity, config.maxVelocity);
            player.moveData.velocity.y = yVel;


            if (player.moveData.velocity.sqrMagnitude == 0f)
            {
                // Do collisions while standing still
                MovementPhysics.ResolveCollisions(player.collider, ref player.moveData.origin,
                    ref player.moveData.velocity, player.moveData.rigidbodyPushForce, 1f);
            }
            else
            {
                // Move origin in small steps, checking for collisions
                float maxStep = 0.2f;
                Vector3 totalDistVec = player.moveData.velocity * deltaTime;
                float totalDist = totalDistVec.magnitude;
                float distLeft = totalDist;
                while (distLeft > 0f)
                {
                    float step = Mathf.Min(maxStep, distLeft);
                    distLeft -= step;
                    player.moveData.origin += totalDistVec * (step / totalDist);

                    MovementPhysics.ResolveCollisions(player.collider, ref player.moveData.origin,
                        ref player.moveData.velocity,player.moveData.rigidbodyPushForce, step / totalDist);
                }
            }

        }

        private bool CheckForGround()
        {
            player.moveData.surfaceFriction = 1f;

            var traceDestination = player.moveData.origin;
            traceDestination.y -= config.groundCheckDistance;
            var trace = PhysicsUtil.TraceCollider(player.collider, player.moveData.origin, traceDestination, MovementPhysics.groundLayerMask);

            // Snap movement to downward slopes
            if (trace.hitCollider == null && player.groundObject != null)
            { // Just stepped off an edge
                Vector3 rayOrigin = player.moveData.origin;
                rayOrigin.y -= player.collider.bounds.extents.y;

                // Check if there's ground directly below
                if (Physics.Raycast(rayOrigin,Vector3.down, out var hit, 0.5f, MovementPhysics.groundLayerMask))
                {
                    trace.hitCollider = hit.collider;
                    trace.planeNormal = hit.normal;
                }
            }

            float groundSlope = Vector3.Angle(Vector3.up, trace.planeNormal);
            float velAwayFromGround = Vector3.Dot(player.moveData.velocity, trace.planeNormal);

            if (trace.hitCollider == null || groundSlope > player.moveData.slopeLimit || jumping && velAwayFromGround > 0.25f)
            {
                player.groundObject = null;
                return false;
            }
            jumping = false;
            groundNormal = trace.planeNormal;
            player.groundObject = trace.hitCollider.gameObject;
            player.moveData.velocity -= velAwayFromGround * groundNormal;
            return true;
        }

        private void CalculateMovementVelocity(float deltaTime)
        {
            if (player.moveData.desiredClimb)
            {   // Climb
                player.moveData.desiredClimb = false;
                player.moveData.velocity.y = config.climbVelocity;
                player.groundObject = null;
                jumping = true;
            }

            if (player.groundObject == null)
            {   // Air movement
                Vector3 moveVector = Vector3.ClampMagnitude(
                    player.moveData.verticalAxis * playerTransform.forward + player.moveData.horizontalAxis * playerTransform.right, 1f);

                if (moveVector.magnitude > 0f)
                    player.moveData.velocity += MovementPhysics.Accelerate(
                        player.moveData.velocity,
                        moveVector.normalized,
                        moveVector.magnitude * config.strafeSpeed,
                        config.airAcceleration * moveVector.magnitude,
                        deltaTime
                    );

                // Reflect off of surfaces, remembering change in velocity to calculate a collision vector
                var initVel = player.moveData.velocity;
                MovementPhysics.Reflect(
                    ref player.moveData.velocity,
                    player.collider,
                    player.moveData.origin,
                    deltaTime,
                    config.overbounce
                );
                var collisionVector = initVel - player.moveData.velocity;
                // TODO: using energy / taking damage proportional to collision vector

            }
            else if (player.moveData.desiredJump)
            {   // Jump
                if (!config.autoBhop) player.moveData.desiredJump = false;

                player.moveData.velocity.y = config.jumpVelocity;
                jumping = true;
            }
            else
            {   // Ground movement

                // Use horizontal velocity
                player.moveData.velocity.y = 0f;

                if (player.moveData.velocity.sqrMagnitude > 0f)
                    player.moveData.velocity += MovementPhysics.Friction(
                        player.moveData.velocity,
                        player.moveData.surfaceFriction * config.friction,
                        deltaTime,
                        config.deceleration
                    );

                Vector3 moveVector = Vector3.ClampMagnitude(
                    player.moveData.verticalAxis * playerTransform.forward + player.moveData.horizontalAxis * playerTransform.right, 1f);

                if (moveVector.sqrMagnitude > 0f)
                    player.moveData.velocity += MovementPhysics.Accelerate(
                        player.moveData.velocity,
                        moveVector.normalized,
                        moveVector.magnitude * config.walkSpeed,
                        config.acceleration * moveVector.magnitude * Mathf.Min(player.moveData.surfaceFriction, 1f),
                        deltaTime
                    );

                player.moveData.velocity = Vector3.ClampMagnitude(player.moveData.velocity, config.maxVelocity);

                // Get velocity direction along ground slope
                Vector3 velocityDirection = Vector3.Cross(
                    groundNormal,
                    Quaternion.AngleAxis(-90, Vector3.up) * player.moveData.velocity
                ).normalized;

                float tangent = Mathf.Tan(Vector3.Angle(velocityDirection, player.moveData.velocity) * Mathf.Deg2Rad);
                if (velocityDirection.y < 0f) tangent *= -1;

                // Set vertical velocity to follow ground slope
                player.moveData.velocity.y = tangent * player.moveData.velocity.magnitude;
            }
        }

    }

}
