using Character;
using Visuals;
using UnityEngine;

namespace Interactables
{
    [RequireComponent(typeof (BoxCollider))]
    public class Fan : BaseToggleable, ITriggerVolume
    {
        [SerializeField] protected Connector _reverseConnector;
        [SerializeField] protected bool _reversed = false;
        [SerializeField] protected float _pushRange = 12f;
        [SerializeField] protected float _pushStrength = 20f;
        [SerializeField] protected FanBlades _blades;
        [SerializeField] protected ToggleMaterial _indicators;
        protected BoxCollider _pushVolume;

        public bool Reversed => _reversed;

        [ContextMenu("Reverse")]
        protected void Reverse()
        {
            _reversed = !_reversed;
            _blades.UpdateVelocity(this);
            _indicators.UpdateMaterial(Toggled, _reversed);
        }

        protected new void Awake()
        {
            base.Awake();
            if (_reverseConnector != null)
                _reverseConnector.ToggleEvent.AddListener(Reverse);
            _pushVolume = GetComponent<BoxCollider>();
        }

        protected void OnValidate()
        {
            _pushVolume = GetComponent<BoxCollider>();
            _pushVolume.isTrigger = true;
            _pushVolume.center = new Vector3(0, _pushRange / 2, 0);
            _pushVolume.size = new Vector3(4, _pushRange, 4);

            _indicators.UpdateMaterialInEditor(true, _reversed);
        }

        protected override void Enabled()
        {
            _pushVolume.enabled = true;
            _blades.UpdateVelocity(this);
            _indicators.UpdateMaterial(Toggled, _reversed);
        }

        protected override void Disabled()
        {
            _pushVolume.enabled = false;
            _blades.UpdateVelocity(this);
            _indicators.UpdateMaterial(Toggled, _reversed);
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