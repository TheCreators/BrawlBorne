using Heroes.Player;
using JetBrains.Annotations;
using Misc;
using Models;
using NaughtyAttributes;
using UnityEngine;

namespace Sound.Sets
{
    [RequireComponent(typeof(AudioSource))]
    public class WeaponSound : MonoBehaviour
    {
        [SerializeField] [BoxGroup(Group.Sounds)]
        private AudioClipWithSource _used;
        
        [SerializeField] [BoxGroup(Group.Sounds)]
        private AudioClipWithSource _reload;
        
        [SerializeField] [BoxGroup(Group.Sounds)]
        private AudioClipWithSource _outOfCells;
        
        [SerializeField] [BoxGroup(Group.Sounds)]
        private AudioClipWithSource _dryUsed;
        
        [SerializeField] [BoxGroup(Group.Settings)]
        private bool _usedRandomPitch = false;
        
        [SerializeField] [BoxGroup(Group.Settings)] [ShowIf(nameof(_usedRandomPitch))] [MinMaxSlider(0.5f, 1.5f)]
        private Vector2 _usedPitchRange = new(0.9f, 1.1f);

        public void PlayUseSound(Component component, object data)
        {
            if (component.gameObject != gameObject) return;

            if (_usedRandomPitch)
            {
                _used.SetPitch(Random.Range(_usedPitchRange.x, _usedPitchRange.y));
            }
            
            _used.PlayOneShot();
        }
        
        public void PlayReloadSound(Component component, object data)
        {
            if (component.gameObject != gameObject) return;
            
            Player player = component.GetComponentInParent<Player>();
            if (player is null) return;
            
            _reload.PlayOneShot();
        }
        
        public void PlayOutOfCellsSound(Component component, object data)
        {
            if (component.gameObject != gameObject) return;
            
            Player player = component.GetComponentInParent<Player>();
            if (player is null) return;
            
            _outOfCells.PlayOneShot();
        }
        
        public void PlayDryUsedSound(Component component, object data)
        {
            if (component.gameObject != gameObject) return;
            
            Player player = component.GetComponentInParent<Player>();
            if (player is null) return;
            
            _dryUsed.PlayOneShot();
        }
    }
}