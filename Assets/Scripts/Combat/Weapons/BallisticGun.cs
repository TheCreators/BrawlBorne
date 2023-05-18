using Combat.Projectiles;
using Misc;
using UnityEngine;

namespace Combat.Weapons
{
    public class BallisticGun : Gun<Bomb>
    {
        [Header("Settings")]
        [SerializeField, Min(0)] private float _throwPower = 10f;
        [SerializeField, Range(0, 2)] private float _heroVelocityInfluence = 0.5f;
        
        [Header("Bomb Settings")]
        [SerializeField] private GameObject _explosion;
        [SerializeField, Min(0)] private float _timeToExplode = 3f;
        [SerializeField, Min(0)] private float _explosionRadius = 5f;
        
        private Rigidbody _rigidbody;
        
        public Transform ShotDirection { get; set; }

        protected override void OnValidate()
        {
            this.CheckIfNull(_explosion);
            
            base.OnValidate();
        }

        private void Awake()
        {
            _rigidbody = this.GetComponentInParentWithNullCheck<Rigidbody>();
        }

        protected override void Start()
        {
            ShotDirection = _shootingDirection;

            base.Start();
        }

        protected override void Shoot()
        {
            CanBeUsed = false;

            Vector3 spawnPosition = _shootingDirection.position + // Position
                                    _shootingDirection.forward * _bulletSpawnDistance + // Distance from camera
                                    _shootingDirection.up * _bulletSpawnHeight; // Height from camera

            Bomb projectile = Instantiate(_projectile, spawnPosition, _shootingDirection.rotation);
            projectile.Init(_damage, _hitLayers, _explosion, _timeToExplode, _explosionRadius);
            projectile.SetVelocity(_rigidbody.velocity * _heroVelocityInfluence);
            projectile.AddForce(_shootingDirection.forward * _throwPower, ForceMode.Impulse);
            
            CanBeUsed = true;
        }
    }
}