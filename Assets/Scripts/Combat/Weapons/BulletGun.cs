using Combat.Projectiles;
using Models;
using NaughtyAttributes;
using UnityEngine;

namespace Combat.Weapons
{
    public abstract class BulletGun<TBullet> : Gun<TBullet> where TBullet : Bullet
    {
        [SerializeField] [BoxGroup(Group.Projectiles)] [Min(0)]
        protected float _speed = 20f;

        [SerializeField] [BoxGroup(Group.Projectiles)] [Min(0)]
        protected float _maxDistance = 100f;
    }
}