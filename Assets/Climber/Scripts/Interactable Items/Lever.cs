using UnityEngine;

namespace Interactables
{
    public class Lever : BaseToggleable, IInteractable
    {
        [SerializeField] protected float _energyCost = 0f;
        protected bool _canInteract = true;

        public float EnergyCost => _energyCost;
        public bool CanInteract => _canInteract;

        public void OnInteract() => _connector.Toggle();
    }
}