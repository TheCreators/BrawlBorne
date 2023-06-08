using UnityEngine;

namespace Misc
{
    public static class ComponentExtensions
    {
        public static void CheckIfNull<T>(this Component self, T componentToCheck)
        {
            if (self.gameObject.scene.name == null || componentToCheck != null)
            {
                return;
            }

            LogNullError(typeof(T).Name, self);
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
                    LogNullError(typeof(T).Name, self);
                }
            }
        }

        public static T GetComponentWithNullCheck<T>(this Component self) where T : Component
        {
            var component = self.GetComponent<T>();
            
            if (component == null)
            {
                LogNullError(typeof(T).Name, self);
            }

            return component;
        }

        public static T GetComponentInParentWithNullCheck<T>(this Component self) where T : Component
        {
            var component = self.GetComponentInParent<T>();
            
            if (component == null)
            {
                LogNullError(typeof(T).Name, self);
            }

            return component;
        }

        private static void LogNullError(string type, Component component)
        {
            GameObject componentGameObject = component.gameObject;
            Debug.LogError($"{type} is null for {component.GetType().Name} on GameObject: {componentGameObject.name} " +
                           $"(Path: {component.gameObject.transform.GetHierarchyPath()}, Position: {componentGameObject.transform.position})");
        }
    }
}