using System;
using UnityEngine;

namespace Bot
{
    [RequireComponent(typeof(BotController))]
    public class BotGizmos : MonoBehaviour
    {
        private BotController _botController;

        private void Awake()
        {
            _botController = GetComponent<BotController>();
        }

        private void Update()
        {
            switch (_botController.CurrentState)
            {
                case BotState.Wandering:
                    Debug.DrawLine(transform.position, _botController.Agent.destination, Color.green);
                    break;

                case BotState.Chasing:
                    Debug.DrawLine(transform.position, _botController.Agent.destination, Color.yellow);
                    break;

                case BotState.Attacking:
                    if (_botController.Target == null) break;
                    Debug.DrawLine(transform.position, _botController.Target.transform.position, Color.red);
                    break;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _botController.RandomPointMaxRadius);
            Gizmos.DrawWireSphere(transform.position, _botController.RandomPointMinRadius);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _botController.PlayerDetectionRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _botController.PlayerAttackRadius);
        }
    }
}