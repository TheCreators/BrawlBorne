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
                    var targetShootPosition = _botController.TargetShootPosition;
                    if (targetShootPosition == null) break;
                    Debug.DrawLine(transform.position, targetShootPosition.Value, Color.red);
                    break;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_botController == null) return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _botController.PlayerDetectionRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _botController.PlayerAttackRadius);
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _botController.RandomPointMaxRadius);
            Gizmos.DrawWireSphere(transform.position, _botController.RandomPointMinRadius);
        }
    }
}