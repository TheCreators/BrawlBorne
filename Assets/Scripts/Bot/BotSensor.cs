using System.Collections.Generic;
using Combat;
using Environment;
using JetBrains.Annotations;
using Misc;
using UnityEngine;

namespace Bot
{
    public class BotSensor : MonoBehaviour
    {
        [SerializeField] private float _playerDetectionRadius = 7f; // TODO: rename to _heroDetectionRange
        [SerializeField] private Color _playerDetectionColor = Color.yellow;
        
        [SerializeField] private float _crateDetectionRadius = 10f;
        [SerializeField] private Color _crateDetectionColor = Color.magenta;
        
        [SerializeField] private float _crateAttackRadius = 5f;
        [SerializeField] private Color _crateAttackColor = Color.red;
        
        [SerializeField] private float _boostDetectionRadius = 10f;
        [SerializeField] private Color _boostDetectionColor = Color.cyan;
        
        [SerializeField] private float _playerAttackRadius = 5f; // TODO: rename to _attackRange
        [SerializeField] private Color _playerAttackColor = Color.red;

        public bool IsAnyHeroInDetectionRange => IsAnyObjectInRange(_playerDetectionRadius, ObjectsPool.Instance.Heroes);
        [CanBeNull] public Hero ClosestHeroInDetectionRange => GetClosestObjectInRange(_playerDetectionRadius, ObjectsPool.Instance.Heroes);
        
        public bool IsAnyHeroInAttackRange => IsAnyObjectInRange(_playerAttackRadius, ObjectsPool.Instance.Heroes);
        [CanBeNull] public Hero ClosestHeroInAttackRange => GetClosestObjectInRange(_playerAttackRadius, ObjectsPool.Instance.Heroes);
        
        public bool IsAnyCrateInDetectionRange => IsAnyObjectInRange(_crateDetectionRadius, ObjectsPool.Instance.Crates);
        [CanBeNull] public Crate ClosestCrateInDetectionRange => GetClosestObjectInRange(_crateDetectionRadius, ObjectsPool.Instance.Crates);
        
        public bool IsAnyCrateInAttackRange => IsAnyObjectInRange(_crateAttackRadius, ObjectsPool.Instance.Crates);
        [CanBeNull] public Crate ClosestCrateInAttackRange => GetClosestObjectInRange(_crateAttackRadius, ObjectsPool.Instance.Crates);
        
        public bool IsAnyBoostInDetectionRange => IsAnyObjectInRange(_boostDetectionRadius, ObjectsPool.Instance.Boosts);
        [CanBeNull] public Boost ClosestBoostInDetectionRange => GetClosestObjectInRange(_boostDetectionRadius, ObjectsPool.Instance.Boosts);
        
        private bool IsAnyObjectInRange(float range, IEnumerable<Component> objects)
        {
            foreach (Component obj in objects)
            {
                if (obj == null) continue;
                
                var distance = Vector3.Distance(transform.position, obj.transform.position);

                if (distance <= range) return true;
            }

            return false;
        }

        [CanBeNull]
        private T GetClosestObjectInRange<T>(float range, IEnumerable<T> objects) where T : Component
        {
            T closestObject = null;
            var closestDistance = Mathf.Infinity;

            foreach (T obj in objects)
            {
                if (obj == null) continue;

                var distance = Vector3.Distance(transform.position, obj.transform.position);

                if (distance > range || distance >= closestDistance) continue;

                closestObject = obj;
                closestDistance = distance;
            }

            return closestObject;
        }

        public bool IsVisible(Hero hero)
        {
            if (hero == null) return false;
            
            Vector3 direction = hero.ShootAt - transform.position;
            bool isVisible = Physics.SphereCast(transform.position, 0.5f, direction, out var hit, _playerDetectionRadius) &&
                             hit.collider.gameObject == hero.gameObject;
            
            return isVisible;
        }
        
        public bool IsVisible(Component component)
        {
            if (component == null) return false;
            
            Vector3 direction = component.transform.position - transform.position;
            bool isVisible = Physics.SphereCast(transform.position, 0.5f, direction, out var hit, _playerDetectionRadius) &&
                             hit.collider.gameObject == component.gameObject;
            
            return isVisible;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = _playerDetectionColor;
            Gizmos.DrawWireSphere(transform.position, _playerDetectionRadius);
            
            Gizmos.color = _playerAttackColor;
            Gizmos.DrawWireSphere(transform.position, _playerAttackRadius);
            
            Gizmos.color = _crateDetectionColor;
            Gizmos.DrawWireSphere(transform.position, _crateDetectionRadius);
            
            Gizmos.color = _boostDetectionColor;
            Gizmos.DrawWireSphere(transform.position, _boostDetectionRadius);
        }
    }
}