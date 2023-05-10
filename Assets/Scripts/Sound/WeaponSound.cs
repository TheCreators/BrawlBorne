using UnityEngine;

namespace Sound
{
    public class WeaponSound : MonoBehaviour
    {
        [Header("Requirements")]
        [SerializeField] private AudioClip _sound;
        [SerializeField] private AudioSource _audioSource;

        public void PlaySound()
        {
            _audioSource.PlayOneShot(_sound);
        }
    }
}