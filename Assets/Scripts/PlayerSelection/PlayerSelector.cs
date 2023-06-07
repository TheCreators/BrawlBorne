using System.Collections;
using System.Collections.Generic;
using Events;
using Misc;
using Models;
using NaughtyAttributes;
using Sound;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayerSelection
{
    public class PlayerSelector : MonoBehaviour
    {
        [SerializeField] [BoxGroup(Group.Prefabs)]
        private List<PlayerDummy> _players;

        [SerializeField] [BoxGroup(Group.Settings)]
        private float _shiftDistance = 2f;

        [SerializeField] [BoxGroup(Group.Settings)]
        private float _shiftSpeed = 2f;

        [SerializeField] [BoxGroup(Group.Settings)] [Required]
        private GameEvent _onPlayerNameChanged;
        
        [SerializeField] [BoxGroup(Group.Settings)] [Range(0, 5)]
        private float _delayBeforeLoading = 2f;

        [SerializeField] [BoxGroup(Group.Sounds)] [Range(0, 100)]
        private int _musicVolumeDecreaseAmount = 10;
        
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
            _players[_selectedPlayerIndex].PlayMusicTheme();
        }

        public void SelectPlayer()
        {
            MixerManager.Instance.Music.Volume -= _musicVolumeDecreaseAmount;
            _players[_selectedPlayerIndex].PlaySelectedSound();
            
            Invoke(nameof(ChangeScene), _delayBeforeLoading);
        }
        
        private void ChangeScene()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            _loadingScreen.SetActive(true);
            SceneManager.LoadScene(_gameScene);
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            MixerManager.Instance.Music.ResetVolume();
            ObjectsPool objectsPool = FindObjectOfType<ObjectsPool>();
            if (objectsPool != null)
            {
                objectsPool.SetPlayerPrefab(_players[_selectedPlayerIndex].PlayerPrefab);
            }

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void SelectNextPlayer()
        {
            if (_isMoving || _selectedPlayerIndex >= _players.Count - 1)
            {
                return;
            }

            _players[_selectedPlayerIndex].StopMusicTheme();
            _selectedPlayerIndex++;
            _onPlayerNameChanged.Raise(this, _players[_selectedPlayerIndex].Name);
            _players[_selectedPlayerIndex].PlayMusicTheme();
            Shift(true);
        }

        public void SelectPreviousPlayer()
        {
            if (_isMoving || _selectedPlayerIndex <= 0)
            {
                return;
            }

            _players[_selectedPlayerIndex].StopMusicTheme();
            _selectedPlayerIndex--;
            _onPlayerNameChanged.Raise(this, _players[_selectedPlayerIndex].Name);
            _players[_selectedPlayerIndex].PlayMusicTheme();
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
                _players[i].Instantiate(
                    _transform.position - _transform.right * _shiftDistance * i,
                    _transform.rotation,
                    _transform);
            }
        }
    }
}