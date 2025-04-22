using UnityEngine;

namespace Interactables
{
    abstract public class BaseToggleable : MonoBehaviour
    {
        [SerializeField] protected Connector _connector;

        public bool Toggled => _connector.Toggled;

        protected void Awake()
        {
            if (_connector != null)
                _connector.ToggleEvent.AddListener(ToggleEventHandler);
        }

        protected void ToggleEventHandler()
        {
            if (_connector.Toggled) Enabled();
            else Disabled();
        }

        virtual protected void Enabled() { }
        virtual protected void Disabled() { }
    }
}