using Misc.StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace Heroes.Bot.States
{
    public abstract class NavMeshAgentMovement : IState
    {
        protected readonly Bot Bot;
        protected readonly NavMeshAgent NavMeshAgent;
        private readonly BotAnimation _botAnimation;

        protected NavMeshAgentMovement(Bot bot, NavMeshAgent navMeshAgent, BotAnimation botAnimation)
        {
            Bot = bot;
            NavMeshAgent = navMeshAgent;
            _botAnimation = botAnimation;
        }
        
        public virtual void Tick()
        {
            Vector3 localVelocity = Bot.transform.InverseTransformDirection(NavMeshAgent.velocity);
            Vector3 normalizedLocalVelocity = localVelocity / NavMeshAgent.speed;
            
            _botAnimation.ForwardSpeed = Mathf.Clamp(normalizedLocalVelocity.z, -1f, 1f);
            _botAnimation.RightSpeed = Mathf.Clamp(normalizedLocalVelocity.x, -1f, 1f);
        }

        public virtual void OnEnter()
        {
            NavMeshAgent.enabled = true;
            _botAnimation.IsMoving = true;
        }

        public virtual void OnExit()
        {
            NavMeshAgent.enabled = false;
            _botAnimation.IsMoving = false;
        }
    }
}