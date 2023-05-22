using Misc;
using NaughtyAttributes;
using UnityEngine;

namespace Sound
{
    public class WeaponSound : MonoBehaviour
    {
        [SerializeField] [Required]
        private AudioClip _sound;

        private AudioSource _audioSource;

        private void OnValidate()
        {
            this.CheckIfNull(_sound);
        }

        private void Awake()
        {
            _audioSource = this.GetComponentInParentWithNullCheck<AudioSource>();
        }

        public void PlaySound(Component component, object data)
        {
            if (component.gameObject != gameObject) return;

            _audioSource.PlayOneShot(_sound);
        }
    }
}