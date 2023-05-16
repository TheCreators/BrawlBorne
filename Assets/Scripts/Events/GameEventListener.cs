using System;
using Misc;
using UnityEngine;

namespace Events
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] private GameEvent _gameEvent;
        [SerializeField] private CustomUnityEvent _response;
        
        private void OnValidate()
        {
            this.CheckIfNull(_gameEvent);
            this.CheckIfNull(_response);
        }

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
            try
            {
                _response.Invoke(component, data);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}