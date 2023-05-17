using Combat.Projectiles;
using UnityEngine;

namespace Combat.Weapons
{
    public abstract class BulletGun<TBullet> : Gun<TBullet> where TBullet : Bullet
    {
        [Header("Bullet Settings")]
        [SerializeField, Min(0)] protected float _speed = 20f;
        [SerializeField, Min(0)] protected float _maxDistance = 100f;
    }
}