using Misc;
using UnityEngine;

namespace Environment
{
    public class Crate : MonoBehaviour
    {
        [SerializeField] private Boost _boost;
        
        private void OnValidate()
        {
            this.CheckIfNull(_boost);
        }

        public void OnDie(Component component, object data)
        {
            if (component.gameObject != gameObject) return;
            if (gameObject.scene.isLoaded is false) return;
            Destroy(gameObject);
            Instantiate(_boost, transform.position, transform.rotation);
        }
    }
}
