using UnityEngine;

namespace Misc
{
    public static class TransformExtensions
    {
        public static string GetHierarchyPath(this Transform transform)
        {
            string path = transform.name;
            while (transform.parent != null)
            {
                transform = transform.parent;
                path = transform.name + "/" + path;
            }
            return path;
        }
    }
}