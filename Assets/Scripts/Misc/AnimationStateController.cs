using UnityEngine;

namespace Misc
{
    [RequireComponent(typeof(Animator))]
    public abstract class AnimationStateController : MonoBehaviour
    {
        protected Animator Animator;

        protected static readonly int VelocityX = Animator.StringToHash("Velocity X");
        protected static readonly int VelocityZ = Animator.StringToHash("Velocity Z");
        protected static readonly int IsGrounded = Animator.StringToHash("Is Grounded");
        protected static readonly int IsJumping = Animator.StringToHash("Is Jumping");
        protected static readonly int IsMoving = Animator.StringToHash("Is Moving");

        protected virtual void Awake()
        {
            Animator = this.GetComponentWithNullCheck<Animator>();
        }
    }
}
