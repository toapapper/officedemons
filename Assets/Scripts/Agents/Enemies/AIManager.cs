using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// <para>
/// Manages the group behaviour of AI-agents.
///
/// </para>
///
///  <para>
///  Author: Tinea Larsson, Tim Wennerberg
///
/// </para>
///
/// </summary>

// Last Edited: 15-10-21

public class AIManager : MonoBehaviour
{
    private UnityEvent doneEvent;
    private AIManager instance;
    private Queue<GameObject> actionsQueue;
    private float actionsTime;
    private const float maxActionsTime = 2.5f;

    private List<GameObject> playerList;
    public List<GameObject> PlayerList
    {
        get { return playerList; }
        set { playerList = value; }
    }

    private List<GameObject> enemyList;
    public List<GameObject> EnemyList
    {
        get { return enemyList; }
        set { enemyList = value; }
    }

    private List<Vector3> coverList;
    public List<Vector3> CoverList
    {
        get { return coverList; }
        set { coverList = value; }
    }

    private List<Vector3> takenCoverPositions;
    public List<Vector3> TakenCoverPositions
    {
        get { return takenCoverPositions; }
        set { takenCoverPositions = value; }
    }

    private List<GameObject> allWeapons;
    public List<GameObject> AllWeapons
    {
        get { return allWeapons; }
    }


    private void Start()
    {
        actionsQueue = new Queue<GameObject>();

        CoverList = FindCoverSpotsInEncounter();
        TakenCoverPositions = new List<Vector3>();
        PlayerList = new List<GameObject>();
    }

    /// <summary>
    /// Prepares variables for a new encounter
    /// </summary>
    /// <param name=""></param>
    public void BeginCombat()
    {
        EnemyList = GameManager.Instance.CurrentEncounter.GetEnemylist();
        EnableEnemyDamage();
    }

    /// <summary>
    /// Resets variables to prepare for a new turn
    /// </summary>
    /// <param name=""></param>
    public void BeginTurn()
    {
        UpdatePlayerList();
        TakenCoverPositions.Clear();
        actionsQueue.Clear();
        allWeapons = new List<GameObject>(GameObject.FindGameObjectsWithTag("WeaponObject"));
        actionsTime = 0;
        EnemyList = GameManager.Instance.CurrentEncounter.GetEnemylist();

        foreach (GameObject go in GameManager.Instance.GroundEffectObjects)
        {
            go.GetComponent<GroundEffectObject>().ApplyEffectsOnEnemys();
        }

        int enemiesCount = EnemyList.Count;
        for(int i=0; i<enemiesCount;i++)
        {
            if (EnemyList[i] != null)
            {
                if (EnemyList[i].name == "tank")
                {
                    EnemyList[i].GetComponent<TankController>().ResetValues();
                }
                else
                {
                    // if HumanEnemy
                    EnemyList[i].GetComponent<AIController>().TargetType = AIController.TargetTypes.None;
                    EnemyList[i].GetComponent<AIController>().Target = null;
                    //e.GetComponent<AIController>().GetTargetPlayer(PlayerList);

                    //This might be the wrong way to go about paralyzing enemies, but i dont know, mvh. ossian
                    if (!EnemyList[i].GetComponent<StatusEffectHandler>().Paralyzed)
                    {
                        EnemyList[i].GetComponent<Attributes>().Stamina = EnemyList[i].GetComponent<Attributes>().StartStamina;
                        EnemyList[i].GetComponent<AIController>().ActionIsLocked = false;
                    }
                    else
                    {
                        EnemyList[i].GetComponent<Attributes>().Stamina = 0;
                        EnemyList[i].GetComponent<AIController>().ActionIsLocked = true;
                    }

                    EnemyList[i].GetComponent<StatusEffectHandler>().UpdateEffects();
                }
            }
        }
    }

    /// <summary>
    /// Calls every AI-agent's "update" (AIController.PerformBehaviour).
    /// Checks if agents are dead or all have locked in actions.
    /// </summary>
    /// <param name=""></param>
    public void PerformTurn()
    {
        bool allDone = true;
        bool allDead = true;

        for (int i = 0; i < EnemyList.Count; i++)
        {
            GameObject e = EnemyList[i];

            // Tanks
            if (e.name == "tank")
            {
                
                if (!e.GetComponent<TankController>().ActionIsLocked) // if not all locked actions
                {
                    Debug.LogError("TANK PERFORMING TURN");
                    e.GetComponent<TankController>().PerformBehaviour();
                    allDone = false;
                }
                if (e.GetComponent<Attributes>().Health > 0)
                {
                    allDead = false;
                }
            }
            // All other enemies
            else
            {
                if (!e.GetComponent<AIController>().ActionIsLocked) // if not all locked actions
                {
                    e.GetComponent<AIController>().PerformBehaviour();
                    allDone = false;
                }
                if (e.GetComponent<Attributes>().Health > 0)
                {
                    allDead = false;
                }
            }

        }
        if (actionsQueue.Count != EnemyList.Count && !GameManager.Instance.AllStill)
            allDone = false;

        if (allDone)
            GameManager.Instance.EnemiesTurnDone = true;

        if (allDead)
            GameManager.Instance.EndEncounter();
    }

    /// <summary>
    /// Performs the next action in the queue and then calls itself again after a delay. If there are no actions left however it starts the coroutine WaitDone()
    /// </summary>
    public void PerformNextAction()
    {
        float nextActionDelay = .25f; //i sekunder

        if (actionsQueue.Count > 0)
        {
            GameObject currentEnemy = actionsQueue.Dequeue();
            if (currentEnemy.name == "tank")
            {
                currentEnemy.GetComponent<TankController>().PerformAction();
            }
            else
            {
                currentEnemy.GetComponent<AIController>().PerformAction();
            }
            
            Invoke("PerformNextAction", nextActionDelay);
        }
        else
        {
            StartCoroutine(WaitDone());
        }
    }


    /// <summary>
    /// Waits for 1 seconds and untill all gameObjects are still. It then signals the gamemanager that all enemies are done
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitDone()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            if (GameManager.Instance.AllStill)
            {
                GameManager.Instance.EnemiesActionsDone = true;
                break;
            }
            yield return null;
        }
        yield return null;
    }

    private void UpdatePlayerList()
    {
        PlayerList.Clear();
        foreach (GameObject player in PlayerManager.players)
        {
            if (player != null && player.GetComponent<Attributes>().Health > 0)
            {
                PlayerList.Add(player);
            }
        }
    }

    private List<Vector3> FindCoverSpotsInEncounter()
    {
        Bounds bounds = GetComponentInParent<Encounter>().GetComponent<BoxCollider>().bounds;
        GameObject[] allCovers = GameObject.FindGameObjectsWithTag("CoverPosition");
        List<Vector3> temp = new List<Vector3>();

        foreach (GameObject go in allCovers)
        {
            if (bounds.Contains(go.transform.position))
            {
                temp.Add(go.transform.position);
            }
        }

        return temp;
    }

    public void SaveAction(GameObject agent)
    {
        actionsQueue.Enqueue(agent);

        if (agent.name == "tank")
        {
            agent.GetComponent<TankController>().ActionIsLocked = true;
        }
        else
        {
            agent.GetComponent<AIController>().ActionIsLocked = true;
            agent.GetComponent<AIController>().NMAgent.isStopped = true;
        }
    }

    public void RemoveAction(GameObject agent)
    {
        if (actionsQueue.Contains(agent))
        {
            if (agent.name == "tank")
            {
                agent.GetComponent<TankController>().ActionIsLocked = false;
            }
            else
            {
                agent.GetComponent<AIController>().NMAgent.isStopped = true;
                agent.GetComponent<AIController>().ActionIsLocked = false;
            }
           
            Queue<GameObject> newQueue = new Queue<GameObject>();

            foreach (GameObject go in actionsQueue)
            {
                if (go != agent)
                {
                    newQueue.Enqueue(go);
                }
            }
            actionsQueue = newQueue;
        }
    }

    private void EnableEnemyDamage()
    {
        foreach (GameObject e in EnemyList)
        {
            if (e.name == "tank")
            {
                e.GetComponent<TankController>().InActiveCombat = true;
                e.GetComponent<TankController>().CurrentState = TankController.TankStates.Unassigned;
            }
            else
            {
                e.GetComponent<AIController>().InActiveCombat = true;
                e.GetComponent<AIController>().CurrentState = AIStates.States.Unassigned;
            }
            
            e.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            e.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}
