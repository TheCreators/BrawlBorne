using Misc;
using UnityEngine;

namespace Sound
{
    public class WeaponSound : MonoBehaviour
    {
        [Header("Requirements")]
        [SerializeField] private AudioClip _sound;
        [SerializeField] private AudioSource _audioSource;
        
        private void OnValidate()
        {
            this.CheckIfNull(_sound);
            this.CheckIfNull(_audioSource);
        }

        public void PlaySound(Component component, object data)
        {
            if (component.gameObject != gameObject) return;
            
            _audioSource.PlayOneShot(_sound);
        }
    }
}