using UnityEngine;
using UnityEngine.Events;

namespace Interactables
{
    public class Lever : BaseInteractable, IToggleable, IConnector
    {
        protected bool _toggled = false;
        [SerializeField] protected GameObject connector;
        protected IConnector _connector;
        protected UnityEvent _toggleEvent = new();

        public UnityEvent ToggleEvent { get { return _toggleEvent; } }

        public bool IsEnabled { get { return _toggled; } }

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
            if (_connector.IsEnabled) Enable();
            else Disable();
        }

        override public void OnInteract()
        {
            if (_toggled) Disable();
            else Enable();
        }

        public void Enable()
        {
            if (_toggled) return;
            _toggled = true;
            _toggleEvent.Invoke();
        }

        public void Disable()
        {
            if (!_toggled) return;
            _toggled = false;
            _toggleEvent.Invoke();
        }
    }
}