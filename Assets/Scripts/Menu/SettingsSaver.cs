using System;
using Misc;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    [RequireComponent(typeof(Slider))]
    public class SettingsSaver : MonoBehaviour
    {
        [SerializeField] private string _settingsKey;
        
        private void OnValidate()
        {
            this.CheckIfNull(_settingsKey);
        }

        private void Start()
        {
            if (PlayerPrefs.HasKey(_settingsKey))
            {
                GetComponent<Slider>().value = PlayerPrefs.GetFloat(_settingsKey);
            }
        }

        public void SetSettingValue(float value)
        {
            PlayerPrefs.SetFloat(_settingsKey, value);
            PlayerPrefs.Save();
        }
    }
}
