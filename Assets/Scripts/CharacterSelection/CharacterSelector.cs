using System.Collections;
using System.Collections.Generic;
using Events;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Misc
{
    public class CharacterSelector : MonoBehaviour
    {
        [SerializeField] private List<Hero> _characters;
        [SerializeField] private float _shiftDistance = 2f;
        [SerializeField] private float _shiftSpeed = 2f;
        [SerializeField] private GameEvent _onCharacterNameChanged;

        private int _selectedCharacterIndex = 0;
        private bool _isMoving = false;

        private void Start()
        {
            InstantiateCharacters();
            _onCharacterNameChanged.Raise(this, _characters[_selectedCharacterIndex].Name);
        }
        
        public void SelectCharacter()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            HeroesPool heroesPool = FindObjectOfType<HeroesPool>();
            if (heroesPool != null)
            {
                heroesPool.SetPlayerPrefab(_characters[_selectedCharacterIndex]);
            }

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void SelectNextCharacter()
        {
            if (_isMoving || _selectedCharacterIndex >= _characters.Count - 1)
            {
                return;
            }

            _selectedCharacterIndex++;
            _onCharacterNameChanged.Raise(this, _characters[_selectedCharacterIndex].Name);
            Shift(true);
        }
        
        public void SelectPreviousCharacter()
        {
            if (_isMoving || _selectedCharacterIndex <= 0)
            {
                return;
            }

            _selectedCharacterIndex--;
            _onCharacterNameChanged.Raise(this, _characters[_selectedCharacterIndex].Name);
            Shift(false);
        }

        private void Shift(bool toRight)
        {
            Vector3 targetPosition = transform.position + transform.right * _shiftDistance * (toRight ? 1 : -1);
            StartCoroutine(MoveCharacter(transform, targetPosition, _shiftSpeed));
        }

        private IEnumerator MoveCharacter(Transform characterTransform, Vector3 targetPosition, float speed)
        {
            _isMoving = true;

            float elapsedTime = 0f;
            Vector3 startingPosition = characterTransform.position;

            while (elapsedTime < speed)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / speed);
                float progress = Mathf.SmoothStep(0f, 1f, t);
                characterTransform.position = Vector3.Lerp(startingPosition, targetPosition, progress);
                yield return null;
            }

            characterTransform.position = targetPosition;

            _isMoving = false;
        }

        private void InstantiateCharacters()
        {
            for (int i = 0; i < _characters.Count; i++)
            {
                Hero character = Instantiate(_characters[i], transform);
                character.transform.SetParent(transform);
                character.transform.position -= transform.right * _shiftDistance * i;

                Camera characterCamera = character.GetComponentInChildren<Camera>();
                if (characterCamera != null)
                {
                    characterCamera.enabled = false;
                    
                    if (characterCamera.TryGetComponent(out MouseRotation mouseRotation))
                    {
                        mouseRotation.enabled = false;
                    }

                    if (characterCamera.TryGetComponent(out AudioListener audioListener))
                    {
                        audioListener.enabled = false;
                    }
                }
                
                if (character.TryGetComponent(out PlayerInput playerInput))
                {
                    playerInput.enabled = false;
                }
            }
        }
    }
}
