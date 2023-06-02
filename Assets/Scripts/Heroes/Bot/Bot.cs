using System;
using Combat.Weapons;
using Environment;
using Heroes.Bot.States;
using JetBrains.Annotations;
using Misc;
using Misc.StateMachine;
using Models;
using NaughtyAttributes;
using Ultimates;
using UnityEngine;
using UnityEngine.AI;

namespace Heroes.Bot
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(BotAnimation))]
    [RequireComponent(typeof(BotSensor))]
    public class Bot : Hero
    {
        [SerializeField] [BoxGroup(Group.Settings)]
        private ColoredMinMaxRange _randomPointRange;

        [SerializeField] [BoxGroup(Group.Combat)] [Required] [ShowAssetPreview]
        private Weapon _weapon;

        [SerializeField] [BoxGroup(Group.Combat)] [Required] [ShowAssetPreview]
        private Ultimate _ultimate;

        [SerializeField] [BoxGroup(Group.Combat)] [MinValue(0.0f)]
        private float _strafeLength = 2.0f;

        private StateMachine _stateMachine;
        private const float StrafeDistanceFromTargetOffset = 0.5f;

        [ShowNativeProperty]
        private string CurrentState => _stateMachine?.CurrentState.ToString();

        public Vector3? RandomPoint { get; set; }
        [CanBeNull]
        public Crate Crate { get; set; }
        [CanBeNull]
        public Boost Boost { get; set; }
        [CanBeNull]
        public Hero Enemy { get; set; }
        
        protected override void OnValidate()
        {
            base.OnValidate();
            
            this.CheckIfNull(_weapon);
            this.CheckIfNull(_ultimate);
        }

        private void Awake()
        {
            // Components
            var navMeshAgent = this.GetComponentWithNullCheck<NavMeshAgent>();
            var botSensor = this.GetComponentWithNullCheck<BotSensor>();
            var botAnimation = this.GetComponentWithNullCheck<BotAnimation>();
            _stateMachine = new StateMachine();
            
            // States
            var chooseRandomPoint = new ChooseRandomPoint(this, _randomPointRange);
            var goToRandomPoint = new GoToRandomPoint(this, navMeshAgent, botAnimation);

            var chooseCrate = new ChooseCrate(this, botSensor);
            var goToCrate = new GoToCrate(this, navMeshAgent, botAnimation, botSensor);
            var destroyCrate = new DestroyCrate(this, _weapon);

            var chooseBoost = new ChooseBoost(this, botSensor);
            var goToBoost = new CollectBoost(this, navMeshAgent, botAnimation, botSensor);

            var chooseEnemy = new ChooseEnemy(this, botSensor);
            var goToEnemy = new GoToEnemy(this, navMeshAgent, botAnimation, botSensor);
            var killEnemy = new KillEnemy(this, navMeshAgent, botAnimation, _weapon, _ultimate, _strafeLength, botSensor.AttackRange - StrafeDistanceFromTargetOffset);

            // Transitions
            If(chooseRandomPoint, RandomPointChosen(), goToRandomPoint);
            If(goToRandomPoint, SomeEnemyIsNear(), chooseEnemy);
            If(goToRandomPoint, SomeBoostIsNear(), chooseBoost);
            If(goToRandomPoint, SomeCrateIsNear(), chooseCrate);
            If(goToRandomPoint, ReachedRandomPoint(), chooseRandomPoint);

            If(chooseCrate, CrateChosen(), goToCrate);
            If(goToCrate, SomeEnemyIsNear(), chooseEnemy);
            If(goToCrate, SomeBoostIsNear(), chooseBoost);
            If(goToCrate, CanDestroyCrate(), destroyCrate);
            If(goToCrate, CrateIsNotNear(), chooseRandomPoint);
            If(goToCrate, CrateDestroyed(), chooseRandomPoint);
            If(destroyCrate, SomeEnemyIsNear(), chooseEnemy);
            If(destroyCrate, SomeBoostIsNear(), chooseBoost);
            If(destroyCrate, CrateDestroyed(), chooseRandomPoint);
            If(destroyCrate, CannotDestroyCrate(), goToCrate);

            If(chooseBoost, BoostChosen(), goToBoost);
            If(goToBoost, SomeEnemyIsNear(), chooseEnemy);
            If(goToBoost, BoostCollected(), chooseRandomPoint);
            If(goToBoost, BoostIsNotNear(), chooseRandomPoint);

            If(chooseEnemy, EnemyChosen(), goToEnemy);
            If(goToEnemy, CloserEnemyFound(), chooseEnemy);
            If(goToEnemy, CanKillEnemy(), killEnemy);
            If(goToEnemy, EnemyIsNotNear(), chooseRandomPoint);
            If(goToEnemy, EnemyKilled(), chooseRandomPoint);
            If(killEnemy, CloserEnemyFound(), chooseEnemy);
            If(killEnemy, EnemyKilled(), chooseRandomPoint);
            If(killEnemy, CannotKillEnemy(), goToEnemy);

            // Initial state
            _stateMachine.SetState(chooseRandomPoint);

            // Conditions
            Func<bool> RandomPointChosen() => () => RandomPoint.HasValue;
            Func<bool> ReachedRandomPoint() => () => Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(RandomPoint!.Value.x, RandomPoint!.Value.z)) <= 1;

            Func<bool> SomeCrateIsNear() => () => botSensor.IsAnyCrateInDetectionRange;
            Func<bool> CrateIsNotNear() => () => Crate != null && !botSensor.IsInDetectionRange(Crate);
            Func<bool> CrateChosen() => () => Crate != null;
            Func<bool> CanDestroyCrate() => () => Crate != null && botSensor.IsInAttackRange(Crate) && botSensor.IsVisible(Crate);
            Func<bool> CannotDestroyCrate() => () => Crate != null && !botSensor.IsInAttackRange(Crate) || !botSensor.IsVisible(Crate);
            Func<bool> CrateDestroyed() => () => Crate == null;

            Func<bool> SomeBoostIsNear() => () => botSensor.IsAnyBoostInDetectionRange;
            Func<bool> BoostIsNotNear() => () => Boost != null && !botSensor.IsInDetectionRange(Boost);
            Func<bool> BoostChosen() => () => Boost != null;
            Func<bool> BoostCollected() => () => Boost == null;

            Func<bool> SomeEnemyIsNear() => () => botSensor.IsAnyEnemyInDetectionRange;
            Func<bool> EnemyIsNotNear() => () => Enemy != null && !botSensor.IsInDetectionRange(Enemy);
            Func<bool> EnemyChosen() => () => Enemy != null;
            Func<bool> CanKillEnemy() => () => Enemy != null && botSensor.IsInAttackRange(Enemy) && botSensor.IsVisible(Enemy);
            Func<bool> CannotKillEnemy() => () => Enemy != null && !botSensor.IsInAttackRange(Enemy) || !botSensor.IsVisible(Enemy);
            Func<bool> EnemyKilled() => () => Enemy == null;

            Func<bool> CloserEnemyFound() => () =>
            {
                var closerEnemy = botSensor.ClosestEnemyInDetectionRange;
                return Enemy != null && closerEnemy != null && closerEnemy != Enemy;
            };
        }
        
        private void If(IState from, Func<bool> condition, IState to) => _stateMachine.AddTransition(from, to, condition);

        private void If(Func<bool> condition, IState to) => _stateMachine.AddAnyTransition(to, condition);

        private void Update() => _stateMachine.Tick();

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = _randomPointRange.MaxColor;
            Gizmos.DrawWireSphere(transform.position, _randomPointRange.Max);

            Gizmos.color = _randomPointRange.MinColor;
            Gizmos.DrawWireSphere(transform.position, _randomPointRange.Min);
        }
    }
}