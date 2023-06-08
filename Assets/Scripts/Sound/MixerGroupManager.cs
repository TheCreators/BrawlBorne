using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Audio;

namespace Sound
{
    [Serializable]
    public class MixerGroupManager
    {
        [SerializeField] [AllowNesting] [ValidateInput("IsNotNull", "Volume key is required")]
        private string _volumeKey;
        
        [SerializeField] [AllowNesting] [Range(-80, 20)]
        private int _defaultVolume = 0;
        
        private AudioMixer _audioMixer;
        
        public void Init(AudioMixer audioMixer)
        {
            _audioMixer = audioMixer;
        }
        
        private bool IsNotNull(string value) => !string.IsNullOrEmpty(value);

        public float Volume
        {
            get
            {
                _audioMixer.GetFloat(_volumeKey, out var volume);
                return volume;
            }
            set
            {
                if (value < -80)
                {
                    value = -80;
                }
                else if (value > 20)
                {
                    value = 20;
                }
                
                _audioMixer.SetFloat(_volumeKey, value);
            }
        }

        public void ResetVolume()
        {
            Volume = _defaultVolume;
        }
    }
}