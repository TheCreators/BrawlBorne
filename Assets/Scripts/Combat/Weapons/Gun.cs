using Combat.Projectiles;
using Misc;
using UnityEngine;
using UnityEngine.Serialization;

namespace Combat.Weapons
{
    public abstract class Gun<TProjectile> : Weapon where TProjectile : Projectile
    {
        [Header("Requirements")]
        [SerializeField] protected TProjectile _projectile;
        [SerializeField] protected Transform _projectileSpawnPoint;
        [SerializeField] protected Transform _shootingDirection;

        protected Hero Hero;
        
        protected override void Start()
        {
            Hero = this.GetComponentInParentWithNullCheck<Hero>();
            
            base.Start();
        }
        
        protected override void OnValidate()
        {
            this.CheckIfNull(_projectile);
            this.CheckIfNull(_shootingDirection);
            this.CheckIfNull(_projectileSpawnPoint);
            
            base.OnValidate();
        }

        protected override void Use()
        {
            Shoot();
        }
        
        protected abstract void Shoot();
    }
}