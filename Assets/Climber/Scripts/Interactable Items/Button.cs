using System.Collections;
using UnityEngine;

namespace Interactables
{
    public class Button : Lever
    {
        [SerializeField] protected float timerDuration = 3f;
        protected IEnumerator _timer;
        protected bool _pressed = false;

        public new bool CanInteract => _canInteract && !_pressed;

        protected IEnumerator ButtonTimer()
        {
            yield return new WaitForSeconds(timerDuration);
            _connector.Disable();
        }

        override protected void Enabled()
        {
            _pressed = true;

            if (_timer != null) StopCoroutine(_timer);
            _timer = ButtonTimer();
            StartCoroutine(_timer);
        }

        override protected void Disabled()
        {
            _pressed = false;

            if (_timer != null) StopCoroutine(_timer);
            _timer = null;
        }
    }
}