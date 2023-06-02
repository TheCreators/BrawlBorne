using UnityEngine;
using UnityEngine.AI;

namespace Heroes.Bot.States
{
    public class GoToEnemy : NavMeshAgentMovement
    {
        private readonly BotSensor _botSensor;

        public GoToEnemy(Bot bot, NavMeshAgent navMeshAgent, BotAnimation botAnimation, BotSensor botSensor)
            : base(bot, navMeshAgent, botAnimation)
        {
            _botSensor = botSensor;
        }

        public override void Tick()
        {
            base.Tick();
            
            if (Bot.Enemy == null) return;
            
            bool enemyMoved = !Mathf.Approximately(NavMeshAgent.destination.x, Bot.Enemy.transform.position.x) ||
                              !Mathf.Approximately(NavMeshAgent.destination.z, Bot.Enemy.transform.position.z);
            if (enemyMoved)
            {
                NavMeshAgent.SetDestination(Bot.Enemy.transform.position);
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            if (!_botSensor.IsInDetectionRange(Bot.Enemy))
            {
                Bot.Crate = null;
            }
        }
    }
}