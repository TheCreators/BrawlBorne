using Misc;
using UnityEngine;

namespace Heroes.Player
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerJump))]
    [RequireComponent(typeof(GroundChecker))]
    public class PlayerAnimation : HeroAnimation
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
            Animator.SetFloat(ForwardSpeedHash, _playerMovement.GetNormalizedRelativeVelocity().x);
            Animator.SetFloat(RightSpeedHash, _playerMovement.GetNormalizedRelativeVelocity().y);
            Animator.SetBool(IsGroundedHash, _groundChecker.IsGrounded);
            Animator.SetBool(IsJumpingHash, _playerJump.IsJumping);
            Animator.SetBool(IsMovingHash, _playerMovement.IsMoving);
        }
    }
}