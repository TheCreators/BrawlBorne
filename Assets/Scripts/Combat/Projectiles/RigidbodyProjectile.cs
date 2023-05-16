using System;
using Misc;
using UnityEngine;

namespace Combat.Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class RigidbodyProjectile : Projectile
    {
        private Rigidbody _rigidbody;
        
        private void Awake()
        {
            _rigidbody = this.GetComponentWithNullCheck<Rigidbody>();
        }
        
        public void SetVelocity(Vector3 velocity)
        {
            _rigidbody.velocity = velocity;
        }

        public void AddForce(Vector3 force, ForceMode forceMode)
        {
            _rigidbody.AddForce(force, forceMode);
        }
    }
}