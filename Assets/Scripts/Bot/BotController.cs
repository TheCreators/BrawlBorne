using System;
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
            _botMovement = GetComponent<BotMovement>();
            _botSensor = GetComponent<BotSensor>();
            _botCombat = GetComponent<BotCombat>();
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
            var closestHero = _botSensor.ClosestHeroInDetectionRange;
            if (closestHero is null)
            {
                _currentState = BotState.Wandering;
                return;
            }

            
            if (_botSensor.IsAnyHeroInAttackRange && _botSensor.IsVisible(_botSensor.ClosestHeroInAttackRange))
            {
                _currentState = BotState.Attacking;
                return;
            }

            _botMovement.GoToDestination(closestHero.transform.position);
        }

        private void HandleAttackingState()
        {
            if (!_botSensor.IsAnyHeroInDetectionRange)
            {
                _currentState = BotState.Wandering;
                return;
            }

            _botCombat.Target = _botSensor.ClosestHeroInAttackRange;
            if (_botCombat.Target is null || _botSensor.IsVisible(_botCombat.Target) is false)
            {
                _currentState = BotState.Chasing;
                return;
            }

            _botMovement.Stop();
            _botCombat.AimAndTryUseWeapon();
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

                    case BotState.Attacking:
                        Gizmos.color = Color.red;
                        Gizmos.DrawLine(transform.position, _botCombat.TargetShootPosition);
                        break;
                }
            }
            catch (NullReferenceException)
            {
            }
        }
    }
}