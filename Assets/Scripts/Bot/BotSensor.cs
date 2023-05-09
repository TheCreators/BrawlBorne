using Combat;
using JetBrains.Annotations;
using Misc;
using UnityEngine;

namespace Bot
{
    public class BotSensor : MonoBehaviour
    {
        [SerializeField] private float _playerDetectionRadius = 7f;
        [SerializeField] private float _playerAttackRadius = 5f;

        public bool IsAnyHeroInDetectionRange => IsAnyHeroInRange(_playerDetectionRadius);
        public bool IsAnyHeroInAttackRange => IsAnyHeroInRange(_playerAttackRadius);
        [CanBeNull] public GameObject ClosestHeroInDetectionRange => GetClosestHeroInRange(_playerDetectionRadius);
        [CanBeNull] public GameObject ClosestHeroInAttackRange => GetClosestHeroInRange(_playerAttackRadius);

        private bool IsAnyHeroInRange(float range)
        {
            foreach (var hero in HeroesPool.Instance.Heroes)
            {
                if (hero == transform.gameObject) continue;

                var distance = Vector3.Distance(transform.position, hero.transform.position);

                if (!(distance <= range)) continue;

                return true;
            }

            return false;
        }

        [CanBeNull]
        private GameObject GetClosestHeroInRange(float range)
        {
            GameObject closestHero = null;
            var closestDistance = Mathf.Infinity;

            foreach (var hero in HeroesPool.Instance.Heroes)
            {
                if (hero == transform.gameObject) continue;

                var distance = Vector3.Distance(transform.position, hero.transform.position);

                if (!(distance <= range)) continue;

                if (!(distance < closestDistance)) continue;

                closestHero = hero;
                closestDistance = distance;
            }

            return closestHero;
        }

        public bool IsVisible(GameObject target)
        {
            if (target == null) return false;

            Vector3 direction = target.transform.position - transform.position;
            bool isVisible = Physics.SphereCast(transform.position, 2f, direction, out RaycastHit hit, _playerDetectionRadius) &&
                             hit.collider.gameObject == target;
            return isVisible;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _playerDetectionRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _playerAttackRadius);
        }
    }
}