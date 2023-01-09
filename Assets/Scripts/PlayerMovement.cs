using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private float _speed = 12f;

    private Vector2 _moveDirection;

    private void Update()
    {
        Move(_moveDirection);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
    }

    private void Move(Vector2 direction)
    {
        Vector3 move = transform.right * direction.x + transform.forward * direction.y;
        _controller.Move(move * (_speed * Time.deltaTime));
    }
}
