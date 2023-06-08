using Unity.AI.Navigation;
using UnityEngine;

namespace Misc.Validators
{
    public static class Has
    {
        public static bool NavMeshSurface(GameObject targetGameObject)
        {
            return HasComponent<NavMeshSurface>(targetGameObject);
        }
        
        private static bool HasComponent<T>(GameObject targetGameObject) where T : Component
        {
            return targetGameObject.TryGetComponent(out T _);
        }
    }
}