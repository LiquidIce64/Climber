using UnityEngine;

namespace Movement
{

    [System.Serializable]
    public class MovementConfig
    {

        [Header("Jumping and gravity")]
        public bool autoBhop = true;
        public float gravity = 20f;
        public float jumpForce = 6.5f;
        public float climbVelocity = 10f;

        [Header("General physics")]
        public float friction = 6f;
        public float maxVelocity = 50f;
        [Range(30f, 75f)] public float slopeLimit = 45f;

        [Header("Air movement")]
        public float strafeSpeed = 0.4f;
        public float airAcceleration = 12f;
        public float overbounce = 1.001f;

        [Header("Ground movement")]
        public float walkSpeed = 7f;
        public float acceleration = 98f;
        public float deceleration = 10f;
        public float groundCheckDistance = 0.15f;

    }

}