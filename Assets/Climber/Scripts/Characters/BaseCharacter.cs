using Interactables;
using Movement;
using UnityEngine;


namespace Character
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    abstract public class BaseCharacter : MonoBehaviour, IMovementControllable, IDamageable
    {
        [Header("Physics Settings")]
        public float rigidbodyPushForce = 1f;

        [Header("View Settings")]
        [Range(75f, 90f)] public float maxViewAngle = 85f;
        public Transform viewTransform;

        [Header("Other")]
        [SerializeField] protected float maxHealth = 100f;
        protected float health;

        [SerializeField] protected MovementConfig movementConfig;

        [SerializeField] protected AudioSource _jumpAudio;
        [SerializeField] protected AudioSource _footstepAudio;

        protected GameObject _groundObject;
        protected Vector3 _baseVelocity;
        protected Collider _collider;

        protected MoveData _moveData = new();
        protected MovementController controller;

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
        public AudioSource jumpAudio => _jumpAudio;
        public AudioSource footstepAudio => _footstepAudio;

        public float Health { get { return health; } }
        public float MaxHealth { get { return maxHealth; } }

        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + _moveData.velocity * 0.25f);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + _moveData.pushForce * 5f);
        }

        protected void Awake()
        {
            health = maxHealth;

            _collider = GetComponent<Collider>();

            _moveData.slopeLimit = movementConfig.slopeLimit;
            _moveData.rigidbodyPushForce = rigidbodyPushForce;
            _moveData.playerTransform = transform;
            _moveData.origin = transform.position;
            _moveData.viewTransform = viewTransform;

            controller = new(this, movementConfig)
                { playerTransform = transform };
        }

        protected void Update()
        {
            viewAngle = Mathf.Clamp(viewAngle, -maxViewAngle, maxViewAngle);
            viewTransform.localRotation = Quaternion.AngleAxis(viewAngle, Vector3.left);
            _moveData.viewTransform = viewTransform;

            transform.position = _moveData.origin;
            _moveData.playerTransform = transform;

            controller.ProcessMovement(Time.deltaTime);
        }

        protected void OnCollisionStay(Collision collision)
        {
            if (collision.rigidbody == null) return;

            Vector3 impactVelocity = 0.00005f * collision.rigidbody.mass * collision.relativeVelocity;

            moveData.velocity = Vector3.ClampMagnitude(_moveData.velocity + impactVelocity, movementConfig.maxVelocity);
        }

        virtual public void ApplyDamage(float damage)
        {
            health -= damage;
            if (health <= 0f) {
                health = 0f;
                OnKilled();
            }
        }

        virtual public void OnKilled()
        {
            Destroy(gameObject);
        }

    }
}
