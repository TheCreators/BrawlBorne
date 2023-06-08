using Misc;
using Models;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Audio;

namespace Sound
{
    public class MixerManager : MonoSingleton<MixerManager>
    {
        [SerializeField] [BoxGroup(Group.Settings)] [Required]
        private AudioMixer _audioMixer;

        [SerializeField] [BoxGroup(Group.Settings)]
        private MixerGroupManager _master;
        
        [SerializeField] [BoxGroup(Group.Settings)]
        private MixerGroupManager _music;
        
        public MixerGroupManager Master => _master;
        public MixerGroupManager Music => _music;
        
        private void OnValidate()
        {
            this.CheckIfNull(_audioMixer);
            this.CheckIfNull(_master);
            this.CheckIfNull(_music);
        }

        protected override void Init()
        {
            _master.Init(_audioMixer);
            _music.Init(_audioMixer);
        }
    }
}