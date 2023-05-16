using Combat.Projectiles;
using Misc;
using UnityEngine;

namespace Combat.Weapons
{
    public class BallisticGun : Gun<RigidbodyProjectile>
    {
        [Header("Requirements")]
        [SerializeField] private Rigidbody _rigidbody;
        
        [Header("Settings")]
        [SerializeField, Min(0)] private float _throwPower = 10f;
        [SerializeField, Range(0, 2)] private float _heroVelocityInfluence = 0.5f;

        protected override void OnValidate()
        {
            this.CheckIfNull(_rigidbody);
            
            base.OnValidate();
        }

        protected override void Shoot()
        {
            CanBeUsed = false;

            Vector3 spawnPosition = _shootingDirection.position + // Position
                                    _shootingDirection.forward * _bulletSpawnDistance + // Distance from camera
                                    _shootingDirection.up * _bulletSpawnHeight; // Height from camera

            RigidbodyProjectile projectile = Instantiate(_projectile, spawnPosition, _shootingDirection.rotation);
            projectile.SetVelocity(_rigidbody.velocity * _heroVelocityInfluence);
            projectile.AddForce(_shootingDirection.forward * _throwPower, ForceMode.Impulse);
            
            CanBeUsed = true;
        }
    }
}