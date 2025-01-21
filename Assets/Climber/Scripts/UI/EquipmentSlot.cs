using Equipment;
using UnityEngine;

namespace UI
{
    public class EquipmentSlot : MonoBehaviour
    {
        [SerializeField] protected BaseEquipment equipment;
        [SerializeField] protected RectTransform cooldownPanel;
        protected RectTransform rectTransform;

        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        void Update()
        {
            cooldownPanel.offsetMax = new Vector2
            (
                cooldownPanel.offsetMax.x,
                (equipment.CooldownFraction - 1f) * rectTransform.rect.height
            );
        }
    }
}
