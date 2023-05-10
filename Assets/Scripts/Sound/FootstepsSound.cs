using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _footstepDelay = _footstepWalkDelay;
        }
        
        public void SetIsGroundedToFalse()
        {
            _isGrounded = false;
        }
        
        public void PlayFootstepsSound()
        {
            _footstepCoroutine ??= StartCoroutine(FootstepCoroutine());
        }
        
        public void StopFootstepsSound()
        {
            if (_footstepCoroutine == null) return;
            StopCoroutine(_footstepCoroutine);
            _footstepCoroutine = null;
        }
        
        public void SetFootstepsDelayToSneaking()
        {
            _footstepDelay = _footstepSneakDelay;
        }

        public void SetFootstepsDelayToWalking()
        {
            _footstepDelay = _footstepWalkDelay;
        }

        public void PlayLandingSound()
        {
            _isGrounded = true;
            _audioSource.PlayOneShot(_landingSound);
        }

        public void PlayJumpSound()
        {
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
