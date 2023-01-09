using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private Transform _groundCheck;

    [Header("Settings")]
    [SerializeField] private float _groundDistance = 0.4f;
    [SerializeField] private LayerMask _groundMask;

    public bool IsGrounded => Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);
}
