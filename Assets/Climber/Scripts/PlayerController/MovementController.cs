using UnityEngine;

namespace Movement
{
    public class MovementController
    {
        [HideInInspector] public Transform playerTransform;
        private IMovementControllable player;
        private MovementConfig config;
        private float deltaTime;

        public bool jumping = false;

        Vector3 groundNormal = Vector3.up;

        public void ProcessMovement(IMovementControllable player, MovementConfig config, float deltaTime)
        {
            // Cache parameters to use in other functions
            this.player = player;
            this.config = config;
            this.deltaTime = deltaTime;

            if (player.moveData.velocity.y <= 0f)
                jumping = false;
            
            // Apply gravity
            if (player.groundObject == null)
            {
                player.moveData.velocity.y -= (player.moveData.gravityFactor * config.gravity * deltaTime);
                player.moveData.velocity.y += player.baseVelocity.y * deltaTime;
            }


            CheckForGround();
            CalculateMovementVelocity();


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
            var traceDestination = player.moveData.origin;
            traceDestination.y -= config.groundCheckDistance;
            var trace = PhysicsUtil.TraceCollider(player.collider, player.moveData.origin, traceDestination, MovementPhysics.groundLayerMask);
            float groundSlope = Vector3.Angle(Vector3.up, trace.planeNormal);

            var movingUp = player.moveData.velocity.y > 0f;
            player.moveData.surfaceFriction = 1f;

            if (trace.hitCollider == null || groundSlope > config.slopeLimit || (jumping && movingUp))
            {
                player.groundObject = null;
                return false;
            }
            else
            {
                groundNormal = trace.planeNormal;
                player.groundObject = trace.hitCollider.gameObject;
                player.moveData.velocity.y = 0f;
                return true;
            }
        }

        private void CalculateMovementVelocity()
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

                player.moveData.velocity.y += config.jumpForce;
                jumping = true;
            }
            else
            {   // Ground movement
                
                player.moveData.velocity += MovementPhysics.Friction(
                    player.moveData.velocity,
                    player.moveData.surfaceFriction * config.friction,
                    deltaTime,
                    config.deceleration,
                    true
                );

                // Get movement directions relative to ground slope
                Vector3 forward = Vector3.Cross(groundNormal, -playerTransform.right);
                Vector3 right = Vector3.Cross(groundNormal, forward);

                Vector3 moveVector = Vector3.ClampMagnitude(
                    player.moveData.verticalAxis * forward + player.moveData.horizontalAxis * right, 1f);

                if (moveVector.magnitude > 0f)
                    player.moveData.velocity += MovementPhysics.Accelerate(
                        player.moveData.velocity,
                        moveVector.normalized,
                        moveVector.magnitude * config.walkSpeed,
                        config.acceleration * moveVector.magnitude * Mathf.Min(player.moveData.surfaceFriction, 1f),
                        deltaTime
                    );

                // Clamp horizontal velocity
                player.moveData.velocity.y = 0f;
                player.moveData.velocity = Vector3.ClampMagnitude(player.moveData.velocity, config.maxVelocity);

                // Get velocity direction along ground slope
                Vector3 velocityDirection = Vector3.Cross(
                    groundNormal,
                    Quaternion.AngleAxis(-90, Vector3.up) * player.moveData.velocity
                ).normalized;

                // Set vertical velocity to follow ground slope
                player.moveData.velocity.y = velocityDirection.y * player.moveData.velocity.magnitude;
            }
        }

    }

}