using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] [Scene]
        private int _playerSelection;
        
        [SerializeField] [Scene]
        private int _mainMenu;
        
        public void GoToPlayerSelection()
        {
            SceneManager.LoadScene(_playerSelection);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene(_mainMenu);
        }
    }
}