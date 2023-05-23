using Misc;
using UnityEngine;

namespace Heroes.Bot
{
    [RequireComponent(typeof(BotMovement))]
    public class BotAnimationStateController : HeroAnimationStateController
    {
        private BotMovement _botMovement;

        protected override void Awake()
        {
            _botMovement = this.GetComponentWithNullCheck<BotMovement>();

            base.Awake();
        }

        private void Update()
        {
            Animator.SetFloat(VelocityX, _botMovement.GetNormalizedRelativeVelocity().x);
            Animator.SetFloat(VelocityZ, _botMovement.GetNormalizedRelativeVelocity().y);
            Animator.SetBool(IsMoving, _botMovement.IsMoving);
        }
    }
}