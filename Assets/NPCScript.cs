using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCScript : MonoBehaviour
{
    Vector3 targetPosition;
    Vector3 spawnPosition;
    Vector3 position;
    NavMeshAgent navMeshAgent;
    Attributes attributes;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    public void Update()
    {
        // if hp <= 0 -> Die

        // else move
    }

    // 
    public void Move()
    {
        // Claculate next step in navmesh 
        // try to avoid players

    }

    public void InstantiateValues(Vector3 start, Vector3 exit)
    {
        targetPosition = exit;
        spawnPosition = start;
        navMeshAgent = new NavMeshAgent();
        position = spawnPosition;
        
        attributes = GetComponent<Attributes>();

    }

    public void Die()
    {
        // play dying effect
        // remove from list in manager
    }
}
