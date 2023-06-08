using Misc;
using NaughtyAttributes;
using UnityEngine;

namespace Environment
{
    public class Crate : MonoBehaviour
    {
        [SerializeField] [Required] [ShowAssetPreview]
        private Boost _boost;

        private void OnValidate()
        {
            this.CheckIfNull(_boost);
        }

        public void OnDie(Component component, object data)
        {
            if (component.gameObject != gameObject || gameObject.scene.isLoaded is false)
            {
                return;
            }

            ObjectsPool.Instance.AddBoost(Instantiate(_boost, transform.position, transform.rotation));
        }
    }
}