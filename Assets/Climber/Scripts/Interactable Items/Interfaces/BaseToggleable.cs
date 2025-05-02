using UnityEngine;

namespace Interactables
{
    abstract public class BaseToggleable : MonoBehaviour
    {
        [SerializeField] protected Connector _connector;

        public bool Toggled => (_connector == null) || _connector.Toggled;

        protected void Awake()
        {
            if (_connector != null)
                _connector.ToggleEvent.AddListener(ToggleEventHandler);
        }

        virtual protected void Start()
        {
            ToggleEventHandler();
        }

        protected void ToggleEventHandler()
        {
            if (Toggled) Enabled();
            else Disabled();
        }

        virtual protected void Enabled() { }
        virtual protected void Disabled() { }
    }
}