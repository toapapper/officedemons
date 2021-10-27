
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// <para>
/// Encapsulates one encounter and contains the enemies for that encounter.<br/>
/// Contains methods to start and end the encounter.<br/>
/// Contains a method to start the encounter when the player enters the designated area.
/// </para>
///   
/// <para>
///  Author: Ossian
/// </para>
///  
/// </summary>

/*
 * Last Edited:
 * 15-10-2021
 */

[RequireComponent(typeof(BoxCollider))]
public class Encounter : MonoBehaviour
{
    //[HideInInspector]
    //public List<GameObject> enemies;    
    
    public List<NavMeshAgent> navMeshAgents;
    public AIManager aIManager;

    private bool myTurn = false;
    private int currentEnemysTurn = 0;

    void Awake()
    {
        aIManager = GetComponentInChildren<AIManager>();
    }


    public List<GameObject> GetEnemylist()
    {
        List<GameObject> enemies = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.CompareTag("Enemy"))
                enemies.Add(child);
        }

        return enemies;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("collision enter " + collision.collider.CompareTag("Player"));
    }

    //Kanske temporär, för att avgöra om spelare kommit in i encounterområdet
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && GameManager.Instance.CurrentCombatState == CombatState.none)
        {
            GameManager.Instance.StartEncounter(this);
        }
    }

    public void EndEncounter()
    {
        Destroy(gameObject);
    }
}
