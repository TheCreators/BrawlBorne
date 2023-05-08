using UnityEngine;

namespace Combat.Projectiles
{
    public class Bomb : RigidbodyProjectile
    {
        [SerializeField] private LayerMask _hitLayers;
        [SerializeField, Min(0)] private float _timeToExplode = 3f;
        [SerializeField, Min(0)] private float _damage = 20f;
        [SerializeField, Min(0)] private float _explosionRadius = 5f;

        private void Start()
        {
            Invoke(nameof(Explode), _timeToExplode);
        }

        private void Explode()
        {
            var colliders = new Collider[20];
            int count = Physics.OverlapSphereNonAlloc(transform.position, _explosionRadius, colliders, _hitLayers);

            for (int i = 0; i < count; i++)
            {
                if (colliders[i].gameObject.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(_damage);
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