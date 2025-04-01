using UnityEngine.Events;

namespace Interactables
{
    public class Lever : BaseInteractable, IConnector
    {
        protected UnityEvent _toggleEvent = new();

        public UnityEvent ToggleEvent { get { return _toggleEvent; } }

        public bool Toggled { get { return _toggled; } }

        override protected void _Enabled()
        {
            _toggleEvent.Invoke();
        }

        override protected void _Disabled()
        {
            _toggleEvent.Invoke();
        }
    }
}