using Misc;
using Models;
using NaughtyAttributes;
using UnityEngine;

namespace Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class MenuSound : MonoSingleton<MenuSound>
    {
        [SerializeField] [BoxGroup(Group.Settings)] [Required]
        private AudioClip _simpleButton;
        
        [SerializeField] [BoxGroup(Group.Settings)] [Required]
        private AudioClip _backButton;
        
        [SerializeField] [BoxGroup(Group.Settings)] [Required]
        private AudioClip _playerSelected;
        
        private AudioSource _audioSource;

        protected override void Init()
        {
            _audioSource = this.GetComponentWithNullCheck<AudioSource>();
        }

        public void PlaySimpleButton()
        {
            _audioSource.PlayOneShot(_simpleButton);
        }
        
        public void PlayBackButton()
        {
            _audioSource.PlayOneShot(_backButton);
        }
        
        public void PlayPlayerSelected()
        {
            _audioSource.PlayOneShot(_playerSelected);
        }
    }
}