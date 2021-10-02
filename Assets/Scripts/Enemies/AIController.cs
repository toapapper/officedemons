using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Decisions are made and the corresponding action is called

public class AIController : MonoBehaviour
{
    FieldOfView fov;
    Actions actions;
    NavMeshAgent agent;
    AIManager aiManager;
    GameObject closestPlayer;

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
    }

    // peforms the corresponding behaviour to the enemy's current state (Called in AIManager)
    public void PerformBehaviour()
    {
        // Check currentState and call corresponding Action

        aiStateHandler.UpdateState();                  

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
                agent.isStopped = true;
                break;

            case AIStates.States.Move:
                closestPlayer = CalculateClosest(PlayerManager.players);
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

        ////När action är bestämt kalla AIManager.SaveAction();
        //CurrentState = AIStates.States.Attack;
        //aiManager.SaveAction(this.gameObject);
    }

    public void PerformAction()
    {
        // Utför action för this enemy
        Debug.Log("ACTION PERFORMED");

        switch (currentState)
        {
            case AIStates.States.Attack:
                Debug.Log("he attacked");
                break;
            default:
                Debug.Log("default");
                break;
        }
        // Ta bort ifrån AIManager.actions (deque)
    }

    public GameObject CalculateClosest(List<GameObject> players)
    {
        closestPlayer = null;

        float closestDistance = float.MaxValue;

        for (int i = 0; i < players.Count; i++)
        {
            Debug.Log("PLayer position: " + players[i].transform.position);
            agent.SetDestination(players[i].transform.position);

            float distance = 0;

            for (int j = 0; j < agent.path.corners.Length - 1; j++)
            {
                distance += Vector3.Distance(agent.path.corners[j], agent.path.corners[j + 1]);
            }

            Debug.Log("distance: " + distance);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = players[i];
                Debug.Log("Closest player is " + closestDistance + " m from  " + closestPlayer + " which is the closest player");
            }
        }
        return closestPlayer;
    }
}
