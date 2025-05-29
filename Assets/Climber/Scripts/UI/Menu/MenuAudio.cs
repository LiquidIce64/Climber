using UnityEngine;
using UnityEngine.Audio;

namespace UI
{
    [RequireComponent(typeof(AudioSource))]
    public class MenuAudio : MonoBehaviour
    {
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(AudioResource sound)
        {
            audioSource.resource = sound;
            audioSource.Play();
        }
    }
}
