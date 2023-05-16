using Combat.Projectiles;
using Misc;
using UnityEngine;

namespace Combat.Weapons
{
    public abstract class Gun<TProjectile> : Weapon where TProjectile : Projectile
    {
        [Header("Requirements")]
        [SerializeField] protected TProjectile _projectile;
        [SerializeField] protected Transform _shootingDirection;
        
        [Header("Bullet Spawn Settings")]
        [SerializeField] protected float _bulletSpawnDistance = 1f;
        [SerializeField] protected float _bulletSpawnHeight = 0.5f;
        
        protected virtual void OnValidate()
        {
            this.CheckIfNull(_projectile);
            this.CheckIfNull(_shootingDirection);
        }

        protected override void Use()
        {
            Shoot();
        }
        
        protected abstract void Shoot();
    }
}