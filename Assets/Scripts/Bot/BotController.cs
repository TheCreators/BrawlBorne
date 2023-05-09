using Combat;
using Combat.Weapons;
using JetBrains.Annotations;
using Misc;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Bot
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BotController : MonoBehaviour
    {
        [Header("Requirements")] [SerializeField]
        private Weapon _weapon;

        [Header("Movement")] [SerializeField] private float _randomPointMaxRadius = 10f;
        [SerializeField] private float _randomPointMinRadius = 5f;
        [SerializeField] private float _playerDetectionRadius = 7f;
        [SerializeField] private float _playerAttackRadius = 5f;
        
        [Header("Strafe")]
        [SerializeField] private float _strafeSpeed = 1.0f;
        [SerializeField] private float _strafeDistance = 1.0f;
        private float _strafeDirection = 1.0f;
        private float _currentStrafe = 0.0f;

        public NavMeshAgent Agent { get; private set; }

        [CanBeNull]
        public GameObject Target { get; private set; }

        public Vector3? TargetShootPosition
        {
            get
            {
                if (Target == null) return null;
                var shootAt = Target.GetComponentInChildren<ShootAt>();
                Debug.Log(shootAt);
                return shootAt == null ? Target.transform.position : shootAt.transform.position;

            }
        }

        public BotState CurrentState { get; private set; } = BotState.Wandering;

        public float RandomPointMaxRadius => _randomPointMaxRadius;
        public float RandomPointMinRadius => _randomPointMinRadius;
        public float PlayerDetectionRadius => _playerDetectionRadius;
        public float PlayerAttackRadius => _playerAttackRadius;


        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Target = gameObject;
        }

        private void Start()
        {
            Agent.SetDestination(GetRandomDestination());
        }

        private void Update()
        {
            switch (CurrentState)
            {
                case BotState.Wandering:
                    HandleWanderingState();
                    break;

                case BotState.Chasing:
                    HandleChasingState();
                    break;

                case BotState.Attacking:
                    HandleAttackingState();
                    break;
            }
        }

        private void HandleWanderingState()
        {
            if (IsAnyHeroInRange(_playerDetectionRadius))
            {
                CurrentState = BotState.Chasing;
                return;
            }

            if (HasReachedDestination() is false) return;

            Agent.SetDestination(GetRandomDestination());
        }

        private void HandleChasingState()
        {
            var closestHero = GetClosestHeroInRange(_playerDetectionRadius);
            if (closestHero is null)
            {
                CurrentState = BotState.Wandering;
                return;
            }

            if (IsAnyHeroInRange(_playerAttackRadius))
            {
                CurrentState = BotState.Attacking;
                return;
            }

            Agent.SetDestination(closestHero.transform.position);
        }

        private void HandleAttackingState()
        {
            if (IsAnyHeroInRange(_playerDetectionRadius) is false)
            {
                Agent.isStopped = false;
                CurrentState = BotState.Wandering;
                return;
            }

            Target = GetClosestHeroInRange(_playerAttackRadius);
            if (Target is null)
            {
                Agent.isStopped = false;
                CurrentState = BotState.Chasing;
                return;
            }

            Agent.isStopped = true;

            var lookDirection = TargetShootPosition!.Value - transform.position;
            transform.rotation = Quaternion.LookRotation(lookDirection);
            _weapon.TryUse();

            Strafe();
        }

        private void Strafe()
        {
            float strafeStep = _strafeSpeed * Time.deltaTime * _strafeDirection;
            _currentStrafe += strafeStep;

            if (Mathf.Abs(_currentStrafe) >= _strafeDistance)
            {
                _strafeDirection = -_strafeDirection;
                _currentStrafe = 0.0f;
            }

            transform.position += transform.right * strafeStep;
        }

        private Vector3 GetRandomDestination()
        {
            while (true)
            {
                var randomRadius = Random.Range(_randomPointMinRadius, _randomPointMaxRadius);
                var randomDirection = Random.insideUnitSphere * randomRadius;
                randomDirection += transform.position;
                NavMesh.SamplePosition(randomDirection, out var navMeshHit, randomRadius, -1);
                var distance = Vector3.Distance(transform.position, navMeshHit.position);
                if (distance >= _randomPointMinRadius && distance <= _randomPointMaxRadius)
                {
                    return navMeshHit.position;
                }
            }
        }

        private bool HasReachedDestination() => !Agent.pathPending && Agent.remainingDistance < 0.5f;

        private bool IsAnyHeroInRange(float range)
        {
            foreach (var hero in HeroesPool.Instance.Heroes)
            {
                if (hero == gameObject) continue;

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
                if (hero == gameObject) continue;

                var distance = Vector3.Distance(transform.position, hero.transform.position);

                if (!(distance <= range)) continue;

                if (!(distance < closestDistance)) continue;

                closestHero = hero;
                closestDistance = distance;
            }

            return closestHero;
        }
    }
}