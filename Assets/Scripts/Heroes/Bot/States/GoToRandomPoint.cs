using UnityEngine.AI;

namespace Heroes.Bot.States
{
    public class GoToRandomPoint : NavMeshAgentMovement
    {
        public GoToRandomPoint(Bot bot, NavMeshAgent navMeshAgent, BotAnimation botAnimation)
            : base(bot, navMeshAgent, botAnimation)
        {
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            
            NavMeshAgent.SetDestination(Bot.RandomPoint!.Value);
        }
    }
}