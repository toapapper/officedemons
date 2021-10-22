using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// <para>
/// Controls the AI-agents
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

public enum Class { Aggresive, Defensive, Healer};

public class AIController : MonoBehaviour
{
    private FieldOfView fov;
    private NavMeshAgent navMeshAgent;
    private AIManager aiManager;
    private GameObject closestPlayer;
    private WeaponHand weapon;
    private GameObject target;
    private Class currentClass;

    List<GameObject> priorites;
    public List<GameObject> Priorites
    {
        get { return priorites; }
        set { priorites = value; }
    }

    private AIStates.States currentState;
    public AIStates.States CurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }

    [SerializeField]
    private Animator animator;
    private AIStateHandler aiStateHandler;

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
        navMeshAgent = GetComponent<NavMeshAgent>();
        CurrentState = AIStates.States.Unassigned;
        aiStateHandler = GetComponent<AIStateHandler>();
        aiManager = transform.parent.GetComponentInChildren<AIManager>();
        weapon = GetComponent<WeaponHand>();
    }

    /// <summary>
    /// Performs the behaviour corresponding to the current state.
    /// </summary>
    /// <param name=""></param>
    public void PerformBehaviour()
    {
        aiStateHandler.GetState(currentClass);

        switch (CurrentState) 
        {
            case AIStates.States.FindCover:
                // TO DO: Implement a behaviour for low health
                break;

            case AIStates.States.CallForHealing:
                // TO DO: Implement a behaviour for low health
                break;

            case AIStates.States.Attack:
                aiManager.SaveAction(this.gameObject);
                navMeshAgent.destination = transform.position;
                weapon.StartAttack();
                navMeshAgent.isStopped = true;
                break;

            case AIStates.States.Move:
                closestPlayer = CalculateClosest(PlayerManager.players, priorites);
                if (closestPlayer == null)
                {
                    currentState = AIStates.States.Wait;
                }
                EnemyActions.Instance.MoveTowards(navMeshAgent, closestPlayer);
                break;

            case AIStates.States.Wait:
                aiManager.SaveAction(this.gameObject);
                navMeshAgent.destination = transform.position;
                navMeshAgent.isStopped = true;
                break;
        }
    }

    /// <summary>
    /// Performs the action corresponding to the state.
    /// </summary>
    /// <param name=""></param>
    public void PerformAction()
    {
        switch (currentState)
        {
            case AIStates.States.Attack:
                weapon.Attack();
                break;
            default:
                Debug.Log("default");
                break;
        }
    }

    /// <summary>
    /// Calculates what player is the closest to the AI-agent.
    /// </summary>
    /// <param name="players, priorities"></param>
    public GameObject CalculateClosest(List<GameObject> players, List<GameObject> priorities)
    {
        float closestDistance = float.MaxValue;
        for (int i = 0; i < priorites.Count; i++)
        {
            if (priorites[i].GetComponent<Attributes>().Health <=0)
            {
                priorites.RemoveAt(i);
            }
        }

        for (int i = 0; i < priorites.Count; i++)
        {
            if (priorites[i].GetComponent<Attributes>().Health <= 0)
            {
                continue;
            }
            float distance = CalculateDistance(priorites[i]);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = priorites[i];
            }
        }

        if (closestDistance == float.MaxValue)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] == null)
                {
                    continue;
                }
                else if (players[i].GetComponent<Attributes>().Health <= 0)
                {
                    continue;
                }
                float distance = CalculateDistance(players[i]);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayer = players[i];
                }
            }
        }
        return closestPlayer;
    }

    /// <summary>
    /// Check if the agent will be able to reach the player.
    /// </summary>
    /// <param name="target"></param>
    public bool ReachableTarget(GameObject target)
    {
        float stamina = navMeshAgent.gameObject.GetComponent<Attributes>().Stamina;
        float targetDistance = CalculateDistance(target);
        float lastPathDistance = CalculateLastPathDistance(target);

        if ( lastPathDistance <= fov.ViewRadius)
        {
            if (targetDistance - lastPathDistance <= stamina * navMeshAgent.speed / 1.2f)
            {
                return true;
            }
        }
        else if (targetDistance - fov.ViewRadius <= stamina * navMeshAgent.speed / 1.2f)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Calulate navmesh path distance from agent to target (another NavMeshAgent).
    /// </summary>
    /// <param name="target"></param>
    private float CalculateDistance(GameObject target)
    {
        NavMeshPath path = new NavMeshPath();
        float distance = float.MaxValue;

        if (NavMesh.CalculatePath(transform.position, target.gameObject.transform.position, navMeshAgent.areaMask, path))
        {
            distance = Vector3.Distance(transform.position, path.corners[0]);
            for (int i = 1; i < path.corners.Length; i++)
            {
                distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }
        }
        return distance;
    }

    /// <summary>
    /// Get the last straight length from agent to target (another NavMeshAgent).
    /// </summary>
    /// <param name="target"></param>
    private float CalculateLastPathDistance(GameObject target)
    {
        NavMeshPath path = new NavMeshPath();
        float distance = 0;
        if (NavMesh.CalculatePath(transform.position, target.gameObject.transform.position, navMeshAgent.areaMask, path))
        {
            for (int i = path.corners.Length - 1; i > 1; i--)
            {
                distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                break;
            }
        }
        return distance;
    }

    public bool FindClosestAndCheckIfReachable()
    {
       GameObject closest = CalculateClosest(PlayerManager.players, Priorites);
        if (ReachableTarget(closest))
        {
            return true;
        }
        return false;
    }
}
