using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private TextMeshProUGUI text;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = PlayerPrefs.GetFloat("Volume", 0.25f);
    }

    public void OnChangeSlider(float value)
    {
        text.text = value.ToString("N2");
        mixer.SetFloat("Volume", Mathf.Log10(value) * 20);
        
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }
}
