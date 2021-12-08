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

// Last Edited: 08-12-21

public class AIController : MonoBehaviour
{
    private FieldOfView fov;
    public NavMeshAgent navMeshAgent; // TODO: Maybe change to property
    private AIManager aiManager;
    private WeaponHand weaponHand;

    private List<GameObject> targetsChosen;
    public List<GameObject> KillPriority
    {
        get { return targetsChosen; }
        set { targetsChosen = value; }
    }

    private GameObject target;
    public GameObject Target
    {
        get { return target; }
        set { target = value; }
    }

    public enum TargetTypes { CoverSpot, ShootSpot, Player, Item, None};

    private TargetTypes targetType;
    public TargetTypes TargetType
    {
        get { return targetType; }
        set { targetType = value; }
    }

    private Vector3 targetPosition;
    public Vector3 TargetPosition
    {
        get { return Target.transform.position; }
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
        Target = new GameObject(); //
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        fov = GetComponent<FieldOfView>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        CurrentState = AIStates.States.Unassigned;
        aiStateHandler = GetComponent<AIStateHandler>();
        aiManager = transform.parent.GetComponentInChildren<AIManager>();
        weaponHand = GetComponent<WeaponHand>();
        KillPriority = new List<GameObject>();
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

                if (TargetPosition == Vector3.zero)
                {
                    GameObject closestPlayer = CalculateClosest(aiManager.PlayerList);
                    FindCover(closestPlayer);
                    if (Target == null)
                    {
                        Target = GetTargetPlayer(aiManager.PlayerList);
                        TargetType = TargetTypes.Player;
                        CurrentState = AIStates.States.Attack;
                    }
                    else
                    {
                        TargetType = TargetTypes.CoverSpot;
                    }

                }

                if (ReachedTargetPosition())
                {
                    currentState = AIStates.States.Wait;
                }
                else
                {
                    MoveTowards(TargetPosition);
                }
                break;

            case AIStates.States.Attack:
                //Debug.Log("Target : "  + Target + "of type: " + TargetType);
                //Debug.Log("TargetPosition : " + TargetPosition);
                aiManager.SaveAction(this.gameObject);

                break;

            case AIStates.States.Move:
                if (TargetPosition == Vector3.zero || TargetType == TargetTypes.None)
                {
                    Target = GetTargetPlayer(aiManager.PlayerList);
                    TargetType = TargetTypes.Player;
                }

                //Debug.LogError("IsArmed(): " + IsArmed());
                //Debug.LogError("TargetType: " + TargetType.ToString());
                //Debug.LogError("TargetPosition: " + TargetPosition);
                //Debug.LogError("ReachedTargetPosition(): " + ReachedTargetPosition());


                if (!IsArmed() && TargetType == TargetTypes.Item && ReachedTargetPosition())
                {
                    Debug.LogError("KOMMIT TILL PICKUP");
                    PickupWeapon(Target);
                    currentState = AIStates.States.Unassigned;
                }
                else if (!ReachedTargetPosition())
                {
                    MoveTowards(TargetPosition);
                }
                else
                {
                    currentState = AIStates.States.Unassigned;         //Kanske kan tas bort?
                    TargetType = TargetTypes.None;
                }
                break;

            case AIStates.States.SearchingForWeapon:
                Target = GetClosestWeapon();
                TargetType = TargetTypes.Item;
                if (Target == null)
                {
                    Target = CalculateClosest(PlayerManager.players);
                    TargetType = TargetTypes.Player;
                }
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
                //Debug.Log("default");
                break;
        }
    }

    public bool ReachedTargetPosition()
    {
        return Vector3.Distance(TargetPosition, gameObject.transform.position) < 2;
    }


    private GameObject GetClosestWeapon()
    {
        Bounds bounds = aiManager.GetComponentInParent<Encounter>().GetComponent<BoxCollider>().bounds;
        GameObject closestWeapon = null;
        float closest = float.MaxValue;
        if (aiManager.AllWeapons.Count == 0)
        {
            return null;
        }

        for (int i = 0; i < aiManager.AllWeapons.Count; i++)
        {
            if (bounds.Contains(aiManager.AllWeapons[i].transform.position))
            {
                float distance = CalculateDistance(aiManager.AllWeapons[i]);
                if (distance < closest && !aiManager.AllWeapons[i].GetComponent<AbstractWeapon>().IsHeld)
                {
                    closest = distance;
                    closestWeapon = aiManager.AllWeapons[i];
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
    public GameObject CalculateClosest(List<GameObject> players)
    {
        //Debug.Log("PLAYERS " + players.Count + "       PRIORITIES " + priorites.Count);
        float closestDistance = float.MaxValue;
        GameObject closestTarget = new GameObject();

        for (int i = 0; i < players.Count; i++)
        {
            float distance = CalculateDistance(players[i]);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = players[i];
            }
        }

        return closestTarget;
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
        GameObject closest = CalculateClosest(PlayerManager.players);
        if (ReachableTarget(closest))
        {
            return true;
        }
        return false;
    }

    public void FindCover(GameObject opponent)
    {
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
                                    //Target = hit.transform.gameObject;
                                    TargetType = TargetTypes.CoverSpot;
                                    minDistToCover = Vector3.Magnitude((hit2.transform.position - opponent.transform.position));
                                }
                            }
                        }
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
        if (gameObject.GetComponentInChildren<RangedWeapon>())
        {
            return true;
        }
        return false;
    }

    public bool HoldingMeleeWeapon()
    {
        if (gameObject.GetComponentInChildren<MeleeWeapon>())
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
        //if (gameObject.GetComponentInChildren<AbstractWeapon>())
        //{
        //    return true;
        //}
        return false;
    }

    public void MoveTowards(Vector3 targetPos)
    {
        navMeshAgent.isStopped = false;

        navMeshAgent.SetDestination(targetPos);
        gameObject.GetComponent<Attributes>().Stamina -= 1 * Time.deltaTime;
        targetPosition = targetPos;
    }

    public void PickupWeapon(GameObject weapon)
    {
        gameObject.GetComponent<WeaponHand>().Equip(weapon);
        navMeshAgent.isStopped = true;
        currentState = AIStates.States.Unassigned;
        Target = GetTargetPlayer(aiManager.PlayerList);
        TargetType = TargetTypes.Player;
    }

    //To solve the issue of standing too close to shoot
    public void GetShootPosition()
    {
        //Targetplayer
        GameObject target = GetTargetPlayer(aiManager.PlayerList);
        //walk x meters in opposite direction of it
        float distance = 7;
        Vector3 oppositeDirection = -(target.transform.position - gameObject.transform.position).normalized;

        Target = null;
        TargetType = TargetTypes.ShootSpot;
    }

    public GameObject GetTargetPlayer(List<GameObject> players) // maybe only target?
    {
        //KillPriority.Clear();
        GameObject target = new GameObject();
        float maxTravelDist = GetComponent<Attributes>().Stamina * navMeshAgent.speed / 1.2f;
        float minDist = Mathf.Infinity;
        float minHealth = Mathf.Infinity;
        NavMeshPath sim_path = new NavMeshPath();

        // of all players within range, chose the one with lowest health
        // if none found walk towards the player closest to AI

        foreach (GameObject player in players)
        {
            //simulates a path from AI to player
            NavMesh.CalculatePath(transform.position, player.transform.position, NavMesh.AllAreas, sim_path);
            float pathLength = 0;
            Vector3 previousCorner = sim_path.corners[0];

            if (HoldingRangedWeapon())
            {
                //for each corner in path see if you can shoot player and break if path is bigger than what AI can travel
                for (int i = 0; i < sim_path.corners.Length - 1; i++)
                {
                    //check if path has gotten too long for stamina
                    pathLength += Vector3.Distance(previousCorner, sim_path.corners[i]);
                    if (pathLength > maxTravelDist)
                    {
                        break;
                    }
                    previousCorner = sim_path.corners[i];

                    // Raycast to player and see if hit
                    Vector3 direction = (player.transform.position - sim_path.corners[i]).normalized;
                    RaycastHit hit = new RaycastHit();

                    if (Physics.Raycast(sim_path.corners[i], direction, out hit))
                    {
                        if (hit.transform.gameObject.tag == "Player")
                        {
                            // if lowest health yet
                            if (player.GetComponent<Attributes>().Health < minHealth)
                            {
                                minHealth = player.GetComponent<Attributes>().Health;
                                target = player;
                            }
                            // OR if closest distance to travel yet
                            else if (pathLength < minDist)
                            {
                                //update target
                                minDist = Vector3.Distance(sim_path.corners[i], transform.position);
                                target = player;
                            }
                        }
                    }
                }
            }
            //Melee or unarmed should calculate travel distanse to hit and chose the one with lowest health or shortest travel distance
            else
            {
                if(ReachableTarget(player))
                {
                    // if lowest health yet
                    if (player.GetComponent<Attributes>().Health < minHealth)
                    {
                        minHealth = player.GetComponent<Attributes>().Health;
                        target = player;
                    }
                    // OR if closest distance to travel yet
                    else if (CalculateNavMeshPathLength(sim_path) < minDist)
                    {
                        //update target
                        minDist = CalculateNavMeshPathLength(sim_path);
                        target = player;
                    }
                }
            }
        }

        // If nothing reachable start walking towards closest player
        if (KillPriority.Count == 0)
        {
            target = CalculateClosest(players);
        }

        return target;
    }

    private float CalculateNavMeshPathLength(NavMeshPath path)
    {
        float dist = 0;
        Vector3 previousCorner = path.corners[0];

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            dist += Vector3.Distance(previousCorner, path.corners[i]);
            previousCorner = path.corners[i];
        }

        return dist;
    }

}
