
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
 * 29-12-2021
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

    private GameObject wall;
    private GameObject topWall;
    private GameObject bottomWall;
    private GameObject leftWall;
    private GameObject rightWall;

    [SerializeField] private bool topWallActive = true;
    [SerializeField] private bool bottomWallActive = true;
    [SerializeField] private bool leftWallActive = true;
    [SerializeField] private bool rightWallActive = true;

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
        InitializeWalls();

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



    const float WallEmissionPerScale = 100f/15f;//emission max number per x scale
    /// <summary>
    /// Creates the walls and sets correct values for particleSystem. Sets them to inactive
    /// </summary>
    private void InitializeWalls()
    {
        if(wall == null)
        {
            wall = Resources.Load<GameObject>("combatWall");
        }

        float yPos = wall.transform.localScale.y/2;

        Vector3 wallPos = midPoint.transform.position;
        wallPos.y = wall.transform.localScale.y / 2;
        Vector3 wallScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, wall.transform.localScale.z);
        
        Vector3 psScale = wallScale;//particleSystemScale
        psScale.z = .1f;
        psScale.y = 1;

        #region top wall
        //(top left - top right) = scale in x move in z.
        wallScale.x =  rightTopPoint.transform.position.x - leftTopPoint.transform.position.x;
        wallPos.z = rightTopPoint.transform.position.z;

        topWall = Instantiate(wall, transform);
        topWall.transform.position = wallPos;
        topWall.transform.localScale = wallScale;
        topWall.SetActive(false);

        ParticleSystem particleSystem = topWall.transform.GetComponentInChildren<ParticleSystem>();
        psScale.x = wallScale.x;
        var shape = particleSystem.shape;
        shape.scale = psScale;

        var emission = particleSystem.emission;
        var eCurve = emission.rateOverTime;
        eCurve.curveMultiplier = WallEmissionPerScale * psScale.x;
        emission.rateOverTime = eCurve;
        #endregion

        #region bottom wall
        //bottom wall (bottom left - bottom right) = scale in x move in z, wallscale is the same as topWall
        wallPos.z = rightBottomPoint.transform.position.z;
        bottomWall = Instantiate(wall, transform);
        bottomWall.transform.position = wallPos;
        bottomWall.transform.localScale = wallScale;
        bottomWall.SetActive(false);

        particleSystem = bottomWall.transform.GetComponentInChildren<ParticleSystem>();
        psScale.x = wallScale.x;
        shape = particleSystem.shape;
        shape.scale = psScale;

        emission = particleSystem.emission;
        eCurve = emission.rateOverTime;
        eCurve.curveMultiplier = WallEmissionPerScale * psScale.x;
        emission.rateOverTime = eCurve;

        #endregion

        #region left wall
        //left wall (bottom left - top left) = scale in x, rotate 90 deg, and then move in x.
        Quaternion wallRot = Quaternion.Euler(0, 90, 0);
        wallScale.x = rightTopPoint.transform.position.z - rightBottomPoint.transform.position.z;
        wallPos.x = leftTopPoint.transform.position.x;
        wallPos.z = midPoint.transform.position.z;


        leftWall = Instantiate(wall, transform);
        leftWall.transform.position = wallPos;
        leftWall.transform.rotation = wallRot;
        leftWall.transform.localScale = wallScale;
        leftWall.SetActive(false);

        particleSystem = leftWall.transform.GetComponentInChildren<ParticleSystem>();
        psScale.x = wallScale.x;
        shape = particleSystem.shape;
        shape.scale = psScale;

        emission = particleSystem.emission;
        eCurve = emission.rateOverTime;
        eCurve.curveMultiplier = WallEmissionPerScale * psScale.x;
        emission.rateOverTime = eCurve;

        #endregion

        #region right wall
        //right wall (bottom right - top right) = scale in x, rotate 90 deg, and then move in x.
        wallPos.x = rightTopPoint.transform.position.x;
        rightWall = Instantiate(wall, transform);
        rightWall.transform.position = wallPos;
        rightWall.transform.localScale = wallScale;
        rightWall.transform.rotation = wallRot;
        rightWall.SetActive(false);

        particleSystem = rightWall.transform.GetComponentInChildren<ParticleSystem>();
        psScale.x = wallScale.x;
        shape = particleSystem.shape;
        shape.scale = psScale;

        emission = particleSystem.emission;
        eCurve = emission.rateOverTime;
        eCurve.curveMultiplier = WallEmissionPerScale * psScale.x;
        emission.rateOverTime = eCurve;

        #endregion
    }

    private void ToggleWalls(bool on)
    {
        topWall.SetActive(topWallActive ? on : false);
        bottomWall.SetActive(bottomWallActive ? on : false);
        leftWall.SetActive(leftWallActive ? on : false);
        rightWall.SetActive(rightWallActive ? on : false);
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

    public void StartEncounter()
    {
        ToggleWalls(true);
    }

    public void EndEncounter()
    {
        AkSoundEngine.SetState("Music_State", "Roaming");
        AkSoundEngine.SetState("Music", "RoamingState1");
        ToggleWalls(false);
        Destroy(gameObject);
    }

    public void ResetEncounter()
    {
        ToggleWalls(false);
        DestroyEnemies();
        foreach (KeyValuePair<Vector3, string> enemy in enemyStartPositions)
        {
            string enemyName = enemy.Value/*.Remove(enemy.Value.IndexOf(' '))*/;
            if (enemyName.Contains(" "))
            {
                enemyName = enemyName.Remove(enemy.Value.IndexOf(' '));
            }
			if (enemyName.Contains("("))
			{
                enemyName = enemyName.Remove(enemy.Value.IndexOf('('));
            }
            if (Resources.Load(enemyName))
            {
                GameObject newEnemy = Instantiate(Resources.Load(enemyName), enemy.Key, Quaternion.Euler(0, 0, 0)) as GameObject;
                newEnemy.transform.parent = this.transform;
            }
        }
    }
}
