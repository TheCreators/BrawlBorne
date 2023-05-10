using System.Collections;
using Combat.Projectiles;
using UnityEngine;

namespace Combat.Weapons
{
    public class DualGun : Gun<Bullet>
    {
        [SerializeField, Min(0)] private float _bulletsSpread = 0.5f;

        [Header("Settings")]
        [SerializeField, Min(0)] private int _bulletsPerShot = 6;
        [SerializeField, Min(0)] private float _timeBetweenBullets = 0.25f;
        
        protected override void Shoot()
        {
            StartCoroutine(StartShooting());
        }

        private IEnumerator StartShooting()
        {
            CanBeUsed = false;

            int positionShiftAmount = 1;
            for (int i = 0; i < _bulletsPerShot; i++)
            {
                Vector3 spawnPosition = _shootingDirection.position + // Position
                                        _shootingDirection.forward * _bulletSpawnDistance + // Distance from camera
                                        _shootingDirection.up * _bulletSpawnHeight + // Height from camera
                                        _bulletsSpread * positionShiftAmount * transform.right; // Spread (left or right)

                bool isLastBullet = i == _bulletsPerShot - 1;
                if (isLastBullet is false)
                {
                    _onUse.Raise(this, null);
                }
                
                Instantiate(_projectile, spawnPosition, _shootingDirection.rotation);

                positionShiftAmount *= -1;
                yield return new WaitForSeconds(_timeBetweenBullets);
            }
            
            CanBeUsed = true;
        }
    }
}
