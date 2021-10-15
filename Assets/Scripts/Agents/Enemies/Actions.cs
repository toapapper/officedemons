using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Last Edited: 15/10-21
public class Actions : MonoBehaviour
{
    public void MoveTowards(NavMeshAgent agent, GameObject target)
    {
		agent.isStopped = false;

		agent.SetDestination(target.transform.position);
        agent.gameObject.GetComponent<Attributes>().Stamina -= 1 * Time.deltaTime;
    }
}
