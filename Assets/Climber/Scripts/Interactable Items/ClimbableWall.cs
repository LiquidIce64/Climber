using UnityEngine;
using Visuals;

namespace Interactables
{
    public class ClimbableWall : BaseToggleable
    {
        [SerializeField] protected EnergyColor _color = EnergyColor.Blue;
        [SerializeField] protected EnergyMaterial _indicator;

        public EnergyColor Color {
            get => _color;
            set {
                _color = value;
                _indicator.UpdateMaterial(_color);
            }
        }

        protected void OnValidate()
        {
            _indicator.UpdateMaterialInEditor(_color);
        }

        protected override void Enabled()
        {
            _indicator.UpdateMaterial(_color);
        }

        protected override void Disabled()
        {
            _indicator.UpdateMaterial(EnergyColor.Off);
        }
    }
}
