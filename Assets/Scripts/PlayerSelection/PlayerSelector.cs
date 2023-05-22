using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using Heroes.Player;
using Misc;
using Models;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace PlayerSelection
{
    public class PlayerSelector : MonoBehaviour
    {
        [SerializeField] [BoxGroup(Group.Prefabs)]
        private List<Player> _players;

        [SerializeField] [BoxGroup(Group.Settings)]
        private float _shiftDistance = 2f;

        [SerializeField] [BoxGroup(Group.Settings)]
        private float _shiftSpeed = 2f;

        [SerializeField] [BoxGroup(Group.Settings)]
        private GameEvent _onPlayerNameChanged;

        [SerializeField] [BoxGroup(Group.SceneLoading)]
        private GameObject _loadingScreen;

        [SerializeField] [BoxGroup(Group.SceneLoading)] [Scene]
        private int _gameScene;

        private int _selectedPlayerIndex;
        private bool _isMoving;
        private Transform _transform;

        private void OnValidate()
        {
            this.CheckIfNull(_onPlayerNameChanged);
        }

        private void Awake()
        {
            _transform = transform;
        }

        private void Start()
        {
            InstantiatePlayers();
            _onPlayerNameChanged.Raise(this, _players[_selectedPlayerIndex].Name);
        }

        public void SelectPlayer()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            _loadingScreen.SetActive(true);
            SceneManager.LoadScene(_gameScene);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            ObjectsPool objectsPool = FindObjectOfType<ObjectsPool>();
            if (objectsPool != null)
            {
                objectsPool.SetPlayerPrefab(_players[_selectedPlayerIndex]);
            }

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void SelectNextPlayer()
        {
            if (_isMoving || _selectedPlayerIndex >= _players.Count - 1)
            {
                return;
            }

            _selectedPlayerIndex++;
            _onPlayerNameChanged.Raise(this, _players[_selectedPlayerIndex].Name);
            Shift(true);
        }

        public void SelectPreviousPlayer()
        {
            if (_isMoving || _selectedPlayerIndex <= 0)
            {
                return;
            }

            _selectedPlayerIndex--;
            _onPlayerNameChanged.Raise(this, _players[_selectedPlayerIndex].Name);
            Shift(false);
        }

        private void Shift(bool toRight)
        {
            Vector3 targetPosition = _transform.position + _transform.right * _shiftDistance * (toRight ? 1 : -1);
            StartCoroutine(MoveSmoothly(targetPosition, _shiftSpeed));
        }

        private IEnumerator MoveSmoothly(Vector3 targetPosition, float speed)
        {
            _isMoving = true;

            float elapsedTime = 0f;
            Vector3 startingPosition = _transform.position;

            while (elapsedTime < speed)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / speed);
                float progress = Mathf.SmoothStep(0f, 1f, t);
                _transform.position = Vector3.Lerp(startingPosition, targetPosition, progress);
                yield return null;
            }

            _transform.position = targetPosition;

            _isMoving = false;
        }

        private void InstantiatePlayers()
        {
            for (int i = 0; i < _players.Count; i++)
            {
                Player player = Instantiate(
                    _players[i],
                    _transform.position - _transform.right * _shiftDistance * i,
                    _transform.rotation,
                    _transform);

                TurnPlayerDown(player);
            }
        }

        private void TurnPlayerDown(Player player)
        {
            Camera playerCamera = player.GetComponentInChildren<Camera>();
            if (playerCamera != null)
            {
                playerCamera.enabled = false;

                if (playerCamera.TryGetComponent(out MouseRotation mouseRotation))
                {
                    mouseRotation.enabled = false;
                }

                if (playerCamera.TryGetComponent(out AudioListener audioListener))
                {
                    audioListener.enabled = false;
                }
            }

            if (player.TryGetComponent(out PlayerInput playerInput))
            {
                playerInput.enabled = false;
            }

            if (player.TryGetComponent(out AudioSource audioSource))
            {
                audioSource.mute = true;
            }

            if (player.TryGetComponent(out Rigidbody playerRigidbody))
            {
                playerRigidbody.interpolation = RigidbodyInterpolation.None;
            }
        }
    }
}