using UnityEngine;

namespace Interactables
{
    abstract public class BaseInteractable: MonoBehaviour
    {
        [SerializeField] protected float _energyCost = 0f;

        public bool canInteract = true;
        
        public float EnergyCost { get { return _energyCost; } }

        abstract public void OnInteract();
    }
}