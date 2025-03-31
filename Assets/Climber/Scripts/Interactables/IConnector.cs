using UnityEngine.Events;

namespace Interactables
{
    public interface IConnector
    {
        public UnityEvent ToggleEvent { get; }

        bool IsEnabled { get; }
    }
}