using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private CharacterController _controller;

    [Header("Settings")]
    [SerializeField] [Range(-50, 0)] private float _gravity = -9.81f;
    [SerializeField] [Range(0, 10)] private float _jumpHeight = 3f;

    private Vector3 _velocity;
    private bool _isJumping;

    private void Update()
    {
        if (_controller.isGrounded)
        {
            if (_velocity.y < 0)
            {
                ResetVelocity();
            }

            if (_isJumping)
            {
                ApplyJumpVelocity();
            }
        }

        ApplyGravity();

        _controller.Move(_velocity * Time.deltaTime);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        _isJumping = context.performed;
    }

    private void ResetVelocity()
    {
        _velocity.y = -2f;
    }

    private void ApplyGravity()
    {
        _velocity.y += _gravity * Time.deltaTime;
    }

    private void ApplyJumpVelocity()
    {
        _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
    }
}
