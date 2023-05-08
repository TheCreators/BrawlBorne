using System.Collections.Generic;
using Combat.Projectiles;
using UnityEngine;

namespace Combat.Weapons
{
    public class ShotGun : Gun<Bullet>
    {
        [Header("Settings")]
        [SerializeField, Min(0)] private int _bulletsPerShot = 4;

        [Header("Spread Settings")]
        [SerializeField, Min(0)] private float _verticalSpread = 10f;
        [SerializeField, Min(0)] private float _horizontalSpread = 10f;

        protected override void Shoot()
        {
            CanBeUsed = false;

            List<Quaternion> rotations = GenerateRandomRotations(_bulletsPerShot);
            
            Vector3 spawnPosition = _shootingDirection.position + // Position
                                    _shootingDirection.forward * _bulletSpawnDistance + // Distance from camera
                                    _shootingDirection.up * _bulletSpawnHeight; // Height from camera
            
            for (int i = 0; i < _bulletsPerShot; i++)
            {
                Instantiate(_projectile, spawnPosition, rotations[i]);
            }

            CanBeUsed = true;
        }

        private List<Quaternion> GenerateRandomRotations(int count)
        {
            List<Quaternion> rotations = new List<Quaternion>();
            Vector3 forward = Vector3.forward;

            for (int i = 0; i < count; i++)
            {
                // Generate random rotation around X and Y axes
                float randomXRotation = Random.Range(-_verticalSpread, _verticalSpread);
                float randomYRotation = Random.Range(-_horizontalSpread, _horizontalSpread);

                // Apply random rotation to the fixed forward direction
                Quaternion randomRotation = Quaternion.Euler(randomXRotation, randomYRotation, 0);
                Vector3 randomizedDirection = randomRotation * forward;

                // Rotate the randomized direction by the rotation of _shootingDirection
                randomizedDirection = _shootingDirection.rotation * randomizedDirection;

                rotations.Add(Quaternion.LookRotation(randomizedDirection));
            }

            return rotations;
        }

    }
}