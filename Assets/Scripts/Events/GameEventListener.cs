using UnityEngine;

namespace Events
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] private GameEvent _gameEvent;
        [SerializeField] private CustomUnityEvent _response;
        
        private void OnEnable()
        {
            _gameEvent.RegisterListener(this);
        }
        
        private void OnDisable()
        {
            _gameEvent.UnregisterListener(this);
        }
        
        public void OnEventRaised(Component component, object data)
        {
            _response.Invoke(component, data);
        }
    }
}