using Misc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(GroundChecker))]
    public class FootstepsSound : MonoBehaviour
    {
        [Header("Requirements")]
        [SerializeField] private AudioClip _footstepsSound;
        [SerializeField] private AudioClip _jumpSound;
        [SerializeField] private AudioClip _landingSound;
        [SerializeField] private AudioSource _audioSource;

        private Rigidbody _character;
        private GroundChecker _groundChecker;

        private bool wasGrounded = false;

        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _character = GetComponent<Rigidbody>();
            _groundChecker = GetComponent<GroundChecker>();
        }

        void FixedUpdate()
        {

            if (_groundChecker.IsGrounded)
            {
                if (_character.velocity.sqrMagnitude > 0f)          // Character is moving. sqrMagnitude is in less need for resources.
                {
                    if (!_audioSource.isPlaying)
                    {
                        _audioSource.clip = _footstepsSound;
                        _audioSource.Play();
                    }
                }
                else
                {
                    if (_audioSource.clip != _jumpSound && _audioSource.clip != _landingSound)
                    {
                        if (_audioSource.isPlaying)
                        {
                            _audioSource.Stop();
                        }
                    }
                }

                if (_groundChecker.IsGrounded != wasGrounded)
                {
                    _audioSource.Stop();
                    _audioSource.clip = _landingSound;
                    _audioSource.Play();

                }

                wasGrounded = true;
            }
            else
            {
                if (_audioSource.clip != _jumpSound && _audioSource.clip != _landingSound)
                {
                    if (_audioSource.isPlaying)
                    {
                        _audioSource.Stop();
                    }
                }

                if (_groundChecker.IsGrounded != wasGrounded)
                {
                    _audioSource.Stop();
                    _audioSource.clip = _jumpSound;
                    _audioSource.Play();
                }

                wasGrounded = false;
            }
        }
    }
}
