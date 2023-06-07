using System;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

namespace Sound
{
    [Serializable]
    public class AudioClipWithSource
    {
        [SerializeField] [AllowNesting] [CanBeNull]
        private AudioClip _clip;
        
        [SerializeField] [AllowNesting] [CanBeNull]
        private AudioSource _source;

        public void PlayOneShot()
        {
            if (_clip is null || _source is null) return;
            
            _source.PlayOneShot(_clip);
        }
        
        public void Play()
        {
            if (_clip is null || _source is null) return;
            
            _source.clip = _clip;
            _source.Play();
        }
        
        public void SetPitch(float pitch)
        {
            if (_source is null) return;
            
            _source.pitch = pitch;
        }
    }
}