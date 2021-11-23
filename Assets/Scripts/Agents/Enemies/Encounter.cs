
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
    public Dictionary<Vector3, string> enemyStartPositions;


    private bool myTurn = false;
    private int currentEnemysTurn = 0;

    void Awake()
    {        
        aIManager = GetComponentInChildren<AIManager>();
        enemyStartPositions = new Dictionary<Vector3, string>();
        foreach (GameObject enemy in GetEnemylist())
        {
            //Transform tempTransform = enemy.transform;
            Vector3 tempPosition = enemy.transform.position;
            enemyStartPositions.Add(tempPosition, enemy.name);
        }
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

    public void DestroyEnemies()
	{
        //aIManager.EnemyList.Clear();

        for (int i = 0; i < transform.childCount; i++)
		{
			GameObject child = transform.GetChild(i).gameObject;
			if (child.CompareTag("Enemy"))
			{
                child.GetComponent<AIController>().Die();
				//Destroy(child);
			}
		}        
  //      Utilities.CleanList(GameManager.Instance.StillCheckList);
		//Camera.main.GetComponent<MultipleTargetCamera>().ObjectsInCamera = GameManager.Instance.StillCheckList;

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

    public void ResetEncounter()
    {
        DestroyEnemies();
        foreach(KeyValuePair<Vector3, string> enemy in enemyStartPositions)
		{
            string enemyName = enemy.Value/*.Remove(enemy.Value.IndexOf(' '))*/;
            if(enemyName.Contains(" "))
			{
                enemyName = enemyName.Remove(enemy.Value.IndexOf(' '));
            }
            GameObject newEnemy = Instantiate(Resources.Load(enemyName), enemy.Key, Quaternion.Euler(0,0,0)) as GameObject;
            newEnemy.transform.parent = this.transform;
        }
    }
}
