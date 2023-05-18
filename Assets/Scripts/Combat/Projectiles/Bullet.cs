using Misc;
using UnityEngine;

namespace Combat.Projectiles
{
    public class Bullet : Projectile
    {
        private float _speed = 20f;
        private float _maxDistance = 100f;
        private Vector3 _oldPosition;

        private void Update()
        {
            _oldPosition = transform.position;
            transform.Translate(Vector3.forward * (_speed * Time.deltaTime));
            DetectCollision();
        }

        public void Init(
            float damage, 
            LayerMask hitLayers, 
            Hero sender, 
            float speed, 
            float maxDistance)
        {
            base.Init(damage, hitLayers, sender);
            _speed = speed;
            _maxDistance = maxDistance;
            
            var lifeTime = _maxDistance / _speed;
            Destroy(gameObject, lifeTime);
        }

        private void DetectCollision()
        {
            if (Physics.Linecast(_oldPosition, transform.position, out var hit, HitLayers) is false || 
                (Sender is not null && hit.collider.gameObject == Sender.gameObject)) return;

            if (hit.collider.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(Damage);
            }

            Destroy(gameObject);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * _maxDistance);
        }
    }
}