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
    
    private AIManager aiManager;
    private WeaponHand weaponHand;
    private const float movingSpeed = 5;
    private const float staminaDrainFactor = 0.4f;

    private NavMeshAgent navMeshAgent; // TODO: Maybe change to property

    public NavMeshAgent NMAgent
    {
        get { return navMeshAgent; }
        set { navMeshAgent = value; }
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

    private void ResetTarget()
    {
        Target = null;
        TargetPosition = Vector3.zero;
        TargetType = TargetTypes.None;
    }

    private void ResetNavMeshAgent()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = movingSpeed;
    }

    void Start()
    {
        ResetTarget();
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        fov = GetComponent<FieldOfView>();
        ResetNavMeshAgent();
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

        GameObject skeleton = GameManager.Instance.Skeleton;
        if(skeleton != null)
        {
            skeleton = Instantiate(skeleton, transform.position, transform.rotation);
            skeleton.transform.parent = GameObject.Find("Skeletons").transform;
        }

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
                    GameObject closestPlayer = GetClosestPlayer(aiManager.PlayerList);
                    Vector3 coverPos = FindCover(closestPlayer);

                    if (coverPos == Vector3.zero) //couldn't find any free cover spot
                    {
                        SetTarget(GetTargetPlayer(aiManager.PlayerList), TargetTypes.Player);
                        CurrentState = AIStates.States.Attack;
                    }
                    else
                    {
                        SetTarget(null, TargetTypes.CoverSpot, coverPos);
                    }
                }

                if (TargetType != TargetTypes.None && ReachedTargetPosition())
                {
                    currentState = AIStates.States.Wait;
                }
                else
                {
                    MoveTowards(TargetPosition);
                    currentState = AIStates.States.Move;
                }
                break;

            case AIStates.States.Attack:
                aiManager.SaveAction(this.gameObject);
                Debug.Log("Attack");
                break;

            case AIStates.States.Move:
                if (TargetPosition == Vector3.zero || TargetType == TargetTypes.None)
                {
                    SetTarget(GetTargetPlayer(aiManager.PlayerList), TargetTypes.Player);
                }

                if (!IsArmed() && TargetType == TargetTypes.Item && ReachedTargetPosition())
                {
                    PickupWeapon(Target);
                    currentState = AIStates.States.Unassigned;
                    ResetTarget();
                }
                else if (!ReachedTargetPosition())
                {
                    MoveTowards(TargetPosition);
                }
                //else
                //{
                //    currentState = AIStates.States.Unassigned;         //Kanske kan tas bort?
                //    ResetTarget();
                //}
                if (ReachedTargetPosition() && TargetType == TargetTypes.Player)
                {
                    currentState = AIStates.States.Attack;
                }

                break;

            case AIStates.States.SearchingForWeapon:

                GameObject closestWeapon = GetClosestWeapon();

                if (closestWeapon == null) // no weapon found
                {
                    SetTarget(GetClosestPlayer(PlayerManager.players), TargetTypes.Player);
                }
                else
                {
                    SetTarget(closestWeapon, TargetTypes.Item);
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
        return Vector3.Distance(TargetPosition, gameObject.transform.position) <= 3;
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
            if (aiManager.AllWeapons[i] != null && bounds.Contains(aiManager.AllWeapons[i].transform.position) && aiManager.AllWeapons[i].GetComponent<AbstractWeapon>() != null)
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
    public GameObject GetClosestPlayer(List<GameObject> players)
    {
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
        //float lastPathDistance = CalculateLastPathDistance(target);

        if (targetDistance <= fov.ViewRadius)
        {
            if (targetDistance <= stamina * navMeshAgent.speed / movingSpeed)
            {
                return true;
            }
        }
        else if (targetDistance - fov.ViewRadius <= stamina * navMeshAgent.speed / movingSpeed)
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
        GameObject closest = GetClosestPlayer(PlayerManager.players);
        if (ReachableTarget(closest))
        {
            return true;
        }
        return false;
    }

    public Vector3 FindCover(GameObject opponent)
    {
        RaycastHit hit = new RaycastHit();
        float minDistToCover = Mathf.Infinity;
        Vector3 currentFoundBestPos = Vector3.zero;

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
                                    currentFoundBestPos = child.position;
                                    aiManager.TakenCoverPositions.Add(child.position);
                                    
                                    minDistToCover = Vector3.Magnitude((hit2.transform.position - opponent.transform.position));
                                }
                            }
                        }
                    }
                }
            }
        }
        return currentFoundBestPos;
    }

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
        return false;
    }

    public void MoveTowards(Vector3 targetPos)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(targetPos);
        gameObject.GetComponent<Attributes>().Stamina -= (staminaDrainFactor * movingSpeed) * Time.deltaTime;
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
    public Vector3 GetShootPosition()
    {
        //Targetplayer
        GameObject target = GetTargetPlayer(aiManager.PlayerList);
        //walk x meters in opposite direction of it
        float distance = 2;
        Vector3 oppositeDirection = -(target.transform.position - gameObject.transform.position) * distance;

        return oppositeDirection;
    }

    public GameObject GetTargetPlayer(List<GameObject> players) // maybe only target?
    {
        GameObject target = null;
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
        if (target == null)
        {
            target = GetClosestPlayer(players);
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

    public void SetTarget(GameObject target, TargetTypes type, Vector3 position = default(Vector3))
    {
        TargetType = type;

        switch(type)
        {
            case TargetTypes.CoverSpot:
                TargetPosition = position;
                Target = null;
                break;

            case TargetTypes.ShootSpot:
                TargetPosition = position;
                Target = null;
                break;

            case TargetTypes.Item:
                TargetPosition = target.transform.position;
                Target = target;
                break;

            case TargetTypes.Player:
                TargetPosition = target.transform.position;
                Target = target;
                break;

            case TargetTypes.None:
                TargetPosition = Vector3.zero;
                Target = null;
                break;
        }
    }
}
