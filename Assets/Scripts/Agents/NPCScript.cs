using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class NPCScript : MonoBehaviour
{
    Vector3 exitPosition;
    Vector3 spawnPosition;
    Vector3 currentTargetPosition;
    NavMeshAgent navMeshAgent;
    Attributes attributes;
    NPCManager npcManager;

    // for random wandering
    [Header("Random Wandering Properties")]
    [SerializeField] float wanderRadius = 10;
    [SerializeField] float wanderTimer = 3;
    float timer = 0;
    [SerializeField] int wandersBeforeExit = 5; // How many times they take a random destination before going towards an exit
    int counter = 0;

    public void Update()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer && npcManager.spawn)
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
        else if (AgentSlowOrStopped() && npcManager.spawn)
        {
            currentTargetPosition = RandomNavSphere(transform.position, wanderRadius, -1);
        }
        else if(!npcManager.spawn && AgentSlowOrStopped())
        {
            Die();
        }

        MoveTowards(currentTargetPosition);

        if (ReachedExit())
        {
            Despawn();
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
        npcManager = GetComponentInParent<NPCManager>();
        attributes.Health = attributes.StartHealth;
        counter = 0;
    }

    private bool ReachedExit()
    {
        return Vector3.Distance(new Vector3(exitPosition.x, gameObject.transform.position.y, exitPosition.z), gameObject.transform.position) <= 1;
    }

    private bool AgentSlowOrStopped()
    {
        return Math.Abs(navMeshAgent.velocity.x + navMeshAgent.velocity.y + navMeshAgent.velocity.z) < .05;
    }

    public void Die()
    {
        // play dying effect
        foreach (GameObject go in GameManager.Instance.GroundEffectObjects)
        {
            if (go.GetComponent<GroundEffectObject>().agentsOnGroundEffect.Contains(gameObject))
            {
                go.GetComponent<GroundEffectObject>().agentsOnGroundEffect.Remove(gameObject);
            }
        }
        if (navMeshAgent.hasPath)
        {
            navMeshAgent.ResetPath();
        }

        GameObject skeleton = GameManager.Instance.Skeleton;
        if (skeleton != null)
        {
            skeleton = Instantiate(skeleton, transform.position, transform.rotation);

            //Set parent to Skeletons container object
            Transform parent = GameObject.Find("Skeletons").transform;
            if (parent == null)
            {
                parent = new GameObject().transform;
            }

            skeleton.transform.parent = parent;
        }

        gameObject.SetActive(false);
    }

    public void Despawn()
    {
        gameObject.SetActive(false);
    }

    public void SetTargetPosition(Vector3 newTarget)
    {
        currentTargetPosition = newTarget;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
