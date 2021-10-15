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

public class EnemyActions : MonoBehaviour
{
    public static EnemyActions Instance { get; private set; }

    private void Start()
    {
        Instance = this;
    }

    public void Die(GameObject target)
    {
        if (target.tag == "Enemy")
        {
            target.GetComponent<MeshRenderer>().material.color = Color.black;
            target.GetComponent<AIController>().CurrentState = AIStates.States.Dead;
        }
        else if (target.tag == "Player")
        {
            target.GetComponent<PlayerStateController>().Die();
            if (GameManager.Instance.CurrentCombatState == CombatState.none)
            {
                StartCoroutine(DelayedSelfRevive(target));
            }
        }
    }
    IEnumerator DelayedSelfRevive(GameObject target)
    {
        yield return new WaitForSeconds(1);
        Revive(target);
        yield return null;
    }

    public void Revive(GameObject target)
    {
        if (target.tag == "Enemy")
        {
            // TODO: Implement Revive
        }
        else if (target.tag == "Player")
        {
            target.GetComponent<PlayerStateController>().Revive(); //Remove?
        }        
    }

    public void MoveTowards(NavMeshAgent agent, GameObject target)
    {
        agent.isStopped = false;

        agent.SetDestination(target.transform.position);
        agent.gameObject.GetComponent<Attributes>().Stamina -= 1 * Time.deltaTime;
    }
}
