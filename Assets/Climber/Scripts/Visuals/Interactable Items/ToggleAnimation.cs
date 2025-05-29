using UnityEngine;

namespace Visuals
{
    abstract public class ToggleAnimation : MonoBehaviour
    {
        [SerializeField] protected float _animationLength = 1f;
        protected float _targetT = 0f;
        protected float _currentT = 0f;

        public void ForceToggle(bool toggled)
        {
            _currentT = _targetT = toggled ? 1f : 0f;
            AnimationStep();
        }

        public void UpdateToggle(bool toggled)
        {
            _targetT = toggled ? 1f : 0f;
            enabled = true;
        }

        void Update()
        {
            _currentT = Mathf.MoveTowards(_currentT, _targetT, Time.deltaTime / _animationLength);
            AnimationStep();
            enabled = _currentT != _targetT;
        }

        abstract protected void AnimationStep();
    }
}
