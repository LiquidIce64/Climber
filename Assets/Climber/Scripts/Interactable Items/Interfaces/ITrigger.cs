using Character;

namespace Interactables
{
    public interface ITriggerItem
    {
        void TriggerAction(Player player);
    }

    public interface ITriggerVolume
    {
        void TriggerAction(Player player);
    }
}