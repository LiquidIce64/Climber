using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Slider))]
    public abstract class BaseSlider : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI valueText;
        private Slider slider;

        private void Awake()
        {
            slider = GetComponent<Slider>();
            slider.value = GetValue();
        }

        public void OnChangeSlider(float value)
        {
            valueText.text = value.ToString("N2");
            SetValue(value);
        }

        protected abstract float GetValue();
        protected abstract void SetValue(float value);
    }
}
