using UnityEngine;
using UnityEngine.InputSystem;
public class CharacterMovement : MonoBehaviour
{
    private Vector2 _moveInput;
    private Rigidbody _rigidBody;
    
    [SerializeField] private float _moveSpeed;
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate()
    {
        TryMove();
    }
    
    private void TryMove()
    {
        var velocity = new Vector3(_moveInput.x * _moveSpeed, _rigidBody.velocity.y, _moveInput.y * _moveSpeed);
        _rigidBody.velocity = velocity;
    }

    
    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }
}
