using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(GroundChecker))]
public class PlayerJump : MonoBehaviour
{
    [SerializeField] [Range(0, 5)] private float _gravityMultiplier = 1;
    [SerializeField] [Range(0, 50)] private float _jumpPower = 3f;

    private CharacterController _controller;
    private GroundChecker _groundChecker;

    private Vector3 _direction;
    private float _velocity;
    private const float Gravity = -9.81f;
    private bool _jumpHeld;
    public bool IsJumping { get; private set; }

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _groundChecker = GetComponent<GroundChecker>();
    }

    private void Update()
    {
        ApplyGravity();
        ApplyMovement();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _jumpHeld = true;
        }

        if (context.canceled)
        {
            _jumpHeld = false;
        }
    }

    private void ApplyGravity()
    {
        if (_groundChecker.IsGrounded && _velocity < 0.0f)
        {
            _velocity = -1.0f;
            IsJumping = false;
        }
        else
        {
            _velocity += Gravity * _gravityMultiplier * Time.deltaTime;
            IsJumping = true;
        }

        _direction.y = _velocity;

        if (_groundChecker.IsGrounded && _jumpHeld)
        {
            _velocity = Mathf.Sqrt(_jumpPower * -2.0f * Gravity);
        }
    }

    private void ApplyMovement()
    {
        _controller.Move(_direction * Time.deltaTime);
    }
}