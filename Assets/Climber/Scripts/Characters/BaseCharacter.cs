using Movement;
using System.Collections.Generic;
using UnityEngine;


namespace Character {
    abstract public class BaseCharacter : MonoBehaviour, IMovementControllable
    {

        public enum ColliderType
        {
            Capsule,
            Box
        }
        public ColliderType collisionType = ColliderType.Capsule;

        [Header("Physics Settings")]
        public Vector3 colliderSize = new(1f, 2f, 1f);
        public float weight = 75f;
        public float rigidbodyPushForce = 1f;
        public bool solidCollider = false;

        [Header("View Settings")]
        [Range(75f, 90f)] public float maxViewAngle = 85f;
        public GameObject viewObject;

        [Header("Configs")]
        public MovementConfig movementConfig;

        protected GameObject _groundObject;
        protected Vector3 _baseVelocity;
        protected Collider _collider;
        protected GameObject _colliderObject;

        protected MoveData _moveData = new();
        protected MovementController controller;

        protected Rigidbody rb;

        protected List<Collider> triggers = new();
        protected int numberOfTriggers = 0;

        protected float viewAngle = 0f;

        public MovementConfig moveConfig { get { return movementConfig; } }
        public MoveData moveData { get { return _moveData; } }
        public new Collider collider { get { return _collider; } }

        public GameObject groundObject
        {
            get { return _groundObject; }
            set { _groundObject = value; }
        }

        public Vector3 baseVelocity { get { return _baseVelocity; } }

        public Vector3 forward { get { return _moveData.viewTransform.forward; } }
        public Vector3 right { get { return _moveData.viewTransform.right; } }
        public Vector3 up { get { return _moveData.viewTransform.up; } }

        protected void OnDrawGizmos()
        {
            if (_groundObject == null)
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, colliderSize);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + _moveData.velocity * 0.25f);
        }

        protected void Start()
        {
            // Add a rigidbody
            rb = gameObject.GetComponent<Rigidbody>();
            if (rb == null)
                rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.angularDamping = 0f;
            rb.linearDamping = 0f;
            rb.mass = weight;

            // Add a collider (destroy and replace it if one already exists)
            _collider = gameObject.GetComponent<Collider>();
            if (_collider != null) Destroy(_collider);
            switch (collisionType)
            {
                case ColliderType.Box:
                    _collider = gameObject.AddComponent<BoxCollider>();
                    var boxc = (BoxCollider)_collider;
                    boxc.size = colliderSize;
                    break;

                case ColliderType.Capsule:
                    _collider = gameObject.AddComponent<CapsuleCollider>();
                    var capc = (CapsuleCollider)_collider;
                    capc.height = colliderSize.y;
                    capc.radius = colliderSize.x / 2f;
                    break;
            }
            _collider.isTrigger = !solidCollider;

            _moveData.slopeLimit = movementConfig.slopeLimit;
            _moveData.rigidbodyPushForce = rigidbodyPushForce;
            _moveData.playerTransform = transform;
            _moveData.origin = transform.position;
            _moveData.viewTransform = viewObject.transform;

            controller = new(this, movementConfig)
                { playerTransform = transform };
        }

        protected void Update()
        {
            viewAngle = Mathf.Clamp(viewAngle, -maxViewAngle, maxViewAngle);
            viewObject.transform.localRotation = Quaternion.AngleAxis(viewAngle, Vector3.left);
            _moveData.viewTransform = viewObject.transform;

            transform.position = _moveData.origin;
            _moveData.playerTransform = transform;

            // Handle triggers
            if (numberOfTriggers != triggers.Count)
            {
                triggers.RemoveAll(item => item == null);
                foreach (Collider trigger in triggers)
                {
                    if (trigger == null) continue;

                    // Put trigger handling here

                }
                numberOfTriggers = triggers.Count;
            }

            controller.ProcessMovement(Time.deltaTime);
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (!triggers.Contains(other))
                triggers.Add(other);
        }

        protected void OnTriggerExit(Collider other)
        {
            if (triggers.Contains(other))
                triggers.Remove(other);
        }

        protected void OnCollisionStay(Collision collision)
        {
            if (collision.rigidbody == null) return;

            Vector3 impactVelocity = 0.00005f * collision.rigidbody.mass * collision.relativeVelocity;

            moveData.velocity = Vector3.ClampMagnitude(_moveData.velocity + impactVelocity, movementConfig.maxVelocity);
        }

    }
}