using System.Collections;
using System.Collections.Generic;
using Combat.Projectiles;
using Events;
using Models;
using NaughtyAttributes;
using UnityEngine;

namespace Combat.Weapons
{
    public class DualGun : BulletGun<Bullet>
    {
        [SerializeField] [BoxGroup(Group.Settings)] [Min(0)]
        private float _bulletsSpread = 0.5f;

        [SerializeField] [BoxGroup(Group.Settings)] [Min(0)]
        private int _bulletsPerShot = 6;

        [SerializeField] [BoxGroup(Group.Settings)] [Min(0)]
        private float _timeBetweenBullets = 0.25f;

        [SerializeField] [BoxGroup(Group.Events)] [Required]
        private GameEvent _onPartialUse;

        private const bool FirstShotFromRight = true;
        public float BulletsSpread => _bulletsSpread;
        public bool ShotFromRight { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            
            ShotFromRight = FirstShotFromRight;
        }

        protected override void Use(IEnumerator<Quaternion> aimRotations)
        {
            StartCoroutine(StartShooting(aimRotations));
        }

        private IEnumerator StartShooting(IEnumerator<Quaternion> aimRotations)
        {
            CanBeUsed = false;

            for (int i = 0; i < _bulletsPerShot; i++)
            {
                Vector3 spawnPosition = _projectileSpawnPoint.position + // Position
                                        transform.right * (ShotFromRight ? _bulletsSpread : -_bulletsSpread); // Shift

                if (!aimRotations.MoveNext())
                {
                    break;
                }

                Bullet bullet = Instantiate(_projectile, spawnPosition, aimRotations.Current);
                bullet.Init(_damage, _hitLayers, Owner, _speed, _maxDistance);
                _onPartialUse.Raise(this, null);

                ShotFromRight = !ShotFromRight;
                yield return new WaitForSeconds(_timeBetweenBullets);
            }

            ShotFromRight = FirstShotFromRight;
            CanBeUsed = true;
        }
    }
}