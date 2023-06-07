using System;
using System.Collections.Generic;
using Heroes.Player;
using JetBrains.Annotations;
using Misc;
using Models;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sound.Sets
{
    [RequireComponent(typeof(AudioSource))]
    public class VoiceSound : MonoBehaviour
    {
        [SerializeField] [BoxGroup(Group.Sounds)]
        private List<AudioClip> _startSounds;

        [SerializeField] [BoxGroup(Group.Sounds)]
        private List<AudioClip> _killSounds;

        [SerializeField] [BoxGroup(Group.Sounds)]
        private List<AudioClip> _hurtSounds;

        [SerializeField] [BoxGroup(Group.Sounds)]
        private List<AudioClip> _deathSounds;

        [SerializeField] [BoxGroup(Group.Sounds)]
        private List<AudioClip> _attackSounds;

        [SerializeField] [BoxGroup(Group.Sounds)]
        private List<AudioClip> _ultimateSounds;

        [SerializeField] [BoxGroup(Group.Settings)] [Required]
        private AudioSource _audioSource;

        [SerializeField] [BoxGroup(Group.Settings)] [Range(0, 20)]
        private float _timeBetweenHurtSounds = 10f;

        [SerializeField] [BoxGroup(Group.Settings)] [Range(0, 20)]
        private float _timeBetweenAttackSounds = 10f;

        [SerializeField] [BoxGroup(Group.Settings)] [Range(0, 1)]
        private float _ultimateSoundChance = 0.5f;

        private float _lastHurtSoundTime;
        private float _lastAttackSoundTime;
        private List<AudioClip> _selectedSounds;
        
        public AudioSource AudioSource => _audioSource;

        private void OnValidate()
        {
            this.CheckIfNull(_audioSource);
        }

        private void Awake()
        {
            _selectedSounds = new List<AudioClip>();
            _selectedSounds.AddRange(_startSounds);
            _selectedSounds.AddRange(_killSounds);
            _selectedSounds.AddRange(_ultimateSounds);
        }

        private void Start()
        {
            PlayStartSound(this, null);
        }

        public void PlayStartSound(Component component, object data)
        {
            if (component.TryGetComponent(out Player _) is false) return;

            AudioClip sound = GetRandomSound(_startSounds);
            if (sound is null) return;
            _audioSource.PlayOneShot(sound);
        }

        public void PlayKillSound(Component component, object data)
        {
            if (((Component) data).TryGetComponent(out Player _) is false) return;
            
            AudioClip sound = GetRandomSound(_killSounds);
            if (sound is null) return;
            _audioSource.PlayOneShot(sound);
        }

        public void PlayHurtSound(Component component, object data)
        {
            if (Time.time - _lastHurtSoundTime < _timeBetweenHurtSounds) return;
            
            if (component.TryGetComponent(out Player _) is false) return;

            AudioClip sound = GetRandomSound(_hurtSounds);
            if (sound is null) return;
            _audioSource.PlayOneShot(sound);

            _lastHurtSoundTime = Time.time;
        }

        public void PlayDeathSound(Component component, object data)
        {
            if (component.TryGetComponent(out Player _) is false) return;

            AudioClip sound = GetRandomSound(_deathSounds);
            if (sound is null) return;
            _audioSource.PlayOneShot(sound);
        }

        public void PlayAttackSound(Component component, object data)
        {
            Player player = component.GetComponentInParent<Player>();
            if (player is null) return;

            if (Time.time - _lastAttackSoundTime < _timeBetweenAttackSounds) return;

            AudioClip sound = GetRandomSound(_attackSounds);
            if (sound is null) return;
            _audioSource.PlayOneShot(sound);

            _lastAttackSoundTime = Time.time;
        }

        public void PlayUltimateSound(Component component, object data)
        {
            Player player = component.GetComponentInParent<Player>();
            if (player is null) return;

            if (Random.Range(0f, 1f) > _ultimateSoundChance) return;

            AudioClip sound = GetRandomSound(_ultimateSounds);
            if (sound is null) return;
            _audioSource.PlayOneShot(sound);
        }

        public void PlaySelectedSound(Component component, object data)
        {
            if (component.TryGetComponent(out Player _) is false) return;
            
            AudioClip sound = GetRandomSound(_selectedSounds);
            if (sound is null) return;
            _audioSource.PlayOneShot(sound);
        }

        [CanBeNull]
        private static AudioClip GetRandomSound(IReadOnlyList<AudioClip> sounds)
        {
            if (sounds.Count == 0) return null;
            return sounds[Random.Range(0, sounds.Count)];
        }
    }
}