using System;
using UnityEngine;

namespace Menu
{
    public class SettingsSaver : MonoBehaviour
    {
        private void Start()
        {
            PlayerPrefs.SetFloat("volume", 1);
            PlayerPrefs.Save();
        }

        public void SetVolumeSetting(float value)
        {
            PlayerPrefs.SetFloat("volume", value);
            PlayerPrefs.Save();
        }
    }
}
