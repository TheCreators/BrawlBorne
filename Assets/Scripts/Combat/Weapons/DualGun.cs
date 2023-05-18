using System.Collections;
using Combat.Projectiles;
using UnityEngine;

namespace Combat.Weapons
{
    public class DualGun : BulletGun<Bullet>
    {

        [Header("Gun Settings")]
        [SerializeField, Min(0)] private float _bulletsSpread = 0.5f;
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
                Vector3 spawnPosition = _projectileSpawnPoint.position + // Position
                                        _bulletsSpread * positionShiftAmount * transform.right; // Spread (left or right)

                bool isLastBullet = i == _bulletsPerShot - 1;
                if (isLastBullet is false)
                {
                    _onUse.Raise(this, null);
                }
                
                Bullet bullet = Instantiate(_projectile, spawnPosition, _shootingDirection.rotation);
                bullet.Init(_damage, _hitLayers, Hero, _speed, _maxDistance);

                positionShiftAmount *= -1;
                yield return new WaitForSeconds(_timeBetweenBullets);
            }
            
            CanBeUsed = true;
        }
    }
}
