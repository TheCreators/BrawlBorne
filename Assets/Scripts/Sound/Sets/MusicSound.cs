using System.Collections.Generic;
using Misc;
using UnityEngine;

namespace Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicSound : MonoSingleton<MusicSound>
    {
        [SerializeField]
        private List<AudioClip> _musicClips;
        
        private AudioSource _audioSource;
        
        protected override void Init()
        {
            _audioSource = this.GetComponentWithNullCheck<AudioSource>();
        }

        private void Start()
        {
            PlayRandomMusic();
        }

        private void PlayRandomMusic()
        {
            _audioSource.clip = _musicClips[UnityEngine.Random.Range(0, _musicClips.Count)];
            _audioSource.Play();
        }
    }
}