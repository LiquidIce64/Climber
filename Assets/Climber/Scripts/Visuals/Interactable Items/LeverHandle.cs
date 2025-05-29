using UnityEngine;

namespace Visuals
{
    public class LeverHandle : ToggleAnimation
    {
        protected override void AnimationStep()
        {
            float angle = Mathf.SmoothStep(-45f, 45f, _currentT);
            transform.localRotation = Quaternion.Euler(angle, 0f, -90f);
        }
    }
}
