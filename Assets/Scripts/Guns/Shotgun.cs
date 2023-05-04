using System.Collections;
using UnityEngine;

namespace Guns
{
    public class Shotgun : Gun
    {
        [Header("Settings")]
        [SerializeField, Min(0)] private int _bulletsPerShot = 4;
        [SerializeField, Min(0)] private float _timeBetweenShots = 0.5f;

        [Header("Spawn Settings")]
        [SerializeField, Min(0)] private float _bulletsSpread = 0.5f;
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
            float positionHorizontal = 0.5f;
            float positionVertical = 0.5f;
            for (int i = 0; i < _bulletsPerShot; i++)
            {
                Vector3 spawnPosition = _shootingDirection.position + // Position
                                        _shootingDirection.forward * _bulletSpawnDistance + // Distance from camera
                                        _shootingDirection.up * positionVertical * _bulletSpawnHeight + // Height from camera
                                        _bulletsSpread * positionHorizontal * transform.right; // Spread (left or right)

                Instantiate(_bullet, spawnPosition, _shootingDirection.rotation);

                float temp = i % 2 == 0 ? 1f : -1f;
                positionHorizontal *= -1 * temp;
                positionVertical *= -1;
                yield return new WaitForSeconds(0);
            }
        }

        private IEnumerator StopShootingAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _isShooting = false;
        }
    }
}

