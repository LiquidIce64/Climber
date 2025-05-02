using System.Collections;
using UnityEngine;

namespace Interactables
{
    public class Button : BaseToggleable, IInteractable
    {
        [SerializeField] protected float _energyCost = 0f;
        [SerializeField] protected float timerDuration = 3f;
        protected IEnumerator _timer;
        protected bool _pressed = false;

        public float EnergyCost => _energyCost;
        public bool CanInteract => !_pressed;

        public void OnInteract() => _connector.Toggle();

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