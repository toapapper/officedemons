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

public enum Class { Aggresive, Defensive, Healer };

public class AIController : MonoBehaviour
{
    private FieldOfView fov;
    public NavMeshAgent navMeshAgent; // TODO: Maybe change to property
    private AIManager aiManager;
    public GameObject targetPlayer;
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
        foreach (GameObject go in GameManager.Instance.GroundEffectObjects)
        {
            if (go.GetComponent<CoffeStain>().agentsOnStain.Contains(gameObject))
            {
                go.GetComponent<CoffeStain>().agentsOnStain.Remove(gameObject);
            }
        }
        if (navMeshAgent.hasPath)
        {
            navMeshAgent.ResetPath();
        }
        aiManager.RemoveAction(gameObject);
        aiManager.enemyList.Remove(gameObject);
        Destroy(gameObject);
    }


    /// <summary>
    /// Performs the behaviour corresponding to the current state.
    /// </summary>
    /// <param name=""></param>
    public void PerformBehaviour()
    {
        aiStateHandler.StateUpdate();

        switch (CurrentState)
        {
            case AIStates.States.FindCover:

                if (targetPosition == Vector3.zero)
                {
                    targetPlayer = CalculateClosest(PlayerManager.players, aiManager.killPriority);
                    FindCover(targetPlayer);
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
                    // if hasAdvantage or HasRangedWeapon chose the player with lowest health, else chose closest
                    if (HoldingRangedWeapon() || aiStateHandler.HasAdvantage())
                    {
                        targetPlayer = aiManager.killPriority[0];
                    }
                    else
                    {
                        targetPlayer = CalculateClosest(PlayerManager.players, aiManager.killPriority);
                    }
                    
                    if (targetPlayer == null)
                    {
                        currentState = AIStates.States.Wait;
                    }
                    else
                    {
                        targetPosition = targetPlayer.transform.position;
                    }
                }

                if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance || transform.position != targetPosition) // <-- -CanHitTarget?
                {
                    MoveTowards(targetPosition);
                }
                else
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
        for (int i = 0; i < aiManager.killPriority.Count; i++)
        {
            if (aiManager.killPriority[i].GetComponent<Attributes>().Health <= 0)
            {
                aiManager.killPriority.RemoveAt(i);
            }
        }

        for (int i = 0; i < aiManager.killPriority.Count; i++)
        {
            if (aiManager.killPriority[i].GetComponent<Attributes>().Health <= 0) // kanske onödig?
            {
                continue;
            }
            float distance = CalculateDistance(aiManager.killPriority[i]);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                targetPlayer = aiManager.killPriority[i];
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
                    targetPlayer = players[i];
                }
            }
        }
        return targetPlayer;
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

        if (lastPathDistance <= fov.ViewRadius)
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
        GameObject closest = CalculateClosest(PlayerManager.players, aiManager.killPriority);
        if (ReachableTarget(closest))
        {
            return true;
        }
        return false;
    }

    public void FindCover(GameObject opponent)
    {
        // casta en ray fr�n opponent till coverpositions
        // v�lj den som �r n�rmst och obstructed               (�ndra kanske sen s� att den kollar om det finns en som �r obstructed av flera)

        // Add a check if spot already taken
        RaycastHit hit = new RaycastHit();
        float minDistToCover = Mathf.Infinity;

        foreach (Vector3 pos in aiManager.coverList)
        {
            if (Physics.Raycast(opponent.transform.position, (pos - opponent.transform.position).normalized, out hit))
            {
                if (hit.transform.gameObject.tag == "CoverObject")
                {
                    foreach (Transform child in hit.transform)
                    {
                        RaycastHit hit2 = new RaycastHit();
                        if (!aiManager.takenCoverPositions.Contains(child.transform.position) && Physics.Raycast(child.position, (opponent.transform.position - child.position).normalized, out hit2))
                        {
                            if (hit2.transform.gameObject.tag == "CoverObject")
                            {
                                if (Vector3.Magnitude((hit2.transform.position - opponent.transform.position)) < minDistToCover)
                                {
                                    aiManager.takenCoverPositions.Add(child.position);
                                    targetPosition = child.position;
                                    minDistToCover = Vector3.Magnitude((hit2.transform.position - opponent.transform.position));
                                }
                            }
                        }
                        //targetPosition = child.position;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Maybe will keep these because we might have so that ranged weapons can shoot over some obstacles
    /// </summary>
    /// <returns></returns>
    /// rightHand = this.gameObject.transform.GetChild(1).gameObject;

    public bool HoldingRangedWeapon()
    {
        if (gameObject.transform.GetChild(0).transform.GetChild(1).childCount > 0 && gameObject.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).gameObject.GetType() == typeof(RangedWeapon))
        {
            return true;
        }
        return false;
    }

    public bool HoldingMeleeWeapon()
    {
        if (gameObject.transform.GetChild(0).transform.GetChild(1).childCount > 0 && gameObject.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).gameObject.GetType() == typeof(MeleeWeapon))
        {
            return true;
        }
        return false;
    }

    public bool IsArmed()
    {
        if (HoldingMeleeWeapon() || HoldingRangedWeapon())
        {
            return true;
        }
        return false;
    }

    public void MoveTowards(Vector3 targetPos)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(targetPos);
        gameObject.GetComponent<Attributes>().Stamina -= 1 * Time.deltaTime;
        targetPosition = targetPos;
    }

    //private bool CanHitTarget()
    //{
    //    Vector3 direction = (targetPlayer.transform.position - transform.position).normalized;
    //    RaycastHit hit = new RaycastHit();
    //
    //    if (Physics.Raycast(transform.position, direction, out hit))
    //    {
    //        if (hit.transform.gameObject.tag == "Player")
    //        {
    //            // if not too far away
    //            if ((transform.position - hit.transform.position).magnitude <= fov.ViewRadius)
    //            {
    //                return true;
    //            }
    //        }
    //    }
    //    return false;
    //}
}
