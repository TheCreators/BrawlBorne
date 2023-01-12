using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private CharacterController _controller;

    [Header("Settings")]
    [SerializeField] [Range(0, 50)] private float _normalSpeed = 12f;
    [SerializeField] [Range(0, 50)] private float _shiftSpeed = 6f;

    private float _speed;
    private Vector2 _moveDirection;
    
    private void Start()
    {
        _speed = _normalSpeed;
    }

    private void Update()
    {
        Move();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
    }

    public void OnShift(InputAction.CallbackContext context)
    {
        _speed = context.performed ? _shiftSpeed : _normalSpeed;
    }

    private void Move()
    {
        Vector3 move = transform.right * _moveDirection.x + transform.forward * _moveDirection.y;
        _controller.Move(move * (_speed * Time.deltaTime));
    }
}
