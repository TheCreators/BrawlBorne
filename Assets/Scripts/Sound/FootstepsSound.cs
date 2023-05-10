using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class FootstepsSound : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> _footstepSounds;
        [SerializeField] private AudioClip _jumpSound;
        [SerializeField] private AudioClip _landingSound;
        
        [Header("Footsteps Settings")]
        [SerializeField] private float _footstepWalkDelay = 0.35f;
        [SerializeField] private float _footstepSneakDelay = 0.5f;
        
        private AudioSource _audioSource;
        private Coroutine _footstepCoroutine;
        private float _footstepDelay;
        private bool _isGrounded = true;
        private int _previousFootstepIndex = -1;

        private const string SettingsKey = "volume";

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _footstepDelay = _footstepWalkDelay;
        }

        public void Start()
        {
            if (PlayerPrefs.HasKey(SettingsKey))
            {
                _audioSource.volume = PlayerPrefs.GetFloat(SettingsKey);
            }
        }

        public void SetIsGroundedToFalse(Component component, object data)
        {
            if (component.gameObject != gameObject) return;
            
            _isGrounded = false;
        }
        
        public void PlayFootstepsSound(Component component, object data)
        {
            if (component.gameObject != gameObject) return;
            
            _footstepCoroutine ??= StartCoroutine(FootstepCoroutine());
        }
        
        public void StopFootstepsSound(Component component, object data)
        {
            if (component.gameObject != gameObject) return;
            
            if (_footstepCoroutine == null) return;
            StopCoroutine(_footstepCoroutine);
            _footstepCoroutine = null;
        }
        
        public void SetFootstepsDelayToSneaking(Component component, object data)
        {
            if (component.gameObject != gameObject) return;
            
            _footstepDelay = _footstepSneakDelay;
        }

        public void SetFootstepsDelayToWalking(Component component, object data)
        {
            if (component.gameObject != gameObject) return;
            
            _footstepDelay = _footstepWalkDelay;
        }

        public void PlayLandingSound(Component component, object data)
        {
            if (component.gameObject != gameObject) return;
            
            _isGrounded = true;
            _audioSource.PlayOneShot(_landingSound);
        }

        public void PlayJumpSound(Component component, object data)
        {
            if (component.gameObject != gameObject) return;
            
            _isGrounded = false;
            _audioSource.PlayOneShot(_jumpSound);
        }
        
        private IEnumerator FootstepCoroutine()
        {
            while (true)
            {
                if (_isGrounded is false)
                {
                    yield return null;
                    continue;
                }
                
                int index = GenerateRandomFootstepIndex();
                _audioSource.PlayOneShot(_footstepSounds[index]);
                yield return new WaitForSeconds(_footstepDelay);
            }
        }

        private int GenerateRandomFootstepIndex()
        {
            int index = Random.Range(0, _footstepSounds.Count);
            while (index == _previousFootstepIndex)
            {
                index = Random.Range(0, _footstepSounds.Count);
            }
            _previousFootstepIndex = index;
            return index;
        }
    }
}
