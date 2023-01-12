using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DashAbility : MonoBehaviour
{
    [Header("Requirements")] 
    [SerializeField] private CharacterController _controller;

    [Header("Settings")] 
    [SerializeField] [Range(0, 50)] private float _dashSpeed = 10f;
    [SerializeField] [Range(0, 10)] private float _dashDuration = 0.5f;
    
    private bool _isDashing;

    public void Update()
    {
        if (_isDashing)
        {
            Dash();
        }
    }

    public void OnAbility(InputAction.CallbackContext context)
    {
        if (context.performed is false)
        {
            return;
        }

        _isDashing = true;
        StartCoroutine(StopDashingAfter(_dashDuration));
    }
    
    private IEnumerator StopDashingAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _isDashing = false;
    }

    private void Dash()
    {
        _controller.Move(transform.forward * _dashSpeed * Time.deltaTime);
    }
}
