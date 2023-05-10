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
        [CanBeNull] public Hero ClosestHeroInDetectionRange => GetClosestHeroInRange(_playerDetectionRadius);
        [CanBeNull] public Hero ClosestHeroInAttackRange => GetClosestHeroInRange(_playerAttackRadius);

        private bool IsAnyHeroInRange(float range)
        {
            foreach (Hero hero in HeroesPool.Instance.Heroes)
            {
                if (hero.gameObject == transform.gameObject) continue;

                var distance = Vector3.Distance(transform.position, hero.ShootAt);

                if (distance > range) continue;

                return true;
            }

            return false;
        }

        [CanBeNull]
        private Hero GetClosestHeroInRange(float range)
        {
            Hero closestHero = null;
            var closestDistance = Mathf.Infinity;

            foreach (Hero hero in HeroesPool.Instance.Heroes)
            {
                if (hero.gameObject == transform.gameObject) continue;

                var distance = Vector3.Distance(transform.position, hero.ShootAt);

                if (distance > range || distance >= closestDistance) continue;

                closestHero = hero;
                closestDistance = distance;
            }

            return closestHero;
        }

        public bool IsVisible(Hero hero)
        {
            if (hero == null) return false;
            
            Vector3 direction = hero.ShootAt - transform.position;
            bool isVisible = Physics.SphereCast(transform.position, 0.5f, direction, out var hit, _playerDetectionRadius) &&
                             hit.collider.gameObject == hero.gameObject;
            
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