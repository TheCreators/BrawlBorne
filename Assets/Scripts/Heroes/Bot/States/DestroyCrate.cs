using System.Collections.Generic;
using Combat;
using Combat.Weapons;
using Heroes.Bot.AimingStrategies;
using Misc.StateMachine;
using UnityEngine;

namespace Heroes.Bot.States
{
    public class DestroyCrate : IState
    {
        private readonly Bot _bot;
        private readonly Usable _weapon;
        private readonly AimingStrategy _aimingStrategy;
        
        public DestroyCrate(Bot bot, Usable weapon, AimingStrategy aimingStrategy)
        {
            _bot = bot;
            _weapon = weapon;
            _aimingStrategy = aimingStrategy;
        }

        private IEnumerator<Quaternion> GetAimRotations()
        {
            while (_bot.Crate != null)
            {
                yield return _aimingStrategy.GetAimRotation(_bot.ShootFrom, _bot.Crate.transform.position, _weapon);
            }
        }

        public void Tick()
        {
            Vector3 lookDirection = _bot.Crate.transform.position - _bot.transform.position;
            _bot.transform.rotation = Quaternion.LookRotation(lookDirection);
            
            _weapon.TryUse(GetAimRotations());
        }

        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }
    }
}