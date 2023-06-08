using Misc;
using Models;
using NaughtyAttributes;
using UnityEngine;

namespace Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class Sound : Playable
    {
        [SerializeField] [BoxGroup(Group.Sounds)]
        private AudioClip _sound;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = this.GetComponentWithNullCheck<AudioSource>();
        }

        public override void PlaySound(Component component, object data)
        {
            if (component.gameObject != gameObject) return;
            
            _audioSource.pitch = Pitch;
            _audioSource.PlayOneShot(_sound);
        }
    }
}