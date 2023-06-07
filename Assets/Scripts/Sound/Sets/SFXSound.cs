using Environment;
using Heroes.Player;
using Misc;
using Models;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SfxSound : MonoSingleton<SfxSound>
    {
        [SerializeField] [BoxGroup(Group.Settings)]
        private AudioClipWithSource _ultimateCharged;
        
        [SerializeField] [BoxGroup(Group.Settings)]
        private AudioClipWithSource _heroDied;
        
        [SerializeField] [BoxGroup(Group.Settings)]
        private AudioClipWithSource _healthIncreased;
        
        [SerializeField] [BoxGroup(Group.Settings)]
        private AudioClipWithSource _boostGained;
        
        private AudioSource _audioSource;
        
        protected override void Init()
        {
            _audioSource = this.GetComponentWithNullCheck<AudioSource>();
        }
        
        public void PlayUltimateCharged(Component component, object data)
        {
            Player player = component.GetComponentInParent<Player>();
            if (player is null) return;
            
            _ultimateCharged.PlayOneShot();
        }
        
        public void PlayHeroKilled(Component component, object data)
        {
            _heroDied.Play();
        }
        
        public void PlayHealthIncreased(Component component, object data)
        {
            if (component.TryGetComponent(out Player _) is false || data is not Player) return;
            
            _healthIncreased.PlayOneShot();
        }
        
        public void PlayBoostGained(Component component, object data)
        {
            if (component.TryGetComponent(out Boost _) is false || data is not Player) return;
            
            _boostGained.PlayOneShot();
        }
    }
}