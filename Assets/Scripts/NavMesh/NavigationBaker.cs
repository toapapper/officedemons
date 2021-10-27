using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// <para>
/// Enables navmesh baking during runtime.
/// </para>
///  <para>
///  Author: Kristian
/// </para>
/// </summary>

// Last Edited: 22-10-2021
public class NavigationBaker : MonoBehaviour
{
    List<NavMeshSurface> surfaces = new List<NavMeshSurface>();

    /// <summary>
    /// Adds all NavMeshSurfaces to a list and bakes them.
    /// </summary>
    public void BakeNavMesh()
    {
        NavMeshSurface navSurface = GetComponentInChildren<NavMeshSurface>();
        surfaces.Add(navSurface);
        //Debug.Log("Surfaces count: " + surfaces.Count);
        for (int i = 0; i < surfaces.Count; i++)
        {
            surfaces[i].BuildNavMesh();
        }
    }
}