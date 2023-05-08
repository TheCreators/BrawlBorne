using Misc;
using UnityEngine;

namespace Sound
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(GroundChecker))]
    public class FootstepsSound : MonoBehaviour
    {
        [Header("Requirements")] [SerializeField]
        private AudioClip _footstepsSound;

        [SerializeField] private AudioClip _jumpSound;
        [SerializeField] private AudioClip _landingSound;
        [SerializeField] private AudioSource _audioSource;
        private Rigidbody _character;
        private GroundChecker _groundChecker;
        private bool _wasGrounded;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _character = GetComponent<Rigidbody>();
            _groundChecker = GetComponent<GroundChecker>();
        }

        private void Update()
        {
            bool isGrounded = _groundChecker.IsGrounded;
            bool isMoving = _character.velocity.sqrMagnitude > 0f;
            bool isPlaying = _audioSource.isPlaying;

            if (isGrounded)
            {
                HandleGroundedState(isMoving, isPlaying);
            }
            else
            {
                HandleAirborneState(isPlaying);
            }
        }

        private void HandleGroundedState(bool isMoving, bool isPlaying)
        {
            if (isMoving)
            {
                PlayFootstepsSound(isPlaying);
            }
            else
            {
                StopNonJumpOrLandingSounds(isPlaying);
            }

            if (_groundChecker.IsGrounded != _wasGrounded)
            {
                PlayLandingSound();
            }

            _wasGrounded = true;
        }

        private void HandleAirborneState(bool isPlaying)
        {
            StopNonJumpOrLandingSounds(isPlaying);

            if (_groundChecker.IsGrounded != _wasGrounded)
            {
                PlayJumpSound();
            }

            _wasGrounded = false;
        }

        private void PlayFootstepsSound(bool isPlaying)
        {
            if (isPlaying) return;
            _audioSource.clip = _footstepsSound;
            _audioSource.Play();
        }

        private void StopNonJumpOrLandingSounds(bool isPlaying)
        {
            if (_audioSource.clip != _jumpSound && _audioSource.clip != _landingSound && isPlaying)
            {
                _audioSource.Stop();
            }
        }

        private void PlayLandingSound()
        {
            _audioSource.Stop();
            _audioSource.clip = _landingSound;
            _audioSource.Play();
        }

        private void PlayJumpSound()
        {
            _audioSource.Stop();
            _audioSource.clip = _jumpSound;
            _audioSource.Play();
        }
    }
}