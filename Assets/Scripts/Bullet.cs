using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _lifeTime = 10f;
    
    private Vector3 _oldPosition;
    
    
    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        _oldPosition = transform.position;
        
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        
        Debug.DrawLine(_oldPosition, transform.position, Color.red);
        
        if (Physics.Linecast(_oldPosition, transform.position, out RaycastHit hit))
        {
            Destroy(gameObject);
        }
    }
}
