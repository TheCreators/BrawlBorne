using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(fileName = "GameEvent")]
    public class GameEvent : ScriptableObject
    {
        private List<GameEventListener> Listeners { get; set; } = new ();
        
        public void Raise(Component component, object data)
        {
            foreach (var listener in Listeners.ToList())
            {
                listener.OnEventRaised(component, data);
            }
        }

        public void RegisterListener(GameEventListener listener)
        {
            if (!Listeners.Contains(listener))
            {
                Listeners.Add(listener);
            }
        }
        
        public void UnregisterListener(GameEventListener listener)
        {
            if (Listeners.Contains(listener))
            {
                Listeners.Remove(listener);
            }
        }
    }
}