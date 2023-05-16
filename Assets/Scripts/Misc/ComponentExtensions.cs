using UnityEngine;

namespace Misc
{
    public static class ComponentExtensions
    {
        public static void CheckIfNull<T>(this Component self, T componentToCheck)
        {
            if (componentToCheck == null && self.gameObject.scene.name != null)
            {
                Debug.LogError($"{typeof(T).Name} is null for {self.GetType().Name} on GameObject: {self.gameObject.name} " +
                               $"(Path: {self.gameObject.transform.GetHierarchyPath()}, Position: {self.gameObject.transform.position})");
            }
        }
        
        public static void CheckIfNull<T>(this Component self, params T[] objectsToCheck)
        {
            if (self.gameObject.scene.name == null)
            {
                return;
            }
            
            foreach (var obj in objectsToCheck)
            {
                if (obj == null)
                {
                    Debug.LogError($"{typeof(T).Name} is null for {self.GetType().Name} on GameObject: {self.gameObject.name} " +
                                   $"(Path: {self.gameObject.transform.GetHierarchyPath()}, Position: {self.gameObject.transform.position})");
                }
            }
        }
        
        public static T GetComponentWithNullCheck<T>(this Component self) where T : Component
        {
            var component = self.GetComponent<T>();
            if (component == null)
            {
                Debug.LogError($"{typeof(T).Name} is null for {self.GetType().Name} on GameObject: {self.gameObject.name} " +
                               $"(Path: {self.gameObject.transform.GetHierarchyPath()}, Position: {self.gameObject.transform.position})");
            }
            return component;
        }
        
        public static T GetComponentInParentWithNullCheck<T>(this Component self) where T : Component
        {
            var component = self.GetComponentInParent<T>();
            if (component == null)
            {
                Debug.LogError($"{typeof(T).Name} is null for {self.GetType().Name} on GameObject: {self.gameObject.name} " +
                               $"(Path: {self.gameObject.transform.GetHierarchyPath()}, Position: {self.gameObject.transform.position})");
            }
            return component;
        }

    }
}