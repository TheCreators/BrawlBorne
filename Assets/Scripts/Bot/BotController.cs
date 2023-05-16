using System;
using Misc;
using UnityEngine;

namespace Bot
{
    [RequireComponent(typeof(BotMovement))]
    [RequireComponent(typeof(BotSensor))]
    [RequireComponent(typeof(BotCombat))]
    public class BotController : MonoBehaviour
    {
        private BotMovement _botMovement;
        private BotSensor _botSensor;
        private BotCombat _botCombat;

        [SerializeField] private BotState _currentState = BotState.Wandering;

        private void Awake()
        {
            _botMovement = this.GetComponentWithNullCheck<BotMovement>();
            _botSensor = this.GetComponentWithNullCheck<BotSensor>();
            _botCombat = this.GetComponentWithNullCheck<BotCombat>();
        }

        private void Update()
        {
            switch (_currentState)
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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleWanderingState()
        {
            if (_botSensor.IsAnyHeroInDetectionRange)
            {
                _currentState = BotState.Chasing;
                return;
            }
            
            _botMovement.Resume();
            if (!_botMovement.HasReachedDestination()) return;

            _botMovement.GoToRandomDestination();
        }

        private void HandleChasingState()
        {
            Hero closestHeroInDetectionRange = _botSensor.ClosestHeroInDetectionRange;
            if (closestHeroInDetectionRange is null)
            {
                _currentState = BotState.Wandering;
                return;
            }
            
            if (_botSensor.IsAnyHeroInAttackRange && _botSensor.IsVisible(_botSensor.ClosestHeroInAttackRange))
            {
                _currentState = BotState.Attacking;
                return;
            }

            _botMovement.GoToDestination(closestHeroInDetectionRange.ShootAt);
        }

        private void HandleAttackingState()
        {
            if (_botSensor.IsAnyHeroInDetectionRange is false)
            {
                _currentState = BotState.Wandering;
                return;
            }

            Hero closestHeroInAttackRange = _botSensor.ClosestHeroInAttackRange;
            if (closestHeroInAttackRange is null || _botSensor.IsVisible(closestHeroInAttackRange) is false)
            {
                _currentState = BotState.Chasing;
                return;
            }

            _botCombat.Shoot(closestHeroInAttackRange);
            _botMovement.Strafe();
        }

        private void OnDrawGizmos()
        {
            try
            {
                switch (_currentState)
                {
                    case BotState.Wandering:
                        Gizmos.color = Color.green;
                        Gizmos.DrawLine(transform.position, _botMovement.Destination);
                        break;

                    case BotState.Chasing:
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawLine(transform.position, _botMovement.Destination);
                        break;
                }
            }
            catch (SystemException)
            {
            }
        }
    }
}