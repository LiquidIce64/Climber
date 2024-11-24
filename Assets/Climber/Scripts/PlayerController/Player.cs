using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class Player : MonoBehaviour, IMovementControllable
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
        [Range(75f, 90f)] public float maxCameraAngle = 85f;

        [Header("Configs")]
        public MovementConfig movementConfig;
        public InputConfig inputConfig;

        private GameObject _groundObject;
        private Vector3 _baseVelocity;
        private Collider _collider;
        private GameObject _colliderObject;

        private MoveData _moveData = new();
        private MovementController controller;

        private Rigidbody rb;

        private List<Collider> triggers = new();
        private int numberOfTriggers = 0;

        private Camera _camera;
        private float cameraAngle = 0f;

        private ClimbTool climbTool;
        private Railgun railgun;
        private Equipment equipped;

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


        private void OnDrawGizmos()
        {
            if (_groundObject == null)
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, colliderSize);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + _moveData.velocity * 0.25f);
        }


        private void Awake()
        {
            controller = new(this, movementConfig)
                { playerTransform = transform };

            // Hide cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Init equipment
            climbTool = GetComponent<ClimbTool>();
            railgun = GetComponent<Railgun>();
            equipped = climbTool;
        }

        private void Start()
        {
            // Add a collider object
            _colliderObject = new GameObject("PlayerCollider")
                { layer = gameObject.layer };
            _colliderObject.transform.SetParent(transform);
            _colliderObject.transform.rotation = Quaternion.identity;
            _colliderObject.transform.localPosition = Vector3.zero;
            _colliderObject.transform.SetSiblingIndex(0);


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
                    _collider = _colliderObject.AddComponent<BoxCollider>();
                    var boxc = (BoxCollider)_collider;
                    boxc.size = colliderSize;
                    break;

                case ColliderType.Capsule:
                    _collider = _colliderObject.AddComponent<CapsuleCollider>();
                    var capc = (CapsuleCollider)_collider;
                    capc.height = colliderSize.y;
                    capc.radius = colliderSize.x / 2f;
                    break;
            }


            // Init variables
            _moveData.slopeLimit = movementConfig.slopeLimit;
            _moveData.rigidbodyPushForce = rigidbodyPushForce;
            _moveData.playerTransform = transform;
            _moveData.origin = transform.position;

            _camera = Camera.main;
            _moveData.viewTransform = _camera.transform;

            _collider.isTrigger = !solidCollider;
        }

        private void Update()
        {
            GetInputs();

            _colliderObject.transform.rotation = Quaternion.identity;
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

        private void GetInputs()
        {
            // Movement
            _moveData.verticalAxis = Input.GetAxisRaw("Vertical");
            _moveData.horizontalAxis = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump"))
                _moveData.desiredJump = true;
            if (!Input.GetButton("Jump"))
                _moveData.desiredJump = false;

            // Get mouse inputs
            float mouseX = Input.GetAxisRaw("Mouse X") * inputConfig.sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * inputConfig.sensY;
            float mouseWheel = Input.GetAxisRaw("Mouse ScrollWheel");
            bool mouse1 = Input.GetMouseButtonDown(0);

            // Equipment
            if (mouseWheel > 0)
            {
                equipped = climbTool;
                Debug.Log("equipped climbtool");
            }
            else if (mouseWheel < 0)
            {
                equipped = railgun;
                Debug.Log("equipped railgun");
            }
            if (mouse1) equipped.Use();

            // View
            transform.Rotate(transform.up, mouseX);
            cameraAngle = Mathf.Clamp(cameraAngle + mouseY, -maxCameraAngle, maxCameraAngle);
            _camera.transform.localRotation = Quaternion.AngleAxis(cameraAngle, Vector3.left);
            _moveData.viewTransform = _camera.transform;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!triggers.Contains(other))
                triggers.Add(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (triggers.Contains(other))
                triggers.Remove(other);
        }

        private void OnCollisionStay(Collision collision)
        {
            if (collision.rigidbody == null) return;

            Vector3 impactVelocity = 0.00005f * collision.rigidbody.mass * collision.relativeVelocity;

            moveData.velocity = Vector3.ClampMagnitude(_moveData.velocity + impactVelocity, movementConfig.maxVelocity);
        }

    }
}
