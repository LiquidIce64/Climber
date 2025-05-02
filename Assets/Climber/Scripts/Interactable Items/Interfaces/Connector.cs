using UnityEngine;
using UnityEngine.Events;

namespace Interactables
{
    public class Connector: MonoBehaviour
    {
        [SerializeField] protected bool _toggled = false;
        protected UnityEvent _toggleEvent = new();
        [SerializeField] protected Connector _connector;

        public bool Toggled => _toggled;
        public UnityEvent ToggleEvent => _toggleEvent;

        protected void Awake()
        {
            if (_connector != null)
                _connector.ToggleEvent.AddListener(ToggleEventHandler);
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
            _toggleEvent.Invoke();
        }

        public void Disable()
        {
            if (!_toggled) return;
            _toggled = false;
            _toggleEvent.Invoke();
        }

        [ContextMenu("Toggle")]
        public void Toggle()
        {
            if (_toggled) Disable();
            else Enable();
        }
    }
}