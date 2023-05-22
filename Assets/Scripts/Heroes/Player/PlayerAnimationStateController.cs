using Misc;
using UnityEngine;

namespace Heroes.Player
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerJump))]
    [RequireComponent(typeof(GroundChecker))]
    public class PlayerAnimationStateController : HeroAnimationStateController
    {
        private PlayerMovement _playerMovement;
        private PlayerJump _playerJump;
        private GroundChecker _groundChecker;
        
        protected override void Awake()
        {
            _playerMovement = this.GetComponentWithNullCheck<PlayerMovement>();
            _playerJump = this.GetComponentWithNullCheck<PlayerJump>();
            _groundChecker = this.GetComponentWithNullCheck<GroundChecker>();
            
            base.Awake();
        }
        
        private void Update()
        {
            Animator.SetFloat(VelocityX, _playerMovement.GetNormalizedRelativeVelocity().x);
            Animator.SetFloat(VelocityZ, _playerMovement.GetNormalizedRelativeVelocity().y);
            Animator.SetBool(IsGrounded, _groundChecker.IsGrounded);
            Animator.SetBool(IsJumping, _playerJump.IsJumping);
            Animator.SetBool(IsMoving, _playerMovement.IsMoving);
        }
    }
}