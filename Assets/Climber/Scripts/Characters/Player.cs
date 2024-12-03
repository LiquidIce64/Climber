using Movement;
using Equipment;
using UnityEngine;

namespace Character
{
    [RequireComponent(typeof(ClimbTool), typeof(Railgun))]
    public class Player : BaseCharacter
    {
        public InputConfig inputConfig;

        private ClimbTool climbTool;
        private Railgun railgun;
        private BaseEquipment equipped;

        private void Awake()
        {
            // Hide cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private new void Start()
        {
            // Init equipment
            climbTool = GetComponent<ClimbTool>();
            railgun = GetComponent<Railgun>();
            equipped = climbTool;
            
            base.Start();
        }

        private new void Update()
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
            viewAngle = Mathf.Clamp(viewAngle + mouseY, -maxViewAngle, maxViewAngle);
            viewObject.transform.localRotation = Quaternion.AngleAxis(viewAngle, Vector3.left);
            _moveData.viewTransform = viewObject.transform;

            base.Update();
        }

    }
}
