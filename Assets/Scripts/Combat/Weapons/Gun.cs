using System.Collections.Generic;
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

        protected override void OnValidate()
        {
            this.CheckIfNull(_projectile);
            this.CheckIfNull(_projectileSpawnPoint);

            base.OnValidate();
        }
    }
}