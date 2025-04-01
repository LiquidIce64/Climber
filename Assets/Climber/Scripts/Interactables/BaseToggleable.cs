using UnityEngine;

namespace Interactables
{
    abstract public class BaseToggleable : MonoBehaviour
    {
        protected bool _toggled = false;
        [SerializeField] protected GameObject connector;
        protected IConnector _connector;

        protected void OnValidate()
        {
            if (connector != null && !connector.TryGetComponent<IConnector>(out var _))
                Debug.LogError("Could not get connector component from object");
        }

        protected void Awake()
        {
            if (connector != null)
            {
                connector.TryGetComponent(out _connector);
                _connector?.ToggleEvent.AddListener(ToggleEventHandler);
            }
        }

        protected void ToggleEventHandler()
        {
            if (_connector.Toggled) Enable();
            else Disable();
        }

        public void Enable()
        {
            if (_toggled) return;
            _toggled = true;
            _Enabled();
        }

        public void Disable()
        {
            if (!_toggled) return;
            _toggled = false;
            _Disabled();
        }

        abstract protected void _Enabled();
        abstract protected void _Disabled();
    }
}