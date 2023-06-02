using Combat.Weapons;
using Ultimates;
using UnityEngine;
using UnityEngine.AI;

namespace Heroes.Bot.States
{
    public class KillEnemy : NavMeshAgentMovement
    {
        private readonly Weapon _weapon;
        private readonly Ultimate _ultimate;
        private readonly float _strafeLength;
        private readonly float _strafeDistanceFromTarget;

        private float _strafeDirection;

        public KillEnemy(
            Bot bot, 
            NavMeshAgent navMeshAgent, 
            BotAnimation botAnimation, 
            Weapon weapon, 
            Ultimate ultimate, 
            float strafeLength,
            float strafeDistanceFromTarget)
            : base(bot, navMeshAgent, botAnimation)
        {
            _weapon = weapon;
            _ultimate = ultimate;
            _strafeLength = strafeLength;
            _strafeDistanceFromTarget = strafeDistanceFromTarget;
        }

        public override void Tick()
        {
            base.Tick();
            
            Vector3 lookDirection = Aim();
            Attack();
            TryAdjustStrafingPosition(lookDirection);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _strafeDirection = Random.value < 0.5 ? -1 : 1;
        }
        
        private Vector3 Aim()
        {
            Vector3 lookDirection = Bot.Enemy.ShootAt - Bot.transform.position;
            Bot.transform.rotation = Quaternion.LookRotation(lookDirection);
            return lookDirection;
        }
        
        private void Attack()
        {
            _weapon.TryUse();
            _ultimate.TryUse();
        }

        private void TryAdjustStrafingPosition(Vector3 lookDirection)
        {
            if (!NavMeshAgent.hasPath || NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance)
            {
                float angle = 2 * Mathf.Asin(_strafeLength / (2 * _strafeDistanceFromTarget));

                Quaternion rotation = Quaternion.Euler(0, _strafeDirection * Mathf.Rad2Deg * angle, 0);
                Vector3 newLookDirection = rotation * lookDirection;

                Vector3 targetPosition = Bot.Enemy.ShootAt - newLookDirection.normalized * _strafeDistanceFromTarget;
                NavMeshAgent.SetDestination(targetPosition);

                _strafeDirection *= -1;
            }
        }
    }
}