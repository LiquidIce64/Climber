using UnityEngine;
using Visuals;

namespace Interactables
{
    [RequireComponent(typeof(AudioSource))]
    public class Lever : BaseToggleable, IInteractable
    {
        [SerializeField] protected float _energyCost = 0f;
        [SerializeField] protected ToggleMaterial _indicators;
        [SerializeField] protected LeverHandle _handle;
        [SerializeField] protected AudioClip _toggleOnSound;
        [SerializeField] protected AudioClip _toggleOffSound;
        protected AudioSource _audioSource;

        public float EnergyCost => _energyCost;
        public bool CanInteract => true;

        public void OnInteract() => _connector.Toggle();

        protected new void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
        }

        protected override void Start()
        {
            _indicators.UpdateMaterial(Toggled);
            _handle.ForceToggle(Toggled);
        }

        protected override void Enabled()
        {
            _indicators.UpdateMaterial(true);
            _handle.UpdateToggle(true);
            _audioSource.clip = _toggleOnSound;
            _audioSource.Play();
        }

        protected override void Disabled()
        {
            _indicators.UpdateMaterial(false);
            _handle.UpdateToggle(false);
            _audioSource.clip = _toggleOffSound;
            _audioSource.Play();
        }
    }
}