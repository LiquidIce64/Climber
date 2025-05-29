using Character;
using UnityEngine;

namespace Interactables
{
    public class DeathTrigger : MonoBehaviour, ITriggerItem
    {
        public void TriggerAction(Player player)
        {
            player.OnKilled();
        }
    }
}
