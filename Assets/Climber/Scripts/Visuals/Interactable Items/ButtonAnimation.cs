using UnityEngine;

namespace Visuals
{
    public class ButtonAnimation : ToggleAnimation
    {
        protected override void AnimationStep()
        {
            float pos = Mathf.SmoothStep(0, -0.045f, _currentT);
            transform.localPosition = new Vector3(0f, pos, 0f);
        }
    }
}
