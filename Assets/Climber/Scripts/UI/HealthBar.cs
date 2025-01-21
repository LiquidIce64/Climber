using Character;
using UnityEngine;

namespace UI
{
    public class HealthBar : BaseBar
    {
        [SerializeField] Player player;

        protected new void Start()
        {
            base.Start();
            maxValue = player.MaxHealth;
        }

        protected new void Update()
        {
            value = player.Health;
            base.Update();
        }
    }
}
