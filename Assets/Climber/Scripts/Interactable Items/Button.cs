using System.Collections;
using UnityEngine;

namespace Interactables
{
    public class Button : Lever
    {
        [SerializeField] protected float timerDuration = 3f;
        [SerializeField] protected AudioSource timerAudio;
        protected IEnumerator _timer;
        protected bool _pressed = false;

        override public bool CanInteract => !_pressed;

        protected IEnumerator ButtonTimer()
        {
            yield return new WaitForSeconds(timerDuration);
            _connector.Disable();
        }

        override protected void Start()
        {
            base.Start();
            if (Toggled)
            {
                timerAudio.Play();
                _pressed = true;
                _timer = ButtonTimer();
                StartCoroutine(_timer);
            }
        }

        override protected void Enabled()
        {
            base.Enabled();
            timerAudio.Play();
            _pressed = true;
            if (_timer != null) StopCoroutine(_timer);
            _timer = ButtonTimer();
            StartCoroutine(_timer);
        }

        override protected void Disabled()
        {
            base.Disabled();
            timerAudio.Stop();
            _pressed = false;
            if (_timer != null) StopCoroutine(_timer);
            _timer = null;
        }
    }
}
