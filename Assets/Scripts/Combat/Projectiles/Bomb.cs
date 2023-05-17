using Misc;
using UnityEngine;

namespace Combat.Projectiles
{
    public class Bomb : RigidbodyProjectile
    {
        private GameObject _explosion;
        private float _timeToExplode = 3f;
        private float _explosionRadius = 5f;

        public void Init(
            float damage,
            LayerMask hitLayers,
            GameObject explosion,
            float timeToExplode,
            float explosionRadius)
        {
            base.Init(damage, hitLayers);
            _explosion = explosion;
            _timeToExplode = timeToExplode;
            _explosionRadius = explosionRadius;

            Invoke(nameof(Explode), _timeToExplode);
        }

        private void Explode()
        {
            Instantiate(_explosion, transform.position, _explosion.transform.rotation);
            var colliders = new Collider[20];
            int count = Physics.OverlapSphereNonAlloc(transform.position, _explosionRadius, colliders, HitLayers);

            for (int i = 0; i < count; i++)
            {
                if (colliders[i].gameObject.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(Damage);
                }
            }

            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        }
    }
}