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

    [SerializeField]
    private Animator animator;
    private PlayerStateController stateController;

    AIStates.States currentState;

    public AIStates.States CurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }

    

    // Start is called before the first frame update
    void Start()
    {
        fov = GetComponent<FieldOfView>();
		actions = GetComponent<Actions>();
		agent = GetComponent<NavMeshAgent>();
        CurrentState = AIStates.States.Unassigned;
    }

    // Update is called once per frame
    void Update()
    {
        // Check currentState and call corresponding Action

        // if currentState == MOve eller Unnasigned, FindCover, kalla på AIStateHandler

    }

    
}
