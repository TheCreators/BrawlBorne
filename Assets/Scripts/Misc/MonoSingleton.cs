using UnityEngine;


namespace Misc
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError(typeof(T) + " is missing.");
                }

                return _instance;
            }
        }


        private void Awake()
        {
            if (_instance != null)
            {
                Debug.LogWarning("Second instance of " + typeof(T) + " created. Automatic self-destruct triggered.");
                Destroy(gameObject);
            }

            _instance = this as T;

            Init();
        }


        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }


        protected virtual void Init()
        {
        }
    }
}