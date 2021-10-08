using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Decisions are made and the corresponding action is called
//Used more for behaviours than anything.
//No other uses other than to make different decisions based on the class
public enum Class { Aggresive, Defensive, Healer};

public class AIController : MonoBehaviour
{
    FieldOfView fov;
    Actions actions;
    NavMeshAgent agent;
    AIManager aiManager;
    GameObject closestPlayer;
    WeaponHand weapon;
    public GameObject target;
    public Class currentClass;


    List<GameObject> priorites;
    public List<GameObject> Priorites
    {
        get { return priorites; }
        set { priorites = value; }
    }

    [SerializeField]
    private Animator animator;
    private AIStateHandler aiStateHandler;

    AIStates.States currentState;

    public AIStates.States CurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }

    public bool LockedAction()
    {
        if (CurrentState == AIStates.States.Attack || CurrentState == AIStates.States.Wait)
        {
            return true;
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        fov = GetComponent<FieldOfView>();
        actions = GetComponent<Actions>();
        agent = GetComponent<NavMeshAgent>();
        CurrentState = AIStates.States.Unassigned;
        aiStateHandler = GetComponent<AIStateHandler>();
        aiManager = transform.parent.GetComponentInChildren<AIManager>();
        weapon = GetComponent<WeaponHand>();
    }

    // peforms the corresponding behaviour to the enemy's current state (Called in AIManager)
    public void PerformBehaviour()
    {
        // Check currentState and call corresponding Action

        aiStateHandler.GetState(currentClass);

        switch (CurrentState) // FindCover, CallForHealing, Attack, Move, Wait , Unassigned
        {
            case AIStates.States.FindCover:
                // Kan inte hända just nu, inte implementerat
                break;

            case AIStates.States.CallForHealing:
                // Kan inte hända just nu, inte implementerat
                break;

            case AIStates.States.Attack:
                aiManager.SaveAction(this.gameObject);
                agent.destination = transform.position;
                weapon.StartAttack();
                agent.isStopped = true;
                break;

            case AIStates.States.Move:
                closestPlayer = CalculateClosest(PlayerManager.players, priorites);
                if (closestPlayer == null)
                    currentState = AIStates.States.Wait;

                actions.MoveTowards(agent, closestPlayer);
                break;

            case AIStates.States.Wait:
                aiManager.SaveAction(this.gameObject);
                agent.destination = transform.position;
                agent.isStopped = true;
                break;

        }
    }

    public void PerformAction()
    {
        // Utför action för this enemy
        Debug.Log("ACTION PERFORMED");

        switch (currentState)
        {
            case AIStates.States.Attack:
                weapon.Attack();
                Debug.Log("he attacked");
                break;
            default:
                Debug.Log("default");
                break;
        }
        // Ta bort ifrån AIManager.actions (deque)
    }



    //A lot of checks atm might need some cleanups!
    //If we can have the player itself notify when it's in down state that would also work
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
            Debug.Log("No priority player is reachable");
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

            if (closestDistance == float.MaxValue)
                Debug.Log("No player is reachable");

            Debug.LogError(closestDistance);
            return closestPlayer;
        }
        return closestPlayer;
    }

    public bool ReachableTarget(GameObject target)
    {
        float stamina = agent.gameObject.GetComponent<Attributes>().Stamina;

        float targetDistance = CalculateDistance(target);

        //With some testing: with acceleration 10 we get about 1m * speed per stamina 
        //Turns make it take longer
        //made the estimated travel distance 20% longer to ensure that if we think we can make it we make it
        //Right now it does not take into consideration FOV
        //Check the last path from calculatedistance if that path is less than fov.viewradius then subtract the last path.length from targetdistance
        if (targetDistance <= stamina * agent.speed / 1.2f)
        {
            return true;
        }

        return false;
    }
    private float CalculateDistance(GameObject target)
    {
        NavMeshPath path = new NavMeshPath();
        float distance = float.MaxValue;

        if (NavMesh.CalculatePath(transform.position, target.gameObject.transform.position, agent.areaMask, path))
        {
            distance = Vector3.Distance(transform.position, path.corners[0]);
            for (int i = 1; i < path.corners.Length; i++)
            {
                distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }

        }
        return distance;
    }
}
