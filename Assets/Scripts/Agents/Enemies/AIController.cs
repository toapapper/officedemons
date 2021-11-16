using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

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
    public NavMeshAgent navMeshAgent; // TODO: Maybe change to property
    private AIManager aiManager;
    private GameObject closestPlayer;
    private WeaponHand weapon;
    private GameObject target;
    
    private Class aiClass;

    private Vector3 targetPosition;
    public Vector3 TargetPosition
    {
        get { return targetPosition; }
        set { targetPosition = value; }
    }

    private bool inActiveEncounter = false;
    public bool InActiveEncounter
    {
        get { return inActiveEncounter; }
        set { inActiveEncounter = value; }
    }

    private bool currentlyMoving;
    public bool CurrentlyMoving
    {
        get { return currentlyMoving; }
        set { currentlyMoving = value; }
    }

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

    private bool actionIsLocked;
    public bool ActionIsLocked
    {
        //if (CurrentState == AIStates.States.Attack || CurrentState == AIStates.States.Wait)
        //{
        //    return true;
        //}
        //return false;

        get { return actionIsLocked; }
        set { actionIsLocked = value; }
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

    public void Die()
    {
        Debug.Log("D种种种种种种种种种种D");
        foreach (GameObject grondEffectObject in GameManager.Instance.GroundEffectObjects)
        {            
			if (grondEffectObject.GetComponent<CoffeStain>().agentsOnStain.Contains(gameObject))
			{
				grondEffectObject.GetComponent<CoffeStain>().agentsOnStain.Remove(gameObject);
                //Utilities.CleanList(grondEffectObject.GetComponent<CoffeStain>().agentsOnStain);
                Debug.Log(gameObject.name + "REMOVED");
			}
		}
        navMeshAgent.ResetPath();
        aiManager.EnemyList.Remove(gameObject);
        Destroy(gameObject);
    }   
        
        
    /// <summary>
    /// Performs the behaviour corresponding to the current state.
    /// </summary>
    /// <param name=""></param>
    public void PerformBehaviour()
    {
        aiStateHandler.StateUpdate(aiClass);

        switch (CurrentState) 
        {
            case AIStates.States.FindCover:
                // TO DO: Implement a behaviour for low health

                if (targetPosition == Vector3.zero)
                {
                    closestPlayer = CalculateClosest(PlayerManager.players, priorites);
                    FindCover(closestPlayer);
                }

                if (transform.position == targetPosition)
                {
                    currentState = AIStates.States.Wait;
                }
                else
                {
                    MoveTowards(targetPosition);
                }
                break;

            case AIStates.States.CallForHealing:
                // TO DO: Implement a behaviour for low health
                break;

            case AIStates.States.Attack:
                aiManager.SaveAction(this.gameObject);
                ActionIsLocked = true;

                break;

            case AIStates.States.Move:

                if (targetPosition == Vector3.zero)
                {
                    closestPlayer = CalculateClosest(PlayerManager.players, priorites);
                    targetPosition = closestPlayer.transform.position;
                    if (closestPlayer == null)
                    {
                        currentState = AIStates.States.Wait;
                    }
                }

                MoveTowards(targetPosition);

                if (transform.position == targetPosition)
                {
                    currentState = AIStates.States.Unassigned;
                }

                break;

            case AIStates.States.Wait:
                aiManager.SaveAction(this.gameObject);
                Debug.Log("Wait");
                break;

            case AIStates.States.Dead:
                Effects.Die(this.gameObject);
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

    public void FindCover(GameObject opponent)
    {
        List<NavMeshHit> hitList = new List<NavMeshHit>();
        NavMeshHit navHit;

        // Loop to create random points around the player so we can find the nearest point to all of them, storting the hits in a list
        for (int i = 0; i < 15; i++)
        {
            Vector3 spawnPoint = transform.position;
            Vector2 offset = Random.insideUnitCircle * i;
            spawnPoint.x += offset.x;
            spawnPoint.z += offset.y;

            NavMesh.FindClosestEdge(spawnPoint, out navHit, NavMesh.AllAreas);

            hitList.Add(navHit);
        }

        // sort the list by distance using Linq
        var sortedList = hitList.OrderBy(x => x.distance);

        // Loop through the sortedList and see if the hit normal doesn't point towards the enemy.
        // If it doesn't point towards the enemy, navigate the agent to that position and break the loop as this is the closest cover for the agent. (Because the list is sorted on distance)
        foreach (NavMeshHit hit in sortedList)
        {
            if (Vector3.Dot(hit.normal, (opponent.transform.position - transform.position)) < 0)
            {
                targetPosition = hit.position;
                break;
            }
        }
    }

    public void MoveTowards(Vector3 targetPos)
    {
        navMeshAgent.isStopped = false;

        navMeshAgent.SetDestination(targetPos);
        gameObject.GetComponent<Attributes>().Stamina -= 1 * Time.deltaTime;
        targetPosition = targetPos;
    }
}
