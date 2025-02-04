using Character;
using UnityEngine;

namespace InteractableItems
{
    public class DeathTrigger : MonoBehaviour, ITriggerItem
    {
        public void TriggerAction(Player player)
        {
            player.OnKilled();
        }
    }
}