using System.Collections;
using UnityEngine;

namespace Guns
{
    public class DualGun : Gun
    {
        [Header("Settings")]
        [SerializeField] [Min(0)] private int _bulletsPerShot = 6;
        [SerializeField] [Min(0)] private float _timeBetweenBullets = 0.25f;
        [SerializeField] [Min(0)] private float _timeBetweenShots = 0.5f;

        [Header("Spawn Settings")]
        [SerializeField] [Min(0)] private float _bulletsSpread = 0.5f;
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
            StartCoroutine(StartShooting());
            StartCoroutine(StopShootingAfter(_timeBetweenShots));
        }

        private IEnumerator StartShooting()
        {
            int positionShiftAmount = 1;
            for (int i = 0; i < _bulletsPerShot; i++)
            {
                Vector3 spawnPosition = _shootingDirection.position + // Position
                                        _shootingDirection.forward * _bulletSpawnDistance + // Distance from camera
                                        _shootingDirection.up * _bulletSpawnHeight + // Height from camera
                                        _bulletsSpread * positionShiftAmount * transform.right; // Spread (left or right)

                Instantiate(_bullet, spawnPosition, _shootingDirection.rotation);

                positionShiftAmount *= -1;
                yield return new WaitForSeconds(_timeBetweenBullets);
            }
        }

        private IEnumerator StopShootingAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _isShooting = false;
        }
    }
}
