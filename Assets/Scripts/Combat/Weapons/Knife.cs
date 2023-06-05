using System.Collections.Generic;
using Misc;
using Models;
using NaughtyAttributes;
using UnityEngine;

namespace Combat.Weapons
{
    public class Knife : Weapon
    {
        [SerializeField] [BoxGroup(Group.Settings)] [Required]
        protected Transform _lookDirection;

        [SerializeField] [BoxGroup(Group.Hit)] [Min(0)]
        private float _radius = 0.5f;

        [SerializeField] [BoxGroup(Group.Hit)] [Min(0)]
        private float _length = 0.5f;

        [SerializeField] [BoxGroup(Group.Hit)]
        private Vector3 _hitOffset;

        protected override void OnValidate()
        {
            this.CheckIfNull(_lookDirection);
            this.CheckIfNull(_hitOffset);

            base.OnValidate();
        }

        protected override void Use(IEnumerator<Quaternion> aimRotations)
        {
            CanBeUsed = false;
            
            Swing(aimRotations);
            CanBeUsed = true;
        }

        private void Swing(IEnumerator<Quaternion> aimRotations)
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