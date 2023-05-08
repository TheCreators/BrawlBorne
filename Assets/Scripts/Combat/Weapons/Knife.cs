using UnityEngine;

namespace Combat.Weapons
{
    public class Knife : Weapon
    {
        [Header("Requirements")]
        [SerializeField] protected Transform _lookDirection;
        
        [Header("Settings")]
        [SerializeField, Min(0)] private float _damage = 10f;
        [SerializeField] private LayerMask _hitLayers;
        
        [Header("Area Settings")]
        [SerializeField, Min(0)] private float _radius = 0.5f;
        [SerializeField, Min(0)] private float _length = 0.5f;
        [SerializeField] private Vector3 _hitOffset;

        protected override void Use()
        {
            CanBeUsed = false;
            Swing();
            CanBeUsed = true;
        }

        private void Swing()
        {
            Vector3 castStart = _lookDirection.position + _hitOffset;
            
            if (Physics.SphereCast(castStart, _radius, _lookDirection.forward, out RaycastHit hit, _length, _hitLayers) is false) return;
     
            if (hit.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_damage);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 capsuleStart = _lookDirection.position + _hitOffset;
            Vector3 capsuleEnd = _lookDirection.position + _lookDirection.forward * _length + _hitOffset;
            
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(capsuleStart, _radius);
            Gizmos.DrawWireSphere(capsuleEnd, _radius);

            Gizmos.DrawLine(capsuleStart + _lookDirection.right * _radius, capsuleEnd + _lookDirection.right * _radius);
            Gizmos.DrawLine(capsuleStart - _lookDirection.right * _radius, capsuleEnd - _lookDirection.right * _radius);
            Gizmos.DrawLine(capsuleStart + _lookDirection.up * _radius, capsuleEnd + _lookDirection.up * _radius);
            Gizmos.DrawLine(capsuleStart - _lookDirection.up * _radius, capsuleEnd - _lookDirection.up * _radius);
        }
    }
}
