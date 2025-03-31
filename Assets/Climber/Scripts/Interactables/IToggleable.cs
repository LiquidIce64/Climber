namespace Interactables
{
    public interface IToggleable
    {
        bool IsEnabled { get; }

        void Enable();

        void Disable();
    }
}