using Character;
using Visuals;
using UnityEngine;

namespace Interactables
{
    [RequireComponent(typeof (BoxCollider), typeof(ParticleSystem))]
    public class Fan : BaseToggleable, ITriggerVolume
    {
        [SerializeField] protected Connector _reverseConnector;
        [SerializeField] protected bool _reversed = false;
        [SerializeField] protected float _pushRange = 12f;
        [SerializeField] protected float _pushStrength = 20f;
        [SerializeField] protected FanBlades _blades;
        [SerializeField] protected ToggleMaterial _indicators;
        protected BoxCollider _pushVolume;
        protected ParticleSystem.EmissionModule _windEmitter;
        protected ParticleSystem.ShapeModule _windShape;

        public bool Reversed => _reversed;

        [ContextMenu("Reverse")]
        protected void Reverse()
        {
            _reversed = !_reversed;
            _blades.UpdateVelocity(this);
            _indicators.UpdateMaterial(Toggled, _reversed);
            UpdateWindShape();
        }

        protected void UpdateWindShape()
        {
            if (_reversed)
            {
                _windShape.position = new Vector3(0f, _pushRange, 0f);
                _windShape.scale = new Vector3(1f, 1f, -_pushRange);
            }
            else
            {
                _windShape.position = new Vector3(0f, 0f, 0f);
                _windShape.scale = new Vector3(1f, 1f, _pushRange);
            }
        }

        protected new void Awake()
        {
            base.Awake();
            if (_reverseConnector != null)
                _reverseConnector.ToggleEvent.AddListener(Reverse);
            _pushVolume = GetComponent<BoxCollider>();

            var wind = GetComponent<ParticleSystem>();
            _windEmitter = wind.emission;
            _windShape = wind.shape;
        }

        protected void OnValidate()
        {
            _pushVolume = GetComponent<BoxCollider>();
            _pushVolume.isTrigger = true;
            _pushVolume.center = new Vector3(0, _pushRange / 2, 0);
            _pushVolume.size = new Vector3(4, _pushRange, 4);

            _indicators.UpdateMaterialInEditor(true, _reversed);

            var wind = GetComponent<ParticleSystem>();
            _windEmitter = wind.emission;
            _windShape = wind.shape;
            UpdateWindShape();
        }

        protected override void Enabled()
        {
            _pushVolume.enabled = true;
            _blades.UpdateVelocity(this);
            _indicators.UpdateMaterial(Toggled, _reversed);
            _windEmitter.enabled = true;
        }

        protected override void Disabled()
        {
            _pushVolume.enabled = false;
            _blades.UpdateVelocity(this);
            _indicators.UpdateMaterial(Toggled, _reversed);
            _windEmitter.enabled = false;
        }

        public void TriggerAction(Player player)
        {
            // Force multiplier based on squared distance from fan
            float distance = Vector3.Dot(player.moveData.origin - transform.position, transform.up);
            float distanceFrac = distance / _pushRange;
            float forceMult = Mathf.Clamp01(1f - distanceFrac * distanceFrac);

            // Push more if player is moving against push direction
            float velocityBoost = -Vector3.Dot(player.moveData.velocity, transform.up);
            if (_reversed)
            {
                forceMult = -forceMult;
                // Clamp to 0 if moving in the push direction
                velocityBoost = Mathf.Min(0f, velocityBoost);
            }
            else
            {
                // Clamp to 0 if moving in the push direction
                velocityBoost = Mathf.Max(0f, velocityBoost);
            }

            // Apply force
            float pushForce = _pushStrength * forceMult + velocityBoost;
            player.moveData.pushForce += pushForce * Time.deltaTime * transform.up;
        }
    }
}
