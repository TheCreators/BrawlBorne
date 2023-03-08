using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private Transform _groundCheck;

    [Header("Settings")]
    [SerializeField] [Range(0, 5)] private float _checkRadius = 0.4f;
    [SerializeField] private LayerMask _groundMask;

    public bool IsGrounded => Physics.CheckBox(_groundCheck.position, Vector3.one * _checkRadius, Quaternion.identity, _groundMask);

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawWireCube(_groundCheck.position, Vector3.one * _checkRadius);
    }
}
