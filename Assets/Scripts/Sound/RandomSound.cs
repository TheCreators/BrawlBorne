using System.Collections.Generic;
using Models;
using NaughtyAttributes;
using UnityEngine;

namespace Sound
{
    public class RandomSound : Playable
    {
        [SerializeField] [BoxGroup(Group.Sounds)]
        private List<AudioClipWithSource> _sounds;

        public override void PlaySound(Component component, object data)
        {
            if (component.gameObject != gameObject) return;

            AudioClipWithSource sound = _sounds[Random.Range(0, _sounds.Count)];
            sound.SetPitch(Pitch);
            sound.PlayOneShot();
        }

    }
}