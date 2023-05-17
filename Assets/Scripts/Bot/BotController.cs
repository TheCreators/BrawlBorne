using System;
using Environment;
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
        [SerializeField] private BotTarget _currentTarget = BotTarget.None;

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
                _currentTarget = BotTarget.Hero;
                _currentState = BotState.Chasing;
                return;
            }
            if (_botSensor.IsAnyCrateInDetectionRange)
            {
                _currentTarget = BotTarget.Crate;
                _currentState = BotState.Chasing;
                return;
            }
            if (_botSensor.IsAnyBoostInDetectionRange)
            {
                _currentTarget = BotTarget.Boost;
                _currentState = BotState.Chasing;
                return;
            }
            
            _botMovement.Resume();
            if (!_botMovement.HasReachedDestination()) return;

            _botMovement.GoToRandomDestination();
        }

        private void HandleChasingState()
        {
            switch (_currentTarget)
            {
                case BotTarget.Hero:
                    HandleChasingHero();
                    break;
                case BotTarget.Boost:
                    HandleChasingBoost();
                    break;
                case BotTarget.Crate:
                    HandleChasingCrate();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleChasingHero()
        {
            Hero closestHeroInDetectionRange = _botSensor.ClosestHeroInDetectionRange;
            if (closestHeroInDetectionRange is null)
            {
                _currentTarget = BotTarget.None;
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

        private void HandleChasingBoost()
        {
            Boost closestBoostInDetectionRange = _botSensor.ClosestBoostInDetectionRange;
            if (closestBoostInDetectionRange is null)
            {
                _currentTarget = BotTarget.None;
                _currentState = BotState.Wandering;
                return;
            }
            
            if (_botSensor.IsAnyHeroInDetectionRange)
            {
                _currentTarget = BotTarget.Hero;
                return;
            }

            _botMovement.GoToDestination(closestBoostInDetectionRange.transform.position);
        }
        
        private void HandleChasingCrate()
        {
            Crate closestCrateInDetectionRange = _botSensor.ClosestCrateInDetectionRange;
            if (closestCrateInDetectionRange is null)
            {
                _currentTarget = BotTarget.None;
                _currentState = BotState.Wandering;
                return;
            }
            
            if (_botSensor.IsAnyHeroInDetectionRange)
            {
                _currentTarget = BotTarget.Hero;
                return;
            }
            
            if (_botSensor.IsAnyBoostInDetectionRange)
            {
                _currentTarget = BotTarget.Boost;
                return;
            }

            if (_botSensor.IsAnyCrateInAttackRange && _botSensor.IsVisible(_botSensor.ClosestCrateInAttackRange))
            {
                _currentState = BotState.Attacking;
                return;
            }
            
            _botMovement.GoToDestination(closestCrateInDetectionRange.transform.position);
        }

        private void HandleAttackingState()
        {
            switch (_currentTarget)
            {
                case BotTarget.Hero:
                    HandleAttackingHero();
                    break;
                case BotTarget.Crate:
                    HandleAttackingCrate();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleAttackingHero()
        {
            Hero closestHeroInAttackRange = _botSensor.ClosestHeroInAttackRange;
            if (closestHeroInAttackRange is null || _botSensor.IsVisible(closestHeroInAttackRange) is false)
            {
                _currentState = BotState.Chasing;
                return;
            }

            _botCombat.Shoot(closestHeroInAttackRange);
            _botMovement.Strafe();
        }
        
        private void HandleAttackingCrate()
        {
            Crate closestCrateInAttackRange = _botSensor.ClosestCrateInAttackRange;
            if (closestCrateInAttackRange is null || _botSensor.IsVisible(closestCrateInAttackRange) is false)
            {
                _currentState = BotState.Chasing;
                return;
            }
            
            if (_botSensor.IsAnyHeroInDetectionRange)
            {
                _currentTarget = BotTarget.Hero;
                _currentState = BotState.Chasing;
                return;
            }
            
            if (_botSensor.IsAnyBoostInDetectionRange)
            {
                _currentTarget = BotTarget.Boost;
                _currentState = BotState.Chasing;
                return;
            }

            _botCombat.Shoot(closestCrateInAttackRange);
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