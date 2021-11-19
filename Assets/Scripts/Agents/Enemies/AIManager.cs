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

    public List<GameObject> enemyList;
    public List<Vector3> coverList;
    public List<Vector3> takenCoverPositions;

    private void Start()
    {
        actionsQueue = new Queue<GameObject>();
        playerList = PlayerManager.players;
        coverList = FindCoverSpotsInEncounter();
        List<Vector3> takenCoverPositions = new List<Vector3>();
    }

    /// <summary>
    /// Prepares variables for a new encounter
    /// </summary>
    /// <param name=""></param>
    public void BeginCombat()
    {
        enemyList = GameManager.Instance.CurrentEncounter.GetEnemylist();
        GameManager.Instance.StillCheckList.AddRange(enemyList);

        foreach (GameObject e in enemyList)
        {
            e.GetComponent<AIController>().CurrentState = AIStates.States.Unassigned;
        }
    }

    /// <summary>
    /// Resets variables to prepare for a new turn
    /// </summary>
    /// <param name=""></param>
    public void BeginTurn()
    {
        takenCoverPositions.Clear();
        actionsQueue.Clear();

        foreach (GameObject grondEffectObject in GameManager.Instance.GroundEffectObjects)
        {
            grondEffectObject.GetComponent<GroundEffectObject>().ApplyEffectsOnEnemys();
        }

        foreach (GameObject e in enemyList)
        {
            e.GetComponent<AIController>().TargetPosition = Vector3.zero;

            //This might be the wrong way to go about paralyzing enemies, but i dont know, mvh. ossian
            if (!e.GetComponent<StatusEffectHandler>().Paralyzed)
            {
                e.GetComponent<Attributes>().Stamina = e.GetComponent<Attributes>().StartStamina;
                e.GetComponent<AIController>().ActionIsLocked = false;
            }

            e.GetComponent<StatusEffectHandler>().UpdateEffects();
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

        List<GameObject> killOnSight = new List<GameObject>();

        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].GetComponent<Attributes>().Health <= playerList[i].GetComponent<Attributes>().StartHealth / 3
                && playerList[i].GetComponent<Attributes>().Health > 0)
            {
                killOnSight.Add(playerList[i]);
            }
        }

        for (int i = 0; i < enemyList.Count; i++)
        {
            GameObject e = enemyList[i];

            if (!e.GetComponent<AIController>().ActionIsLocked) // if not all locked actions
            {
                e.GetComponent<AIController>().Priorites = killOnSight; // ändra sen
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
        agent.GetComponent<AIController>().ActionIsLocked = true;
        agent.GetComponent<AIController>().navMeshAgent.isStopped = true;
    }

    public void RemoveAction(GameObject agent)
    {
        if (actionsQueue.Contains(agent))
        {
            agent.GetComponent<AIController>().ActionIsLocked = false;
            agent.GetComponent<AIController>().navMeshAgent.isStopped = true;

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

    // Check if players are fewer than AI, if players are unarmed but AI have weapons
    public bool HasAdvantage()
    {
        int armedPlayers = 0;
        int alivePlayers = 0;
        int armedEnemies = 0;
        int aliveEnemies = enemyList.Count;

        //count how many players are alive and armed
        foreach (GameObject player in playerList)
        {
            // Fråga om detta kl. 15
            if (player.GetComponent<WeaponHand>().transform.GetChild(0).gameObject.GetType() == typeof(MeleeWeapon) || player.GetComponent<WeaponHand>().transform.GetChild(0).gameObject.GetType() == typeof(RangedWeapon))
            {
                armedPlayers++;
            }

            if (player.GetComponent<Attributes>().Health > 0)
            {
                alivePlayers++;
            }
        }

        foreach (GameObject enemy in enemyList)
        {
            if (enemy.GetComponent<AIController>().IsArmed())
            {
                armedEnemies++;
            }
        }

        if (armedPlayers < armedEnemies || aliveEnemies > alivePlayers) // Add more complexity ?
        {
            return true;
        }
        return false;
    }
}
