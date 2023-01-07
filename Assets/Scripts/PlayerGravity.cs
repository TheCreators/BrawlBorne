using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private GroundChecker _groundChecker;
    
    private Vector3 _velocity;

    private void Update()
    {
        if (_groundChecker.IsGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
        
        _velocity.y += _gravity * Time.deltaTime;
        
        _controller.Move(_velocity * Time.deltaTime);
    }
}
