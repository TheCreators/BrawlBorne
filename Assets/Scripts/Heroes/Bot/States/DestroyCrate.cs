using Combat.Weapons;
using Misc.StateMachine;
using UnityEngine;

namespace Heroes.Bot.States
{
    public class DestroyCrate : IState
    {
        private readonly Bot _bot;
        private readonly Weapon _weapon;
        
        public DestroyCrate(Bot bot, Weapon weapon)
        {
            _bot = bot;
            _weapon = weapon;
        }


        public void Tick()
        {
            Vector3 lookDirection = _bot.Crate.transform.position - _bot.transform.position;
            _bot.transform.rotation = Quaternion.LookRotation(lookDirection);
            
            _weapon.TryUse();
        }

        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }
    }
}