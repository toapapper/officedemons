using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// <para>
/// Controls the AI-agent
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

public class AIController : MonoBehaviour
{
    private FieldOfView fov;
    private Actions actions;
    private NavMeshAgent navMeshAgent;
    private AIManager aiManager;
    private GameObject closestPlayer;
    private WeaponHand weaponHand;

    [SerializeField]
    private Animator animator;
    private AIStateHandler aiStateHandler;

    private AIStates.States currentState;
    public AIStates.States CurrentState { get; set; }

    public bool LockedAction()
    {
        if (CurrentState == AIStates.States.Attack || CurrentState == AIStates.States.Wait)
        {
            return true;
        }
        return false;
    }

    void Start()
    {
        fov = GetComponent<FieldOfView>();
		actions = GetComponent<Actions>();
		navMeshAgent = GetComponent<NavMeshAgent>();
        CurrentState = AIStates.States.Unassigned;
        aiStateHandler = GetComponent<AIStateHandler>();
        aiManager = transform.parent.GetComponentInChildren<AIManager>();
        weaponHand = GetComponent<WeaponHand>();
    }

    /// <summary>
    /// Performs the behaviour corresponding to the current state.
    /// </summary>
    /// <param name=""></param>
    public void PerformBehaviour()
    {
        aiStateHandler.UpdateState();                  

        switch (CurrentState) 
        {
            case AIStates.States.FindCover:
                // TODO: implement this behaviour
                break;

            case AIStates.States.CallForHealing:
                // TODO: implement this behaviour
                break;

            case AIStates.States.Attack:
                aiManager.SaveAction(this.gameObject);
                navMeshAgent.destination = transform.position;
                weaponHand.StartAttack();
                navMeshAgent.isStopped = true;
                break;

            case AIStates.States.Move:
                closestPlayer = CalculateClosest(PlayerManager.players);
                if (closestPlayer == null)
                    currentState = AIStates.States.Wait;

                actions.MoveTowards(navMeshAgent, closestPlayer);
                break;

            case AIStates.States.Wait:
                aiManager.SaveAction(this.gameObject);
                navMeshAgent.destination = transform.position;
                navMeshAgent.isStopped = true;
                break;
        }
    }

    /// <summary>
    /// Calls the method corresponding to the action that was locked in.
    /// </summary>
    /// <param name=""></param>
    public void PerformAction()
    {
        // TODO: Implement Revive
        switch (currentState)
        {
            case AIStates.States.Attack:
                weaponHand.Attack();
                break;
            default:
                Debug.Log("default");
                break;
        }
    }

    /// <summary>
    /// Calculates what player is the closest to the AI-agent.
    /// </summary>
    /// <param name="players"></param>
    public GameObject CalculateClosest(List<GameObject> players)
    {
        NavMeshPath path = new NavMeshPath();

        float closestDistance = float.MaxValue;

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == null)
            {
                continue;
            }
            if (NavMesh.CalculatePath(transform.position, players[i].gameObject.transform.position, navMeshAgent.areaMask, path))
            {
                float distance = Vector3.Distance(transform.position, path.corners[0]);
                for (int j = 1; j < path.corners.Length; j++)
                {
                    distance += Vector3.Distance(path.corners[j-1],path.corners[j]);
                }

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayer = players[i];
                }
            }
        }
        return closestPlayer;
    }
}
