using UnityEngine.AI;

namespace Heroes.Bot.States
{
    public class CollectBoost : NavMeshAgentMovement
    {
        private readonly BotSensor _botSensor;

        public CollectBoost(Bot bot, NavMeshAgent navMeshAgent, BotAnimation botAnimation, BotSensor botSensor)
            : base(bot, navMeshAgent, botAnimation)
        {
            _botSensor = botSensor;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            
            NavMeshAgent.SetDestination(Bot.Boost.transform.position);
        }

        public override void OnExit()
        {
            base.OnExit();
            
            if (!_botSensor.IsInDetectionRange(Bot.Boost))
            {
                Bot.Boost = null;
            }
        }
    }
}