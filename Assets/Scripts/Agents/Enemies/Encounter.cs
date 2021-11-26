
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
            //GetComponentInChildren<AIManager>().EnableEnemyDamage(GetEnemylist());
        }
    }

    public void EndEncounter()
    {
        AkSoundEngine.SetState("Music", "RoamingState1");
        Destroy(gameObject);
    }
}
