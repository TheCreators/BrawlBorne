using System;
using UnityEngine;

namespace Environment
{
    public class Crate : MonoBehaviour
    {
        [SerializeField] private GameObject _boost;
        private void OnDestroy()
        {
            if (gameObject.scene.isLoaded is false) return;
            Instantiate(_boost, transform.position, transform.rotation);
        }
    }
}
