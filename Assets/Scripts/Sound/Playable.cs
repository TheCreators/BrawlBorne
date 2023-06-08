using Models;
using NaughtyAttributes;
using UnityEngine;

namespace Sound
{
    public abstract class Playable : MonoBehaviour
    {
        [SerializeField] [BoxGroup(Group.Settings)]
        private bool _randomPitch = false;
        
        [SerializeField] [BoxGroup(Group.Settings)] [ShowIf(nameof(_randomPitch))] [MinMaxSlider(0.5f, 1.5f)]
        private Vector2 _pitchRange = new(0.9f, 1.1f);
        
        protected float Pitch => _randomPitch ? Random.Range(_pitchRange.x, _pitchRange.y) : 1f;

        public abstract void PlaySound(Component component, object data);
    }
}