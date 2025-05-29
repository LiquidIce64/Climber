using Character;
using UnityEngine;
using Visuals;

namespace Interactables
{
    [RequireComponent(typeof(BoxCollider))]
    public class ColorField : BaseToggleable, ITriggerItem
    {
        [SerializeField] protected EnergyColor _color;
        [SerializeField] protected GameObject _field;
        [SerializeField] protected EnergyMaterial[] _energyMaterials;
        protected Collider _collider;

        protected void OnValidate()
        {
            foreach (var mat in _energyMaterials)
                mat.UpdateMaterialInEditor(_color);
        }

        protected new void Awake()
        {
            base.Awake();
            _collider = GetComponent<Collider>();
        }

        public void TriggerAction(Player player) => player.SetClimbToolColor(_color);

        protected override void Enabled()
        {
            _field.SetActive(true);
            _collider.enabled = true;
            foreach (var mat in _energyMaterials)
                mat.UpdateMaterial(_color);
        }

        protected override void Disabled()
        {
            foreach (var mat in _energyMaterials)
                mat.UpdateMaterial(_color);
            _collider.enabled = false;
            _field.SetActive(false);
        }
    }
}
