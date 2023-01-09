using UnityEngine;
using UnityEngine.InputSystem;

public class MouseRotation : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 1000f;
    [SerializeField] private Transform _playerBody;

    private Quaternion _rotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>();

        float mouseX = mouseDelta.x * _sensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * _sensitivity * Time.deltaTime;

        _rotation.x -= mouseY;
        _rotation.x = Mathf.Clamp(_rotation.x, -90f, 90f); // clamp the rotation to prevent the camera from flipping upside down

        transform.localRotation = Quaternion.Euler(_rotation.x, 0f, 0f);
        _playerBody.Rotate(Vector3.up * mouseX);
    }
}
