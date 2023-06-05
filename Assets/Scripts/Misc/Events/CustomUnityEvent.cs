using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    [System.Serializable]
    public class CustomUnityEvent : UnityEvent<Component, object>
    {
    }
}