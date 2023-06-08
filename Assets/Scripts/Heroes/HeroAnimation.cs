using Misc;
using UnityEngine;

namespace Heroes
{
    [RequireComponent(typeof(Animator))]
    public abstract class HeroAnimation : MonoBehaviour
    {
        protected Animator Animator;

        protected static readonly int ForwardSpeedHash = Animator.StringToHash("Forward Speed");
        protected static readonly int RightSpeedHash = Animator.StringToHash("Right Speed");
        protected static readonly int IsGroundedHash = Animator.StringToHash("Is Grounded");
        protected static readonly int IsJumpingHash = Animator.StringToHash("Is Jumping");
        protected static readonly int IsMovingHash = Animator.StringToHash("Is Moving");

        protected virtual void Awake()
        {
            Animator = this.GetComponentWithNullCheck<Animator>();
        }
    }
}