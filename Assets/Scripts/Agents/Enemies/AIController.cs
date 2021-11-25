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

public class AIController : MonoBehaviour
{
    private FieldOfView fov;
    public NavMeshAgent navMeshAgent; // TODO: Maybe change to property
    private AIManager aiManager;
    private WeaponHand weaponHand;

    private GameObject targetPlayer;
    public GameObject TargetPlayer
    {
        get { return targetPlayer; }
        set { targetPlayer = value; }
    }

    private GameObject target;
    public GameObject Target
    {
        get { return target; }
        set { target = value; }
    }

    private Vector3 targetPosition;
    public Vector3 TargetPosition
    {
        get { return targetPosition; }
        set { targetPosition = value; }
    }

    private bool inActiveCombat;
    public bool InActiveCombat
    {
        get { return inActiveCombat; }
        set { inActiveCombat = value; }
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
        get { return actionIsLocked; }
        set { actionIsLocked = value; }
    }

    void Start()
    {
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        fov = GetComponent<FieldOfView>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        CurrentState = AIStates.States.Unassigned;
        aiStateHandler = GetComponent<AIStateHandler>();
        aiManager = transform.parent.GetComponentInChildren<AIManager>();
        weaponHand = GetComponent<WeaponHand>();
    }

    public void Die()
    {
        foreach (GameObject go in GameManager.Instance.GroundEffectObjects)
        {
            if (go.GetComponent<GroundEffectObject>().agentsOnGroundEffect.Contains(gameObject))
            {
                go.GetComponent<GroundEffectObject>().agentsOnGroundEffect.Remove(gameObject);
            }
        }
        if (navMeshAgent.hasPath)
        {
            navMeshAgent.ResetPath();
        }
        aiManager.RemoveAction(gameObject);
        aiManager.EnemyList.Remove(gameObject);
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
                    targetPlayer = CalculateClosest(PlayerManager.players, aiManager.KillPriority);
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
                        targetPlayer = aiManager.KillPriority[0];
                    }
                    else
                    {
                        targetPlayer = CalculateClosest(PlayerManager.players, aiManager.KillPriority);
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
                if (!IsArmed() &&Target.CompareTag("WeaponObject") && Vector3.Distance(gameObject.transform.position, Target.transform.position) < 2)
                {
                    PickupWeapon(Target);
                }
                else if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance || transform.position != targetPosition) // <-- -CanHitTarget?
                {
                    MoveTowards(targetPosition);
                }
                else
                {
                    currentState = AIStates.States.Unassigned;
                }
                break;

            case AIStates.States.SearchingForWeapon:
                Target = GetClosestWeapon();
                if (Target == null)
                {
                    TargetPlayer = CalculateClosest(PlayerManager.players, aiManager.KillPriority);
                    Target = TargetPlayer;
                }
                TargetPosition = Target.transform.position;
                CurrentState = AIStates.States.Move;
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
                weaponHand.Attack();
                break;
            default:
                Debug.Log("default");
                break;
        }
    }


    private GameObject GetClosestWeapon()
    {
        Bounds bounds = aiManager.GetComponentInParent<Encounter>().GetComponent<BoxCollider>().bounds;
        GameObject closestWeapon = null;
        float closest = float.MaxValue;
        foreach (GameObject weapon in aiManager.AllWeapons)
        {
            if (bounds.Contains(weapon.transform.position))
            {
                float distance = CalculateDistance(weapon);
                if (distance < closest && !weapon.GetComponent<AbstractWeapon>().IsHeld)
                {
                    closest = distance;
                    closestWeapon = weapon;
                }
            }
        }
        aiManager.AllWeapons.Remove(closestWeapon);
        return closestWeapon;
    }



    /// <summary>
    /// Calculates what player is the closest to the AI-agent.
    /// </summary>
    /// <param name="players, priorities"></param>
    public GameObject CalculateClosest(List<GameObject> players, List<GameObject> killPriority)
    {
        //Debug.Log("PLAYERS " + players.Count + "       PRIORITIES " + priorites.Count);
        float closestDistance = float.MaxValue;

        for (int i = 0; i < killPriority.Count; i++)
        {
            if (killPriority[i].GetComponent<Attributes>().Health <= 0)
            {
                killPriority.RemoveAt(i);
            }
        }

        for (int i = 0; i < killPriority.Count; i++)
        {
            if (killPriority[i].GetComponent<Attributes>().Health <= 0)
            {
                continue;
            }
            float distance = CalculateDistance(killPriority[i]);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                targetPlayer = killPriority[i];
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
    public float CalculateDistance(GameObject target)
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
        GameObject closest = CalculateClosest(PlayerManager.players, aiManager.KillPriority);
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

        foreach (Vector3 pos in aiManager.CoverList)
        {
            if (Physics.Raycast(opponent.transform.position, (pos - opponent.transform.position).normalized, out hit))
            {
                if (hit.transform.gameObject.tag == "CoverObject")
                {
                    foreach (Transform child in hit.transform)
                    {
                        RaycastHit hit2 = new RaycastHit();
                        if (!aiManager.TakenCoverPositions.Contains(child.transform.position) && Physics.Raycast(child.position, (opponent.transform.position - child.position).normalized, out hit2))
                        {
                            if (hit2.transform.gameObject.tag == "CoverObject")
                            {
                                if (Vector3.Magnitude((hit2.transform.position - opponent.transform.position)) < minDistToCover)
                                {
                                    aiManager.TakenCoverPositions.Add(child.position);
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
        if (GetComponent<WeaponHand>().objectInHand.GetComponent<AbstractWeapon>() is RangedWeapon)
        {
            return true;
        }
        return false;
    }

    public bool HoldingMeleeWeapon()
    {
        if (GetComponent<WeaponHand>().objectInHand.GetComponent<AbstractWeapon>() is MeleeWeapon)
        {
            return true;
        }
        return false;
    }

    public bool IsArmed()
    {
		if (GetComponent<WeaponHand>().objectInHand)
		{
            if (HoldingMeleeWeapon() || HoldingRangedWeapon())
            {
                return true;
            }
        }
        return false;
    }

    public void MoveTowards(Vector3 targetPos)
    {
        navMeshAgent.isStopped = false;

        navMeshAgent.SetDestination(targetPos);
        gameObject.GetComponent<Attributes>().Stamina -= 1 * Time.deltaTime;
        //gameObject.GetComponent<Attributes>().Stamina -= 1;
        targetPosition = targetPos;
    }

    public void UpdateClosestPlayer()
    {
        targetPlayer = CalculateClosest(PlayerManager.players, aiManager.KillPriority);
    }


    public void PickupWeapon(GameObject weapon)
    {
        gameObject.GetComponent<WeaponHand>().Equip(weapon);
        navMeshAgent.isStopped = true;
        currentState = AIStates.States.Unassigned;
    }
}