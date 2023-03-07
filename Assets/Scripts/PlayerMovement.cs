using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private CharacterController _controller;

    [Header("Settings")]
    [SerializeField] [Range(0, 50)] private float _normalSpeed = 12f;
    [SerializeField] [Range(0, 50)] private float _shiftSpeed = 6f;
    [SerializeField] [Range(0, 0.5f)] private float _moveDirectionSmoothingSpeed = 0.1f;
    [SerializeField] [Range(0, 0.5f)] private float _speedSmoothingSpeed = 0.1f;

    private Vector2 _inputMoveDirection;
    private Vector2 _smoothMoveDirection;
    private Vector2 _smoothMoveVelocity;
    
    private float _speed;
    private float _smoothSpeed;
    private float _smoothSpeedVelocity;
    

    private void Awake()
    {
        _speed = _normalSpeed;
        _smoothSpeed = _normalSpeed;
    }

    private void Update()
    {
        SmoothValues();
        Move();
    }

    public Vector2 NormalizedSmoothMoveDirectionWithSmoothSpeed => _smoothMoveDirection * _smoothSpeed / _normalSpeed;

    public void OnMove(InputAction.CallbackContext context)
    {
        _inputMoveDirection = context.ReadValue<Vector2>();
    }

    public void OnShift(InputAction.CallbackContext context)
    {
        _speed = context.performed ? _shiftSpeed : _normalSpeed;
    }
    
    private void SmoothValues()
    {
        _smoothMoveDirection = Vector2.SmoothDamp(_smoothMoveDirection, _inputMoveDirection, ref _smoothMoveVelocity, _moveDirectionSmoothingSpeed);
        _smoothSpeed = Mathf.SmoothDamp(_smoothSpeed, _speed, ref _smoothSpeedVelocity, _speedSmoothingSpeed);
    }

    private void Move()
    {
        var move = transform.right * _smoothMoveDirection.x + transform.forward * _smoothMoveDirection.y;
        _controller.Move(move * _smoothSpeed * Time.deltaTime);
    }
}
