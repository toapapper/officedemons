using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// <para>
/// Handles the group behaviour of AI-agents
/// 
/// </para>
///   
///  <para>
///  Author: Tinea Larsson, Tim Wennerberg
///  
/// </para>
///  
/// </summary>

// Last Edited: 2021-10-14

public class AIManager : MonoBehaviour
{
    private List<GameObject> enemyList;
    public List<GameObject> EnemyList { get; set; }

    private List<GameObject> playerList;
    private Queue<GameObject> actionQueue;
    public AIManager instance;

    private void Start()
    {
        actionQueue = new Queue<GameObject>();
        playerList = PlayerManager.players;
    }

    /// <summary>
    /// Initializes necessary variables when entering an encounter.
    /// </summary>
    /// <param name=""></param>
    public void BeginCombat()
    {
        enemyList = GameManager.Instance.currentEncounter.GetEnemylist();
        GameManager.Instance.stillCheckList.AddRange(enemyList);
    }

    /// <summary>
    /// Resets variables to prepare for a new turn.
    /// </summary>
    /// <param name=""></param>
    public void BeginTurn() 
    {
        actionQueue.Clear();
        foreach (GameObject e in enemyList)
        {
            e.GetComponent<Attributes>().Stamina = e.GetComponent<Attributes>().StartStamina;
            e.GetComponent<AIController>().CurrentState = AIStates.States.Unassigned;
        }
    }

    /// <summary>
    /// Loops through AI-agents and performs the assigned behaviour. 
    /// Checks if all agents' actions are locked in, or if all agents are dead, in which case the enemies' turn ends.
    /// </summary>
    /// <param name=""></param>
    public void PerformTurn()
    {
        bool allDone = true;
        bool allDead = true;

        foreach (GameObject e in enemyList)
        {
            if (!e.GetComponent<AIController>().LockedAction())
            {
                e.GetComponent<AIController>().PerformBehaviour();
                allDone = false;
            }

            if (e.GetComponent<Attributes>().Health > 0)
            {
                allDead = false;
            }
        }
        if (!GameManager.Instance.AllStill)
            allDone = false;

        if (allDone)
            GameManager.Instance.enemiesTurnDone = true;

        if (allDead)
            GameManager.Instance.EndEncounter();
    }

    /// <summary>
    /// Performs the next action in the queue.
    /// </summary>
    /// <param name=""></param>
    public void PerformNextAction()
    {
        if (actionQueue.Count > 0)
        {
            GameObject agent = actionQueue.Dequeue();
            agent.GetComponent<AIController>().PerformAction();
            StartCoroutine("WaitDone");
        }
        else
        {
            GameManager.Instance.enemiesActionsDone = true;
        }
    }
    IEnumerator WaitDone()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            if (GameManager.Instance.AllStill)
            {
                StopCoroutine("WaitDone");
                PerformNextAction();
            }
            yield return null;
        }
    }

    /// <summary>
    /// Saves the selected action in the queue.
    /// </summary>
    /// <param name="agent"></param>
    public void SaveAction(GameObject agent)
    {
        actionQueue.Enqueue(agent);
    }
  
}
