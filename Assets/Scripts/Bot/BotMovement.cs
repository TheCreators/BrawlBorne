﻿using Events;
using Misc;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Bot
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BotMovement : MonoBehaviour
    {
        [Header("Random Point")]
        [SerializeField] private float _randomPointMaxRadius = 10f;
        [SerializeField] private float _randomPointMinRadius = 5f;
        
        [Header("Strafe")]
        [SerializeField] private float _strafeSpeed = 1.0f;
        [SerializeField] private float _strafeDistance = 1.0f;

        [Header("Events")] 
        [SerializeField] private GameEvent _onMove;
        [SerializeField] private GameEvent _onStopMoving;
        
        private float _strafeDirection = 1.0f;
        private float _currentStrafe = 0.0f;
        private bool _isStrafing = false;

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
        
        public void Stop()
        {
            _agent.isStopped = true;
        }
        
        public void Resume()
        {
            _onMove.Raise(this, null);
            _agent.isStopped = false;
            _isStrafing = false;
        }
        
        public void GoToDestination(Vector3 destination)
        {
            Resume();
            _agent.SetDestination(destination);
        }
        
        public void GoToRandomDestination()
        {
            Resume();
            var randomDestination = GetRandomDestination();
            _agent.SetDestination(randomDestination);
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
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _randomPointMaxRadius);
            Gizmos.DrawWireSphere(transform.position, _randomPointMinRadius);
        }
    }

}