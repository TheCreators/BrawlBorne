using UnityEngine;
using UnityEngine.AI;

namespace Battlefield
{
    public static class NavMeshSurfaceExtensions
    {
        public static AsyncOperation BuildNavMeshAsync(this NavMeshSurface surface)
        {
            surface.RemoveData();
            surface.navMeshData = new NavMeshData(surface.agentTypeID)
            {
                name = surface.gameObject.name,
                position = surface.transform.position,
                rotation = surface.transform.rotation
            };

            if (surface.isActiveAndEnabled)
                surface.AddData();

            return surface.UpdateNavMesh(surface.navMeshData);
        }
    }
}