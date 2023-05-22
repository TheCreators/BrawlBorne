using System.Numerics;
using Events;
using Misc;
using Models;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Heroes.Bot
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BotMovement : MonoBehaviour
    {
        [SerializeField]
        private ColoredMinMaxRange _randomPointRange;

        [SerializeField] [BoxGroup(Group.Strafe)] [Label("Speed")]
        private float _strafeSpeed = 1.0f;

        [SerializeField] [BoxGroup(Group.Strafe)] [Label("Distance")]
        private float _strafeDistance = 1.0f;

        [SerializeField] [BoxGroup(Group.Events)] [Required]
        private GameEvent _onMove;

        [SerializeField] [BoxGroup(Group.Events)] [Required]
        private GameEvent _onStopMoving;

        private float _strafeDirection = 1.0f;
        private float _currentStrafe;
        private bool _isStrafing;

        private NavMeshAgent _agent;

        public Vector3 Destination => _agent.destination;

        public float WalkSpeed
        {
            get => _agent.speed;
            set
            {
                Debug.Log(value);
                _agent.speed = value;
            }
        }

        private void OnValidate()
        {
            this.CheckIfNull(_onMove, _onStopMoving);
        }

        private void Awake()
        {
            _agent = this.GetComponentWithNullCheck<NavMeshAgent>();
        }

        private void Start()
        {
            GoToRandomDestination();
        }

        public bool IsMoving => _agent.velocity.magnitude > 0.1f || _isStrafing;

        public Vector2 GetNormalizedRelativeVelocity()
        {
            if (_isStrafing)
            {
                return new Vector2(0, _strafeDirection * 0.75f);
            }

            var velocity = transform.InverseTransformDirection(_agent.velocity);
            return new Vector2(velocity.z, -velocity.x).normalized;
        }

        [Button("Stop NavMeshAgent", EButtonEnableMode.Playmode)]
        public void Stop()
        {
            _agent.isStopped = true;
        }

        [Button("Resume NavMeshAgent", EButtonEnableMode.Playmode)]
        private void Resume()
        {
            _onMove.Raise(this, null);
            _agent.isStopped = false;
            _isStrafing = false;
        }

        public void GoToDestination(Vector3 destination)
        {
            if (destination == Destination)
            {
                return;
            }

            Resume();
            _agent.SetDestination(destination);
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public void GoToRandomDestination()
        {
            var randomDestination = GetRandomDestination();
            GoToDestination(randomDestination);
        }

        public bool HasReachedDestination()
        {
            return !_agent.pathPending && _agent.remainingDistance < 0.5f;
        }

        public void Strafe()
        {
            Stop();
            _isStrafing = true;
            float strafeStep = _strafeSpeed * Time.deltaTime * _strafeDirection;
            _currentStrafe += strafeStep;

            bool timeToChangeDirection = Mathf.Abs(_currentStrafe) >= _strafeDistance;
            if (timeToChangeDirection)
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
                var randomRadius = Random.Range(_randomPointRange.Min, _randomPointRange.Max);
                var randomDirection = Random.insideUnitSphere * randomRadius;
                randomDirection += transform.position;
                NavMesh.SamplePosition(randomDirection, out var navMeshHit, randomRadius, -1);
                var distance = Vector3.Distance(transform.position, navMeshHit.position);
                if (distance >= _randomPointRange.Min && distance <= _randomPointRange.Max)
                {
                    return navMeshHit.position;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = _randomPointRange.MaxColor;
            Gizmos.DrawWireSphere(transform.position, _randomPointRange.Max);

            Gizmos.color = _randomPointRange.MinColor;
            Gizmos.DrawWireSphere(transform.position, _randomPointRange.Min);
        }
    }
}