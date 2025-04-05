using Character;
using Movement;
using Unity.VisualScripting;
using UnityEngine;

namespace Interactables
{
    [RequireComponent(typeof (BoxCollider))]
    public class Fan : BaseToggleable, ITriggerVolume
    {
        [SerializeField] protected bool _reversed = false;
        [SerializeField] protected float _pushRange = 12f;
        [SerializeField] protected float _pushStrength = 20f;
        protected BoxCollider _pushVolume;

        public bool Reversed => _reversed;

        protected new void Awake()
        {
            base.Awake();
            _pushVolume = GetComponent<BoxCollider>();
        }

        protected void OnValidate()
        {
            _pushVolume = GetComponent<BoxCollider>();
            _pushVolume.isTrigger = true;
            _pushVolume.center = new Vector3(0, _pushRange / 2, 0);
            _pushVolume.size = new Vector3(4, _pushRange, 4);
        }

        protected override void Enabled()
        {
            _pushVolume.enabled = true;
        }

        protected override void Disabled()
        {
            _pushVolume.enabled = false;
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