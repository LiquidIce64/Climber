namespace Interactables
{
    public interface IInteractable
    {
        public bool CanInteract { get; }

        public float EnergyCost { get; }

        public void OnInteract();
    }
}