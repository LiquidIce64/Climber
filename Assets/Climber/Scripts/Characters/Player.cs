using Movement;
using Equipment;
using UnityEngine;

namespace Character
{
    public class Player : BaseCharacter
    {
        [SerializeField] protected float maxEnergy = 100f;
        protected float energy;

        public InputConfig inputConfig;

        [SerializeField] private ClimbTool climbTool;
        [SerializeField] private Railgun railgun;
        private BaseEquipment equipped;

        public float Energy { get { return energy; } }
        public float MaxEnergy { get { return maxEnergy; } }

        protected void Awake()
        {
            // Hide cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        protected new void Start()
        {
            energy = maxEnergy;

            // Init equipment
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
            }
            else if (equipped != railgun && (mouseWheel > 0 || Input.GetKeyDown(KeyCode.Alpha1)))
            {
                equipped.OnUnequipped();
                railgun.OnEquipped();
                equipped = railgun;
            }
            if (mouse1) equipped.Use();

            // View
            transform.Rotate(transform.up, mouseX);
            viewAngle += mouseY;

            base.Update();
        }

        public float AddEnergy(float amount)
        {
            float energyAdded = energy;
            energy = Mathf.Min(energy + amount, maxEnergy);
            energyAdded = energy - energyAdded;
            return energyAdded;
        }

        public float TakeEnergy(float amount, bool forced = false)
        {
            float energyTaken = 0f;
            if (energy >= amount)
            {
                energyTaken = amount;
                energy -= amount;
            }
            else if (forced)
            {
                energyTaken = energy;
                energy = 0f;
            }
            return energyTaken;
        }

        override public void ApplyDamage(float damage)
        {
            base.ApplyDamage(damage - TakeEnergy(damage, true));
        }

        override protected void OnKilled()
        {
            // TODO: game over screen
            health = 0f;
            Debug.Log("Dead");
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<ITriggerItem>(out var triggerItem))
            {
                triggerItem.TriggerAction(this);
            }
        }

    }
}
