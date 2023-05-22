using Combat.Projectiles;
using Heroes;
using Misc;
using Models;
using NaughtyAttributes;
using UnityEngine;

namespace Combat.Weapons
{
    public abstract class Gun<TProjectile> : Weapon where TProjectile : Projectile
    {
        [SerializeField] [BoxGroup(Group.Projectiles)] [Required] [ShowAssetPreview]
        protected TProjectile _projectile;

        [SerializeField] [BoxGroup(Group.Projectiles)] [Required]
        protected Transform _projectileSpawnPoint;

        [SerializeField] [BoxGroup(Group.Settings)] [Required]
        protected Transform _shootingDirection;

        protected Hero Owner;

        protected override void Start()
        {
            Owner = this.GetComponentInParentWithNullCheck<Hero>();

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