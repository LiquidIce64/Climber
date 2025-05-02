using Interactables;
using UnityEngine;

namespace Visuals
{
    public class FanBlades : MonoBehaviour
    {
        [SerializeField] protected float speed = 720f;
        [SerializeField] protected float acceleration = 2880f;
        protected float _velocity = 0f;
        protected float _targetVelocity = 0f;
        protected float _angle = 0f;

        [ContextMenu("Update velocity")]
        public void UpdateVelocity(Fan fan)
        {
            _targetVelocity = fan.Toggled ? speed : 0f;
            if (!fan.Reversed) _targetVelocity = -_targetVelocity;
            enabled = true;
        }

        protected void Update()
        {
            _velocity = Mathf.MoveTowards(_velocity, _targetVelocity, acceleration * Time.deltaTime);

            transform.localRotation *= Quaternion.AngleAxis(_velocity * Time.deltaTime, Vector3.forward);

            // Disable component if not rotating to prevent unnecessary updates
            if (_velocity == 0f) enabled = false;
        }
    }
}