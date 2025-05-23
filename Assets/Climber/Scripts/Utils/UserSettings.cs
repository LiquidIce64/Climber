using UnityEngine;

namespace Utils
{
    public static class UserSettings
    {
        private static float GetFloat(ref float param, string key, float defaultValue)
        {
            if (param == -1f)
                param = PlayerPrefs.GetFloat(key, defaultValue);
            return param;
        }

        private static void SetFloat(ref float param, string key, float value)
        {
            param = value;
            PlayerPrefs.SetFloat(key, value);
        }

        public static void Save() => PlayerPrefs.Save();

        private static float _sensitivity = -1f;
        public static float Sensitivity
        {
            get => GetFloat(ref _sensitivity, "Sensitivity", 1f);
            set => SetFloat(ref _sensitivity, "Sensitivity", value);
        }

        private static float _volume = -1f;
        public static float Volume
        {
            get => GetFloat(ref _volume, "Volume", 0.25f);
            set => SetFloat(ref _volume, "Volume", value);
        }
    }
}