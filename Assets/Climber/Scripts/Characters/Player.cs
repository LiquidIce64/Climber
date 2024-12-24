using Movement;
using Equipment;
using UnityEngine;
using Unity.VisualScripting;

namespace Character
{
    [RequireComponent(typeof(ClimbTool))]
    public class Player : BaseCharacter
    {
        public InputConfig inputConfig;

        private ClimbTool climbTool;
        [SerializeField] private GameObject railgunObject;
        private Railgun railgun;
        private BaseEquipment equipped;

        protected void OnValidate()
        {
            if (railgunObject.GetComponent<Railgun>() == null)
                Debug.LogError("Railgun component not found");
        }

        protected void Awake()
        {
            // Hide cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        protected new void Start()
        {
            // Init equipment
            climbTool = GetComponent<ClimbTool>();
            railgun = railgunObject.GetComponent<Railgun>();
            railgun.OnUnequipped();
            climbTool.OnEquipped();
            equipped = climbTool;

            base.Start();
        }

        protected new void Update()
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
            bool mouse1 = Input.GetMouseButton(0);

            // Equipment
            if (equipped != climbTool && (mouseWheel < 0 || Input.GetKeyDown(KeyCode.Alpha2)))
            {
                equipped.OnUnequipped();
                climbTool.OnEquipped();
                equipped = climbTool;
                Debug.Log("equipped climbtool");
            }
            else if (equipped != railgun && (mouseWheel > 0 || Input.GetKeyDown(KeyCode.Alpha1)))
            {
                equipped.OnUnequipped();
                railgun.OnEquipped();
                equipped = railgun;
                Debug.Log("equipped railgun");
            }
            if (mouse1) equipped.Use();

            // View
            transform.Rotate(transform.up, mouseX);
            viewAngle += mouseY;

            base.Update();
        }

    }
}
