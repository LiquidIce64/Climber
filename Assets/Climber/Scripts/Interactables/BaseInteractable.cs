using UnityEngine;

namespace Interactables
{
    abstract public class BaseInteractable: BaseToggleable
    {
        [SerializeField] protected float _energyCost = 0f;

        public bool canInteract = true;
        
        public float EnergyCost { get { return _energyCost; } }

        public void OnInteract()
        {
            if (_toggled) Disable();
            else Enable();
        }
    }
}