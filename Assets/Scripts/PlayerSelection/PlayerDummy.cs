using System;
using Heroes.Player;
using JetBrains.Annotations;
using Models;
using NaughtyAttributes;
using Sound;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerSelection
{
    [Serializable]
    public class PlayerDummy
    {
        [SerializeField] [BoxGroup(Group.Prefabs)] [Required]
        private Player _playerPrefab;

        [SerializeField] [BoxGroup(Group.Settings)] [CanBeNull]
        private AudioClip _musicTheme;

        private Player _playerInstance;
        private AudioSource _playerInstanceAudioSource;
        
        public string Name => _playerPrefab.name;
        
        public Player PlayerPrefab => _playerPrefab;

        public void Instantiate(Vector3 position, Quaternion rotation, Transform parent)
        {
            _playerInstance = UnityEngine.Object.Instantiate(_playerPrefab, position, rotation, parent);
            SetupPlayer();
        }
        
        public void PlayMusicTheme()
        {
            if (_playerInstanceAudioSource == null) return;
            _playerInstanceAudioSource.Play();
        }
        
        public void StopMusicTheme()
        {
            if (_playerInstanceAudioSource == null) return;
            _playerInstanceAudioSource.Stop();
        }

        private void SetupPlayer()
        {
            if (_playerInstance == null) return;
            
            Camera playerCamera = _playerInstance.GetComponentInChildren<Camera>();
            if (playerCamera != null)
            {
                playerCamera.enabled = false;
                if (playerCamera.TryGetComponent(out MouseRotation mouseRotation)) mouseRotation.enabled = false;
                if (playerCamera.TryGetComponent(out AudioListener audioListener)) audioListener.enabled = false;
            }

            if (_playerInstance.TryGetComponent(out PlayerInput playerInput)) playerInput.enabled = false;
            if (_playerInstance.TryGetComponent(out Rigidbody playerRigidbody)) playerRigidbody.interpolation = RigidbodyInterpolation.None;
            if (_playerInstance.TryGetComponent(out AudioSource playerAudioSource)) playerAudioSource.mute = true;

            AudioSource audioSource = _playerInstance.gameObject.AddComponent<AudioSource>();
            audioSource.clip = _musicTheme;
            audioSource.loop = true;
            _playerInstanceAudioSource = audioSource;
        }
    }
}