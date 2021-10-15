﻿using System.Collections;
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
    private List<GameObject> playerList;
    private UnityEvent doneEvent;
    private AIManager instance;
    private Queue<GameObject> actionsQueue;

    private List<GameObject> enemyList;
    public List<GameObject> EnemyList
    {
        get { return enemyList; }
        set { enemyList = value; }
    }

    private void Start()
    {
        actionsQueue = new Queue<GameObject>();
        playerList = PlayerManager.players;
    }

    public void BeginCombat()
    {
        enemyList = GameManager.Instance.CurrentEncounter.GetEnemylist();
        GameManager.Instance.StillCheckList.AddRange(enemyList);
    }

    /// <summary>
    /// Resets variables to prepare for a new turn
    /// </summary>
    /// <param name=""></param>
    public void BeginTurn() 
    {
        actionsQueue.Clear();
        foreach (GameObject e in enemyList)
        {
            e.GetComponent<Attributes>().Stamina = e.GetComponent<Attributes>().StartStamina;
            e.GetComponent<AIController>().CurrentState = AIStates.States.Unassigned;
        }
    }

    /// <summary>
    /// Calls every AI-agent's "update" (AIController.PeformBehaviour).
    /// Checks if agents are dead or all have locked in actions.
    /// </summary>
    /// <param name=""></param>
    public void PerformTurn()
    {
        bool allDone = true;
        bool allDead = true;

        List<GameObject> killOnSight = new List<GameObject>();

        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].GetComponent<Attributes>().Health <= playerList[i].GetComponent<Attributes>().StartHealth / 3
                && playerList[i].GetComponent<Attributes>().Health > 0)
            {
                killOnSight.Add(playerList[i]);
            }
        }

        foreach (GameObject e in enemyList)
        {
            if (!e.GetComponent<AIController>().LockedAction())
            {
                e.GetComponent<AIController>().Priorites = killOnSight;
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
            GameManager.Instance.EnemiesTurnDone = true;

        if (allDead)
            GameManager.Instance.EndEncounter();
    }

    /// <summary>
    /// Performs the next action in the queue and waits until it's finished.
    /// </summary>
    /// <param name=""></param>
    public void PerformNextAction()
    {
        if (actionsQueue.Count > 0)
        {
            GameObject agent = actionsQueue.Dequeue();
            agent.GetComponent<AIController>().PerformAction();
            StartCoroutine("WaitDone");
        }
        else
        {
            GameManager.Instance.EnemiesActionsDone = true;
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

    public void SaveAction(GameObject agent)
    {
        actionsQueue.Enqueue(agent);
    }
}