using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BaseBar : MonoBehaviour
    {
        [SerializeField] RectTransform bar;
        [SerializeField] TextMeshProUGUI text;
        [SerializeField] Color barColor;
        protected float maxValue = 0f;
        protected float value = 0f;
        protected RectTransform rectTransform;

        protected void OnValidate()
        {
            bar.GetComponent<Image>().color = barColor;
        }

        protected void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        protected void Update()
        {
            bar.offsetMax = new Vector2
            (
                (value / maxValue - 1f) * rectTransform.rect.width,
                bar.offsetMax.y
            );
            text.text = Mathf.Ceil(value) + " / " + Mathf.Ceil(maxValue);
        }
    }
}
