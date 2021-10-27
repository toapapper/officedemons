using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// <para>
/// A Catalogue over actions the enemy can perform
/// 
/// </para>
///   
///  <para>
///  Author: Tinea Larsson, Tim Wennerberg
///  
/// </para>
///  
/// </summary>

// Last Edited: 15-10-21

public static class EnemyActions 
{
    public static void MoveTowards(NavMeshAgent agent, GameObject target)
    {
        agent.isStopped = false;

        agent.SetDestination(target.transform.position);
        agent.gameObject.GetComponent<Attributes>().Stamina -= 1 * Time.deltaTime;
    }
}
