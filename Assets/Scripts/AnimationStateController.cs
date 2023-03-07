using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerMovement _playerMovement;
    
    private static readonly int VelocityX = Animator.StringToHash("Velocity X"); 
    private static readonly int VelocityZ = Animator.StringToHash("Velocity Z");

    private void Update()
    {
        _animator.SetFloat(VelocityX, _playerMovement.NormalizedSmoothMoveDirectionWithSmoothSpeed.y);
        _animator.SetFloat(VelocityZ, _playerMovement.NormalizedSmoothMoveDirectionWithSmoothSpeed.x);
    }
}
