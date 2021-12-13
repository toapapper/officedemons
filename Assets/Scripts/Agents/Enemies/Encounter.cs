
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



    [SerializeField] int encounterNumber = 0;

    private bool myTurn = false;
    private int currentEnemysTurn = 0;

    private GameObject midPoint;
    private GameObject leftBottomPoint;
    private GameObject rightBottomPoint;
    private GameObject leftTopPoint;
    private GameObject rightTopPoint;
    public GameObject MidPoint { get { return midPoint; } }
    Collider boxCollider;
    Vector3 m_Center;
    Vector3 m_Size, m_Min, m_Max;

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


        CreateCornerPoints();

        // If procedurally generated -> Call SpawnEnemiesRrndomPositions() instead of ActivateEnemies()
    }

    /// <summary>
    /// Returns a list of all the corner points in the box collider
    /// Author: Jonas
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetCameraPoints()
    {
        return new List<GameObject> { leftBottomPoint, rightBottomPoint, leftTopPoint, rightTopPoint };
    }

    /// <summary>
    /// Create gameobjects that are points in each corner of the box collider of the encounter
    /// Author: Jonas
    /// </summary>
    private void CreateCornerPoints()
    {
        boxCollider = GetComponent<BoxCollider>();

        midPoint = new GameObject("Midpoint");
        leftBottomPoint = new GameObject("LeftBottomPoint");
        rightBottomPoint = new GameObject("RightBottomPoint");
        leftTopPoint = new GameObject("LeftTopPoint");
        rightTopPoint = new GameObject("RightTopPoint");

        midPoint.transform.parent = transform;
        leftBottomPoint.transform.parent = transform;
        rightBottomPoint.transform.parent = transform;
        leftTopPoint.transform.parent = transform;
        rightTopPoint.transform.parent = transform;

        midPoint.transform.position = GetComponent<BoxCollider>().bounds.center;
        leftBottomPoint.transform.position = boxCollider.bounds.min;
        rightBottomPoint.transform.position = new Vector3(boxCollider.bounds.min.x + boxCollider.bounds.size.x, boxCollider.bounds.min.y, boxCollider.bounds.min.z);
        leftTopPoint.transform.position = new Vector3(boxCollider.bounds.min.x, boxCollider.bounds.min.y, boxCollider.bounds.max.z);
        rightTopPoint.transform.position = new Vector3(boxCollider.bounds.max.x, boxCollider.bounds.min.y, boxCollider.bounds.max.z);
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
                if (child.name != "tank")
                {
                    child.GetComponent<AIController>().Die();
                }
                else
                {
                    child.GetComponent<TankController>().Die();
                    Destroy(child);
                }
                
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

    //Kanske tempor�r, f�r att avg�ra om spelare kommit in i encounteromr�det
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && GameManager.Instance.CurrentCombatState == CombatState.none)
        {
            AkSoundEngine.SetState("Music_State", "Combat");
            if (encounterNumber == 1)
            {
                AkSoundEngine.SetState("Music", "CombatState1");
            }
            else if (encounterNumber == 2)
            {
                AkSoundEngine.SetState("Music", "CombatState2");
            }
            else if (encounterNumber == 3)
            {
                AkSoundEngine.SetState("Music", "CombatState3");
            }
            else if (encounterNumber == 4)
            {
                AkSoundEngine.SetState("Music", "CombatState4");
            }
            GameManager.Instance.StartEncounter(this);
            //GetComponentInChildren<AIManager>().EnableEnemyDamage(GetEnemylist());
        }
    }

    public void EndEncounter()
    {
        AkSoundEngine.SetState("Music_State", "Roaming");
        AkSoundEngine.SetState("Music", "RoamingState1");
        Destroy(gameObject);
    }

    public void ResetEncounter()
    {
        DestroyEnemies();
        foreach (KeyValuePair<Vector3, string> enemy in enemyStartPositions)
        {
            string enemyName = enemy.Value/*.Remove(enemy.Value.IndexOf(' '))*/;
            if (enemyName.Contains(" "))
            {
                enemyName = enemyName.Remove(enemy.Value.IndexOf(' '));
            }
            if (Resources.Load(enemyName))
            {
                GameObject newEnemy = Instantiate(Resources.Load(enemyName), enemy.Key, Quaternion.Euler(0, 0, 0)) as GameObject;
                newEnemy.transform.parent = this.transform;

            }
        }
    }
}
