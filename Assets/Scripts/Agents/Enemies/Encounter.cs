
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

    //[SerializeField]
    //[Range(1, 6)]
    //int amountOfEnemies;

    public List<NavMeshAgent> navMeshAgents;
    public AIManager aIManager;

    public List<GameObject> playerPositions;

    private bool myTurn = false;
    private int currentEnemysTurn = 0;

    void Awake()
    {        
        aIManager = GetComponentInChildren<AIManager>();
        

        // If procedurally generated -> Call SpawnEnemiesRrndomPositions() instead of ActivateEnemies()
    }

    void ActivateEnemies(List<GameObject> enemyList)
    {
        foreach (GameObject e in enemyList)
        {
            e.GetComponent<AIController>().InActiveCombat = true; 
        }
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
            ActivateEnemies(GetEnemylist());
        }
    }

    public void EndEncounter()
    {
        Destroy(gameObject);
    }
}
