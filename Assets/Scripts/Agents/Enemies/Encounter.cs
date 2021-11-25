
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
    [SerializeField] AK.Wwise.State combatMusicState;
    [SerializeField] AK.Wwise.State roamingState1;
    private bool myTurn = false;
    private int currentEnemysTurn = 0;

    void Awake()
    {        
        aIManager = GetComponentInChildren<AIManager>();
        

        // If procedurally generated -> Call SpawnEnemiesRrndomPositions() instead of ActivateEnemies()
    }

    // Maybe use this for procedural? Would need to randomize amountOfEnemies first.
    //public void SpawnEnemiesRandomPositions(int amountOfEnemies)
    //{
    //    Bounds bounds = gameObject.GetComponent<BoxCollider>().bounds;
    //    List<Vector3> enemySpawnPositions = new List<Vector3>();
    //
    //    for (int i = 0; i < amountOfEnemies; i++)
    //    {
    //        Vector3 v = new Vector3(
    //            Random.Range(bounds.min.x, bounds.max.x),
    //            Random.Range(bounds.min.y, bounds.max.y),
    //            Random.Range(bounds.min.z, bounds.max.z)
    //        );
    //        enemySpawnPositions.Add(v);
    //    }
    //
    //    foreach (Vector3 vector in enemySpawnPositions)
    //    {
    //        // Need to reference prefab at start
    //        GameObject enemy = Instantiate(enemyPrefab, vector, Quaternion.identity);
    //        gameObject.AddComponent<GameObject>(enemy);
    //    }
    //}

    void ActivateEnemies(List<GameObject> enemyList)
    {
        foreach (GameObject e in enemyList)
        {
            e.GetComponent<AIController>().InActiveEncounter = true; 
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
            combatMusicState.SetValue();
            GameManager.Instance.StartEncounter(this);
            ActivateEnemies(GetEnemylist());
        }
    }

    public void EndEncounter()
    {
        AkSoundEngine.SetState("Music", "RoamingState1");
        Destroy(gameObject);
    }
}
