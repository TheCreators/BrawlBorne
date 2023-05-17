using UnityEngine;

namespace Combat.Projectiles
{
    public abstract class Projectile : MonoBehaviour
    {
        protected float Damage = 5f;
        protected LayerMask HitLayers;
        
        protected void Init(float damage, LayerMask hitLayers)
        {
            Damage = damage;
            HitLayers = hitLayers;
        }
    }
}