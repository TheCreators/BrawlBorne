using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerJump))]
[RequireComponent(typeof(GroundChecker))]
public class AnimationStateController : MonoBehaviour
{
    private Animator _animator;
    private PlayerMovement _playerMovement;
    private PlayerJump _playerJump;
    private GroundChecker _groundChecker;
    
    private static readonly int VelocityX = Animator.StringToHash("Velocity X"); 
    private static readonly int VelocityZ = Animator.StringToHash("Velocity Z");
    private static readonly int IsGrounded = Animator.StringToHash("Is Grounded");
    private static readonly int IsJumping = Animator.StringToHash("Is Jumping");
    private static readonly int IsMoving = Animator.StringToHash("Is Moving");
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerJump = GetComponent<PlayerJump>();
        _groundChecker = GetComponent<GroundChecker>();
    }

    private void Update()
    {
        _animator.SetFloat(VelocityX, _playerMovement.NormalizedSmoothMoveDirectionWithSmoothSpeed.y);
        _animator.SetFloat(VelocityZ, _playerMovement.NormalizedSmoothMoveDirectionWithSmoothSpeed.x);
        _animator.SetBool(IsGrounded, _groundChecker.IsGrounded);
        _animator.SetBool(IsJumping, _playerJump.IsJumping);
        _animator.SetBool(IsMoving, _playerMovement.IsMoving);
    }
}
