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
    public static void MoveTowards(NavMeshAgent agent, Vector3 targetPosition)
    {
        agent.gameObject.GetComponent<AIController>().CurrentlyMoving = true;
        agent.isStopped = false;

        agent.SetDestination(targetPosition);
        agent.gameObject.GetComponent<Attributes>().Stamina -= 1 * Time.deltaTime;

        
        agent.gameObject.GetComponent<AIController>().CurrentlyMoving = false;
    }
}
