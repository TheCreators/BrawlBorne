using Environment;
using Misc.StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace Heroes.Bot.States
{
    public class GoToCrate : NavMeshAgentMovement
    {
        private readonly BotSensor _botSensor;

        public GoToCrate(Bot bot, NavMeshAgent navMeshAgent, BotAnimation botAnimation, BotSensor botSensor)
            : base(bot, navMeshAgent, botAnimation)
        {
            _botSensor = botSensor;
        }

        public override void Tick()
        {
            base.Tick();
            
            bool crateMoved = !Mathf.Approximately(NavMeshAgent.destination.x, Bot.Crate.transform.position.x) ||
                              !Mathf.Approximately(NavMeshAgent.destination.z, Bot.Crate.transform.position.z);
            if (crateMoved)
            {
                NavMeshAgent.SetDestination(Bot.Crate.transform.position);
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            if (!_botSensor.IsInDetectionRange(Bot.Crate))
            {
                Bot.Crate = null;
            }
        }
    }
}