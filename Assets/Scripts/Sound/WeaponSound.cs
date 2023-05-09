using Misc;
using UnityEngine;
using Combat.Weapons;

namespace Sound
{
    [RequireComponent(typeof(Weapon))]
    public class WeaponSound : MonoBehaviour
    {
        [Header("Requirements")] [SerializeField]
        private AudioClip _shotgunSound;

        [SerializeField] private AudioSource _audioSource;

        private Weapon _weapon;

        private void Start()
        {
            _weapon = GetComponent<Weapon>();
        }

        private void Update()
        {
            if (_weapon.IsUsing)
            {
                PlayShotgunSound();
                _weapon.IsUsing = false;
            }
        }

        private void PlayShotgunSound()
        {
            _audioSource.clip = _shotgunSound;
            _audioSource.Play();
        }
    }
}