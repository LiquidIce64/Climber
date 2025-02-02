using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Button : MonoBehaviour
    {
        [SerializeField] protected Color hoveredColor;
        protected Color color;
        protected Image image;

        void Awake()
        {
            image = GetComponent<Image>();
            color = image.color;
        }

        public void HoverEnterEvent()
        {
            image.color = hoveredColor;
        }

        public void HoverLeaveEvent()
        {
            image.color = color;
        }
    }
}