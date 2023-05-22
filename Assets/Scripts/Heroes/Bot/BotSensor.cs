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

        public bool IsAnyHeroInDetectionRange => IsAnyObjectInRange(_heroDetection.Value, ObjectsPool.Instance.Heroes);

        [CanBeNull]
        public Hero ClosestHeroInDetectionRange => GetClosestObjectInRange(_heroDetection.Value, ObjectsPool.Instance.Heroes);

        public bool IsAnyHeroInAttackRange => IsAnyObjectInRange(_attack.Value, ObjectsPool.Instance.Heroes);

        [CanBeNull]
        public Hero ClosestHeroInAttackRange => GetClosestObjectInRange(_attack.Value, ObjectsPool.Instance.Heroes);

        public bool IsAnyCrateInDetectionRange => IsAnyObjectInRange(_crateDetection.Value, ObjectsPool.Instance.Crates);

        [CanBeNull]
        public Crate ClosestCrateInDetectionRange => GetClosestObjectInRange(_crateDetection.Value, ObjectsPool.Instance.Crates);

        public bool IsAnyCrateInAttackRange => IsAnyObjectInRange(_attack.Value, ObjectsPool.Instance.Crates);

        [CanBeNull]
        public Crate ClosestCrateInAttackRange => GetClosestObjectInRange(_attack.Value, ObjectsPool.Instance.Crates);

        public bool IsAnyBoostInDetectionRange => IsAnyObjectInRange(_boostDetection.Value, ObjectsPool.Instance.Boosts);

        [CanBeNull]
        public Boost ClosestBoostInDetectionRange => GetClosestObjectInRange(_boostDetection.Value, ObjectsPool.Instance.Boosts);

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

        public bool IsVisible(Hero hero)
        {
            if (hero == null) return false;

            Vector3 direction = hero.ShootAt - transform.position;
            bool isVisible = Physics.SphereCast(transform.position, 0.5f, direction, out var hit, 100000, _layerMask) &&
                             hit.collider.gameObject == hero.gameObject;

            return isVisible;
        }

        public bool IsVisible(Component component)
        {
            if (component == null) return false;

            Vector3 direction = component.transform.position - transform.position;
            bool isVisible = Physics.SphereCast(transform.position, 0.5f, direction, out var hit, 100000, _layerMask) &&
                             hit.collider.gameObject == component.gameObject;

            return isVisible;
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