using UnityEngine;

namespace Movement
{
    public class MoveData
    {
        public Transform playerTransform;
        public Transform viewTransform;
        public Vector3 origin;
        public Vector3 velocity;

        public float slopeLimit = 45f;
        public float surfaceFriction = 1f;
        public float gravityFactor = 1f;

        public float verticalAxis = 0f;
        public float horizontalAxis = 0f;
        public bool desiredJump = false;
        public bool desiredClimb = false;

        public float rigidbodyPushForce = 1f;
    }
}
