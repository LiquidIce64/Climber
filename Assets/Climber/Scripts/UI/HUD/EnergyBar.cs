using Character;
using UnityEngine;

namespace UI
{
    public class EnergyBar : BaseBar
    {
        [SerializeField] Player player;

        protected new void Start()
        {
            base.Start();
            maxValue = player.MaxEnergy;
        }

        protected new void Update()
        {
            value = player.Energy;
            base.Update();
        }
    }
}
