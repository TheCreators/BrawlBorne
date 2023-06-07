using System.Collections.Generic;
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

        [SerializeField] [BoxGroup(Group.Hit)] [Min(0)]
        private float _explosionRadius = 5f;

        private Rigidbody _rigidbody;
        
        public float ThrowPower => _throwPower;
        
        protected override void Awake()
        {
            base.Awake();
            
            _rigidbody = this.GetComponentInParentWithNullCheck<Rigidbody>();
        }

        protected override void Use(IEnumerator<Quaternion> aimRotations)
        {
            if (!aimRotations.MoveNext())
            {
                Debug.LogError("No aim rotations");
                return;
            }
            
            CanBeUsed = false;

            Bomb projectile = Instantiate(_projectile, _projectileSpawnPoint.position, aimRotations.Current);
            projectile.Init(_damage, _hitLayers, Owner, _explosionRadius);
            projectile.SetVelocity(_rigidbody.velocity * _heroVelocityInfluence);
            projectile.AddForce(projectile.transform.forward * _throwPower, ForceMode.Impulse);

            CanBeUsed = true;
        }
    }
}