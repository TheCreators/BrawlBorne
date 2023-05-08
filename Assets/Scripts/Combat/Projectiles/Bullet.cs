using UnityEngine;

namespace Combat.Projectiles
{
    public class Bullet : Projectile
    {
        [SerializeField] private LayerMask _hitLayers;
        [SerializeField, Min(0)] private float _speed = 20f;
        [SerializeField, Min(0)] private float _maxDistance = 100f;
        [SerializeField, Min(0)] private float _damage = 5f;
        

        private Vector3 _oldPosition;

        private void Start()
        {
            var lifeTime = _maxDistance / _speed;
            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            _oldPosition = transform.position;
            transform.Translate(Vector3.forward * (_speed * Time.deltaTime));
            DetectCollision();
        }

        private void DetectCollision()
        {
            if (Physics.Linecast(_oldPosition, transform.position, out var hit, _hitLayers) is false) return;

            if (hit.collider.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_damage);
            }

            Destroy(gameObject);
        }
    }
}