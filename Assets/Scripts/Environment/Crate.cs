using System;
using UnityEngine;

namespace Environment
{
    public class Crate : MonoBehaviour
    {
        [SerializeField] private GameObject _boost;
        private void OnDestroy()
        {
            Instantiate(_boost, transform.position, transform.rotation);
        }
    }
}
