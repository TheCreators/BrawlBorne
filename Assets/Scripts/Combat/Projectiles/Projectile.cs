using Misc;
using UnityEngine;

namespace Combat.Projectiles
{
    public abstract class Projectile : MonoBehaviour
    {
        protected float Damage = 5f;
        protected LayerMask HitLayers;
        protected Hero Sender;
        
        protected void Init(float damage, LayerMask hitLayers, Hero sender)
        {
            Damage = damage;
            HitLayers = hitLayers;
            Sender = sender;
        }
    }
}