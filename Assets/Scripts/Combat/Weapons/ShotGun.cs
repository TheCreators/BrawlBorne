using System.Collections.Generic;
using Combat.Projectiles;
using Misc;
using Models;
using NaughtyAttributes;
using UnityEngine;

namespace Combat.Weapons
{
    public class ShotGun : BulletGun<Bullet>
    {
        [SerializeField] [BoxGroup(Group.Settings)] [Min(0)]
        private int _bulletsPerShot = 4;

        [SerializeField] [BoxGroup(Group.Settings)] [Min(0)]
        private float _verticalSpread = 10f;

        [SerializeField] [BoxGroup(Group.Settings)] [Min(0)]
        private float _horizontalSpread = 10f;

        protected override void Use(IEnumerator<Quaternion> aimRotations)
        {
            if (!aimRotations.MoveNext())
            {
                Debug.LogError("No aim rotations");
                return;
            }
            
            CanBeUsed = false;

            List<Quaternion> rotations = GenerateRandomRotations(aimRotations.Current);

            for (int i = 0; i < _bulletsPerShot; i++)
            {
                Bullet bullet = Instantiate(_projectile, _projectileSpawnPoint.position, rotations[i]);
                bullet.Init(_damage, _hitLayers, Owner, _speed, _maxDistance);
            }

            CanBeUsed = true;
        }

        private List<Quaternion> GenerateRandomRotations(Quaternion aimRotation)
        {
            List<Quaternion> rotations = new List<Quaternion>();
            Vector3 forward = Vector3.forward;

            for (int i = 0; i < _bulletsPerShot; i++)
            {
                // Generate random rotation around X and Y axes
                float randomXRotation = Random.Range(-_verticalSpread, _verticalSpread);
                float randomYRotation = Random.Range(-_horizontalSpread, _horizontalSpread);

                // Apply random rotation to the fixed forward direction
                Quaternion randomRotation = Quaternion.Euler(randomXRotation, randomYRotation, 0);
                Vector3 randomizedDirection = randomRotation * forward;

                // Rotate the randomized direction by the rotation of _shootingDirection
                randomizedDirection = aimRotation * randomizedDirection;

                rotations.Add(Quaternion.LookRotation(randomizedDirection));
            }

            return rotations;
        }
    }
}