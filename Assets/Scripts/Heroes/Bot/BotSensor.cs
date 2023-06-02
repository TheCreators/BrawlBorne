using System;
using System.Collections.Generic;
using Environment;
using JetBrains.Annotations;
using Misc;
using Models;
using UnityEngine;

namespace Heroes.Bot
{
    public class BotSensor : MonoBehaviour
    {
        [SerializeField]
        private ColoredRange _heroDetection;

        [SerializeField]
        private ColoredRange _crateDetection;

        [SerializeField]
        private ColoredRange _boostDetection;

        [SerializeField]
        private ColoredRange _attack;

        [SerializeField]
        private LayerMask _layerMask;
        
        public float AttackRange => _attack.Value;

        public bool IsAnyEnemyInDetectionRange => IsAnyObjectInRange(_heroDetection.Value, ObjectsPool.Instance.Heroes);

        [CanBeNull]
        public Hero ClosestEnemyInDetectionRange => GetClosestObjectInRange(_heroDetection.Value, ObjectsPool.Instance.Heroes);

        public bool IsAnyCrateInDetectionRange => IsAnyObjectInRange(_crateDetection.Value, ObjectsPool.Instance.Crates);

        [CanBeNull]
        public Crate ClosestCrateInDetectionRange => GetClosestObjectInRange(_crateDetection.Value, ObjectsPool.Instance.Crates);

        public bool IsAnyBoostInDetectionRange => IsAnyObjectInRange(_boostDetection.Value, ObjectsPool.Instance.Boosts);

        [CanBeNull]
        public Boost ClosestBoostInDetectionRange => GetClosestObjectInRange(_boostDetection.Value, ObjectsPool.Instance.Boosts);

        public bool IsInAttackRange(Component component) => IsInRange(component, _attack.Value);
        
        public bool IsInDetectionRange(Crate crate) => IsInRange(crate, _crateDetection.Value);
        
        public bool IsInDetectionRange(Hero hero) => IsInRange(hero, _heroDetection.Value);
        
        public bool IsInDetectionRange(Boost boost) => IsInRange(boost, _boostDetection.Value);
        
        private bool IsAnyObjectInRange(float range, IEnumerable<Component> objects)
        {
            foreach (Component obj in objects)
            {
                if (obj == null || obj.gameObject.Equals(gameObject)) continue;

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
                if (obj == null || obj.gameObject.Equals(gameObject)) continue;

                var distance = Vector3.Distance(transform.position, obj.transform.position);

                if (distance > range || distance >= closestDistance) continue;

                closestObject = obj;
                closestDistance = distance;
            }

            return closestObject;
        }

        public bool IsVisible(Component component)
        {
            if (component == null) return false;

            const float sphereRadius = 0.5f;
            const float sphereCastDistance = 100000f;

            Vector3 direction = (component.TryGetComponent(out Hero hero) ? hero.ShootAt : component.transform.position) - transform.position;
            
            var hitColliders = new Collider[3];
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, sphereRadius, hitColliders, _layerMask);
            for (int i = 0; i < numColliders; i++)
            {
                if (hitColliders[i].gameObject.Equals(component.gameObject))
                {
                    return true;
                }
            }

            return Physics.SphereCast(transform.position, sphereRadius, direction, out var hit, sphereCastDistance, _layerMask) && 
                   hit.collider.gameObject.Equals(component.gameObject);
        }

        private bool IsInRange(Component component, float range)
        {
            if (component == null) return false;

            return Vector3.Distance(transform.position, component.transform.position) <= range;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = _heroDetection.Color;
            Gizmos.DrawWireSphere(transform.position, _heroDetection.Value);

            Gizmos.color = _attack.Color;
            Gizmos.DrawWireSphere(transform.position, _attack.Value);

            Gizmos.color = _crateDetection.Color;
            Gizmos.DrawWireSphere(transform.position, _crateDetection.Value);

            Gizmos.color = _boostDetection.Color;
            Gizmos.DrawWireSphere(transform.position, _boostDetection.Value);
        }
    }
}