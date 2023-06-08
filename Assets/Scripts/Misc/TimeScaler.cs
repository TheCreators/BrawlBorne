using System;
using UnityEngine;

namespace Misc
{
    public class TimeScaler : MonoBehaviour
    {
        [SerializeField] [Range(0, 10)]
        private float _timeScale = 1.0f;

        private void Start()
        {
            Time.timeScale = _timeScale;
        }

        private void Update()
        {
            Time.timeScale = _timeScale;
        }
    }
}