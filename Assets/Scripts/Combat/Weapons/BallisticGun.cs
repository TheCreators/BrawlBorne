using Combat.Projectiles;
using Misc;
using Models;
using NaughtyAttributes;
using UnityEngine;

namespace Combat.Weapons
{
    public class BallisticGun : Gun<Bomb>
    {
        [SerializeField] [BoxGroup(Group.Settings)] [Min(0)]
        private float _throwPower = 10f;

        [SerializeField] [BoxGroup(Group.Settings)] [Range(0, 2)]
        private float _heroVelocityInfluence = 0.5f;

        [SerializeField] [BoxGroup(Group.Projectiles)] [Min(0)]
        private float _timeToExplode = 3f;

        [SerializeField] [BoxGroup(Group.Hit)] [Min(0)]
        private float _explosionRadius = 5f;

        private Rigidbody _rigidbody;

        public Transform ShotDirection { get; set; }

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

            Bomb projectile = Instantiate(_projectile, _projectileSpawnPoint.position, _shootingDirection.rotation);
            projectile.Init(_damage, _hitLayers, Owner, _timeToExplode, _explosionRadius);
            projectile.SetVelocity(_rigidbody.velocity * _heroVelocityInfluence);
            projectile.AddForce(_shootingDirection.forward * _throwPower, ForceMode.Impulse);

            CanBeUsed = true;
        }
    }
}