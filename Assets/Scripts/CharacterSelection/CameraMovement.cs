using UnityEngine;

namespace CharacterSelection
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Transform _startPosition;
        [SerializeField] private Transform _endPosition;
        [SerializeField] private float _duration = 2f;
        [SerializeField] private float _delay = 1f;
        [SerializeField] private Canvas _canvas;

        private float _elapsedTime;
        private bool _isDelayComplete;

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