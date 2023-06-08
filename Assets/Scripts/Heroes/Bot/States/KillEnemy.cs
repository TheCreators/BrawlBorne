using System.Collections.Generic;
using Combat;
using Heroes.Bot.AimingStrategies;
using UnityEngine;
using UnityEngine.AI;

namespace Heroes.Bot.States
{
    public class KillEnemy : NavMeshAgentMovement
    {
        private readonly Usable _weapon;
        private readonly AimingStrategy _weaponAimingStrategy;
        private readonly Usable _ultimate;
        private readonly AimingStrategy _ultimateAimingStrategy;
        private readonly float _strafeLength;
        private readonly float _strafeDistanceFromTarget;

        private float _strafeDirection;

        public KillEnemy(
            Bot bot, 
            NavMeshAgent navMeshAgent, 
            BotAnimation botAnimation, 
            Usable weapon, 
            AimingStrategy weaponAimingStrategy,
            Usable ultimate, 
            AimingStrategy ultimateAimingStrategy,
            float strafeLength,
            float strafeDistanceFromTarget)
            : base(bot, navMeshAgent, botAnimation)
        {
            _weapon = weapon;
            _weaponAimingStrategy = weaponAimingStrategy;
            _ultimate = ultimate;
            _ultimateAimingStrategy = ultimateAimingStrategy;
            _strafeLength = strafeLength;
            _strafeDistanceFromTarget = strafeDistanceFromTarget;
        }

        public override void Tick()
        {
            base.Tick();
            
            Vector3 lookDirection = LookAtEnemy();
            TryAdjustStrafingPosition(lookDirection);
            
            _weapon.TryUse(GetWeaponAimRotations());
            _ultimate.TryUse(GetUltimateAimRotations());
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _strafeDirection = Random.value < 0.5 ? -1 : 1;
        }
        
        private Vector3 LookAtEnemy()
        {
            Vector3 lookDirection = Bot.Enemy!.ShootAt - Bot.transform.position;
            lookDirection.y = 0;
            Bot.transform.rotation = Quaternion.Euler(0, Quaternion.LookRotation(lookDirection).eulerAngles.y, 0);
            return lookDirection;
        }
        
        private IEnumerator<Quaternion> GetWeaponAimRotations()
        {
            while (Bot.Enemy != null)
            {
                yield return _weaponAimingStrategy.GetAimRotation(Bot.ShootFrom, Bot.Enemy.ShootAt, _weapon);
            }
        }
        
        private IEnumerator<Quaternion> GetUltimateAimRotations()
        {
            while (Bot.Enemy != null)
            {
                yield return _ultimateAimingStrategy.GetAimRotation(Bot.ShootFrom, Bot.Enemy.ShootAt, _ultimate);
            }
        }

        private void TryAdjustStrafingPosition(Vector3 lookDirection)
        {
            if (NavMeshAgent.hasPath && !(NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance)) return;
            
            float angle = 2 * Mathf.Asin(_strafeLength / (2 * _strafeDistanceFromTarget));

            Quaternion rotation = Quaternion.Euler(0, _strafeDirection * Mathf.Rad2Deg * angle, 0);
            Vector3 newLookDirection = rotation * lookDirection;

            Vector3 targetPosition = Bot.Enemy!.ShootAt - newLookDirection.normalized * _strafeDistanceFromTarget;
            NavMeshAgent.SetDestination(targetPosition);

            _strafeDirection *= -1;
        }
    }
}