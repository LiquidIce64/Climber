using System.Collections;
using UnityEngine;

namespace Interactables
{
    public class Button : Lever
    {
        [SerializeField] protected float timerDuration = 3f;
        protected IEnumerator _timer;

        protected IEnumerator buttonTimer()
        {
            yield return new WaitForSeconds(timerDuration);
            Disable();
        }

        override protected void _Enabled()
        {
            _toggleEvent.Invoke();
            canInteract = false;

            if (_timer != null) StopCoroutine(_timer);
            _timer = buttonTimer();
            StartCoroutine(_timer);
        }

        override protected void _Disabled()
        {
            _toggleEvent.Invoke();
            canInteract = true;

            if (_timer != null) StopCoroutine(_timer);
            _timer = null;
        }
    }
}