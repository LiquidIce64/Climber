using UnityEngine;
using Visuals;

namespace Interactables
{
    [RequireComponent(typeof(AudioSource))]
    public class Lever : BaseToggleable, IInteractable
    {
        [SerializeField] protected float _energyCost = 0f;
        [SerializeField] protected ToggleMaterial _indicators;
        [SerializeField] protected ToggleAnimation _animation;
        [SerializeField] protected AudioClip _toggleOnSound;
        [SerializeField] protected AudioClip _toggleOffSound;
        protected AudioSource _audioSource;

        public float EnergyCost => _energyCost;
        virtual public bool CanInteract => true;

        public void OnInteract() => _connector.Toggle();

        new protected void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
        }

        override protected void Start()
        {
            _indicators.UpdateMaterial(Toggled);
            _animation.ForceToggle(Toggled);
        }

        override protected void Enabled()
        {
            _indicators.UpdateMaterial(true);
            _animation.UpdateToggle(true);
            _audioSource.clip = _toggleOnSound;
            _audioSource.Play();
        }

        override protected void Disabled()
        {
            _indicators.UpdateMaterial(false);
            _animation.UpdateToggle(false);
            _audioSource.clip = _toggleOffSound;
            _audioSource.Play();
        }
    }
}
