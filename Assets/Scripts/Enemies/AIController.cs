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

        Debug.Log("INNE I PERFORMBEHAVIOUR");
        //aiStateHandler.UpdateState();                  
        Debug.Log("CURRENTSTATE: " + CurrentState);

        //switch (CurrentState) // FindCover, CallForHealing, Attack, Move, Wait , Unassigned
        //{
        //    case AIStates.States.FindCover:
        //        // Kan inte hända just nu, inte implementerat
        //        break;
        //
        //    case AIStates.States.CallForHealing:
        //        // Kan inte hända just nu, inte implementerat
        //        break;
        //
        //    case AIStates.States.Attack:
        //
        //        aiManager.SaveAction(this.gameObject);
        //        break;
        //
        //    case AIStates.States.Move:
        //        
        //        GameObject closestPlayer = CalculateClosest(GameManager.Instance.GetPlayers());
        //
        //        actions.MoveTowards(agent, closestPlayer);
        //        break;
        //
        //    case AIStates.States.Wait:
        //
        //        aiManager.SaveAction(this.gameObject);
        //        break;
        //
        //}

        //När action är bestämt kalla AIManager.SaveAction();
        CurrentState = AIStates.States.Wait;                        
    }

    public void PerformAction()
    {
        // Utför action för this enemy
        Debug.Log("ACTION PERFORMED");

        
        // Ta bort ifrån AIManager.actions (deque)
    }

    public GameObject CalculateClosest(List<GameObject> players)
    {
        

        GameObject closestPlayer = this.gameObject;
        float closestDistance = float.MaxValue;

        for (int i=0; i<players.Count; i++)
        {
            Debug.Log("PLayer position: " + players[i].transform.position);
            agent.SetDestination(players[i].transform.position);
            float f = agent.remainingDistance;
            Debug.Log("f: " + f);

            if (f < closestDistance)
            {
                closestDistance = f;
                closestPlayer = players[i];
            }
        }
        return closestPlayer;
    }
}
