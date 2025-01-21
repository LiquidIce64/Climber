using Character;
using UnityEngine;

namespace Equipment
{
    public class EnergyCell : BasePickupItem
    {
        [SerializeField] protected float energy = 100f;

        override protected bool OnPickUp(Player player)
        {
            return player.AddEnergy(energy) > 0f;
        }
    }
}
