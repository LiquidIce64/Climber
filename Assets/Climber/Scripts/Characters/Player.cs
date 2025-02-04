using Movement;
using Equipment;
using UnityEngine;
using InteractableItems;

namespace Character
{
    public class Player : BaseCharacter
    {
        public InputConfig inputConfig;

        [SerializeField] protected float maxEnergy = 100f;
        protected float energy;

        [SerializeField] private ClimbTool climbTool;
        [SerializeField] private Railgun railgun;
        private BaseEquipment equipped;
        private AudioSource audioSource;

        [SerializeField] private GameObject pauseMenu;
        private bool paused = false;

        [SerializeField] private GameObject gameOverScreen;

        public float Energy { get { return energy; } }
        public float MaxEnergy { get { return maxEnergy; } }

        protected new void Awake()
        {
            base.Awake();

            audioSource = GetComponent<AudioSource>();
            energy = maxEnergy;

            railgun.OnUnequipped();
            climbTool.OnEquipped();
            equipped = climbTool;

            // Hide cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        protected new void Update()
        {
            // Pause Menu
            if (Input.GetKeyDown(KeyCode.Escape)) TogglePauseMenu();

            // Movement
            _moveData.verticalAxis = Input.GetAxisRaw("Vertical");
            _moveData.horizontalAxis = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump"))
                _moveData.desiredJump = true;
            if (!Input.GetButton("Jump"))
                _moveData.desiredJump = false;

            // Get mouse inputs
            float mouseX = 0f;
            float mouseY = 0f;
            float mouseWheel = 0f;
            bool mouse1 = false;
            bool key1 = false;
            bool key2 = false;
            if (!paused)
            {
                mouseX = Input.GetAxisRaw("Mouse X") * inputConfig.sensX;
                mouseY = Input.GetAxisRaw("Mouse Y") * inputConfig.sensY;
                mouseWheel = Input.GetAxisRaw("Mouse ScrollWheel");
                mouse1 = Input.GetMouseButton(0);
                key1 = Input.GetKeyDown(KeyCode.Alpha1);
                key2 = Input.GetKeyDown(KeyCode.Alpha2);
            }

            // Equipment
            if (equipped != climbTool && (mouseWheel < 0 || key2))
            {
                equipped.OnUnequipped();
                climbTool.OnEquipped();
                equipped = climbTool;
            }
            else if (equipped != railgun && (mouseWheel > 0 || key1))
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

        override public void OnKilled()
        {
            Instantiate(gameOverScreen, viewTransform.position, viewTransform.rotation);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            base.OnKilled();
        }

        public void PlaySound(AudioClip sound)
        {
            audioSource.clip = sound;
            audioSource.Play();
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<ITriggerItem>(out var triggerItem))
            {
                triggerItem.TriggerAction(this);
            }
        }

        public void TogglePauseMenu()
        {
            paused = !paused;
            pauseMenu.SetActive(paused);
            Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = paused;
        }

    }
}
