using UnityEngine;

namespace Sound
{
    public class WeaponSound : MonoBehaviour
    {
        [Header("Requirements")]
        [SerializeField] private AudioClip _sound;
        [SerializeField] private AudioSource _audioSource;
        
        private readonly string _settingsKey = "volume";

        public void PlaySound()
        {
            if (PlayerPrefs.HasKey(_settingsKey))
            {
                _audioSource.volume = PlayerPrefs.GetFloat(_settingsKey);
            }

            _audioSource.PlayOneShot(_sound);
        }
    }
}