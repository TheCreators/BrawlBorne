using Misc;
using NaughtyAttributes;
using UnityEngine;

namespace PlayerSelection
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] [Required]
        private Transform _startPosition;

        [SerializeField] [Required]
        private Transform _endPosition;

        [SerializeField] [Min(0)]
        private float _duration = 2f;

        [SerializeField] [Min(0)]
        private float _delay = 1f;

        [SerializeField] [Required]
        private Canvas _canvas;

        private float _elapsedTime;
        private bool _isDelayComplete;

        private void OnValidate()
        {
            this.CheckIfNull(_startPosition, _endPosition);
            this.CheckIfNull(_canvas);
        }

        private void Start()
        {
            // Set the starting position and rotation of the camera
            transform.position = _startPosition.position;
            transform.rotation = _startPosition.rotation;
            _isDelayComplete = false;
            _canvas.enabled = false;
        }

        private void Update()
        {
            if (!_isDelayComplete)
            {
                // Wait for the delay to complete
                if (_elapsedTime < _delay)
                {
                    _elapsedTime += Time.deltaTime;
                    return;
                }

                // Delay completed, proceed with movement
                _isDelayComplete = true;
                _elapsedTime = 0f;
            }

            if (_isDelayComplete)
            {
                _elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(_elapsedTime / _duration);
                float progress = Mathf.SmoothStep(0f, 1f, t);
                Vector3 currentPos = Vector3.Lerp(_startPosition.position, _endPosition.position, progress);
                Quaternion currentRot = Quaternion.Lerp(_startPosition.rotation, _endPosition.rotation, progress);

                transform.position = currentPos;
                transform.rotation = currentRot;

                if (progress >= 1.0f)
                {
                    _canvas.enabled = true;
                    enabled = false;
                }
            }
        }
    }
}