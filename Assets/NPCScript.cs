using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCScript : MonoBehaviour
{
    Vector3 exitPosition;
    Vector3 spawnPosition;
    Vector3 currentTargetPosition;
    NavMeshAgent navMeshAgent;
    Attributes attributes;

    // for random wandering
    float wanderRadius = 10;
    float wanderTimer = 3;
    float timer = 0;
    int wandersBeforeExit = 3; // How many times they take a random destination before going towards an exit
    int counter = 0;

    public void Update()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            if (counter < wandersBeforeExit)
            {
                currentTargetPosition = RandomNavSphere(transform.position, wanderRadius, -1);
                timer = 0;
                counter += 1;
            }
            else
            {
                currentTargetPosition = exitPosition;
            }
        }

        // if hp <= 0 -> Die
        if (attributes.Health <= 0)
        {
            Die();
        }
        else
        {
            Debug.Log("spawn: " + spawnPosition + "  exit: " + exitPosition);
            MoveTowards(currentTargetPosition);
        }
        
    }

    public void MoveTowards(Vector3 targetPos)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(targetPos);
    }
    

    public void InstantiateValues(Vector3 start, Vector3 exit)
    {
        exitPosition = exit;
        spawnPosition = start;
        currentTargetPosition = RandomNavSphere(transform.position, wanderRadius, -1);
        
        gameObject.transform.position = spawnPosition;
        navMeshAgent = GetComponent<NavMeshAgent>();
        
        attributes = GetComponent<Attributes>();
    }

    public void Die()
    {
        // play dying effect
        // remove from list in manager
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
