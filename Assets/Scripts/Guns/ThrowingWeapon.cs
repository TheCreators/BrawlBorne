using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns
{
    public class ThrowingWeapon : Gun
    {
        [Header("Settings")]
        [SerializeField, Min(0)] private float _timeBetweenShots = 0.5f;

        [Header("Spawn Settings")]
        [SerializeField] private float _bulletSpawnDistance = 1f;
        [SerializeField] private float _bulletSpawnHeight = 0.5f;

        private bool _isShooting;

        public override void Shoot()
        {
            if (_isShooting)
            {
                return;
            }

            _isShooting = true;
            StartShooting();
            StartCoroutine(StopShootingAfter(_timeBetweenShots));
        }

        private void StartShooting()
        {
            Vector3 spawnPosition = _shootingDirection.position + // Position
                                    _shootingDirection.forward * _bulletSpawnDistance + // Distance from camera
                                    _shootingDirection.up * _bulletSpawnHeight + // Height from camera
                                    transform.right;  // Spread

            Instantiate(_bullet, spawnPosition, _shootingDirection.rotation);
        }

        private IEnumerator StopShootingAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _isShooting = false;
        }
    }
}