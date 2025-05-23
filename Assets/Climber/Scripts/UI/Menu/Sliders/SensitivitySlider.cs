using Utils;

namespace UI
{
    public class SensitivitySlider : BaseSlider
    {
        protected override float GetValue() => UserSettings.Sensitivity;

        protected override void SetValue(float value)
        {
            UserSettings.Sensitivity = value;
            UserSettings.Save();
        }
    }
}
