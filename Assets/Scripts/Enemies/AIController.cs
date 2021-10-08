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

    public GameObject CalculateClosest(List<GameObject> players, List<GameObject> priorities)
    {
        NavMeshPath path = new NavMeshPath();

        float closestDistance = float.MaxValue;

        if (priorites.Count < 0)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] == null)
                {
                    continue;
                }
                if (NavMesh.CalculatePath(transform.position, players[i].gameObject.transform.position, agent.areaMask, path))
                {
                    float distance = Vector3.Distance(transform.position, path.corners[0]);
                    for (int j = 1; j < path.corners.Length; j++)
                    {
                        distance += Vector3.Distance(path.corners[j - 1], path.corners[j]);
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
        else
        {
            for (int i = 0; i < priorites.Count; i++)
            {
                if (NavMesh.CalculatePath(transform.position, priorites[i].gameObject.transform.position, agent.areaMask, path))
                {
                    float distance = Vector3.Distance(transform.position, path.corners[0]);
                    for (int j = 1; j < path.corners.Length; j++)
                    {
                        distance += Vector3.Distance(path.corners[j - 1], path.corners[j]);
                    }
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPlayer = priorites[i];
                    }
                }
            }
            return closestPlayer;
        }
    }
}
