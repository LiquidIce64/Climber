using UnityEngine;
using UnityEngine.Audio;
using Utils;

namespace UI
{
    public class VolumeSlider : BaseSlider
    {
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private AudioListener listener;

        protected override float GetValue() => UserSettings.Volume;

        protected override void SetValue(float value)
        {
            listener.enabled = value > 0f;
            mixer.SetFloat("Volume", Mathf.Log10(value) * 20);
            UserSettings.Volume = value;
            UserSettings.Save();
        }
    }
}
