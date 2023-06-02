using JetBrains.Annotations;
using Misc.StateMachine;
using Models;
using UnityEngine;
using UnityEngine.AI;

namespace Heroes.Bot.States
{
    public class ChooseRandomPoint : IState
    {
        private readonly Bot _bot;
        private readonly MinMaxRange _randomPointRange;

        public ChooseRandomPoint(Bot bot, MinMaxRange randomPointRange)
        {
            _bot = bot;
            _randomPointRange = randomPointRange;
        }

        public void Tick()
        {
            _bot.RandomPoint = TryGetRandomDestination();
        }

        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }
        
        [CanBeNull]
        private Vector3? TryGetRandomDestination()
        {
            var randomRadius = Random.Range(_randomPointRange.Min, _randomPointRange.Max);
            for (int i = 0; i < 30; i++)
            {
                var randomDirection2D = Random.insideUnitCircle * randomRadius;
                Vector3 randomDirection = _bot.transform.position + new Vector3(randomDirection2D.x, 0, randomDirection2D.y);

                if (NavMesh.SamplePosition(randomDirection, out var navMeshHit, randomRadius, NavMesh.AllAreas))
                {
                    return navMeshHit.position;
                }
            }
            return null;
        }
    }
}