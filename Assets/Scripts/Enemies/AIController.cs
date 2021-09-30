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
    private AbstractWeapon objectInHand;

    [SerializeField]
    private Animator animator;
    private PlayerStateController stateController;

    // Start is called before the first frame update
    void Start()
    {
        fov = GetComponent<FieldOfView>();
		actions = GetComponent<Actions>();
		agent = GetComponent<NavMeshAgent>();
        objectInHand = GetComponent<AbstractWeapon>();
       
    }

    // Update is called once per frame
    void Update()
    {
        List<GameObject> targetList = fov.visibleTargets;

        if (targetList.Count > 0)
        {
			//objectInHand.Hit(animator);
			//actions.Attack(objectInHand);

			MoveToTarget(targetList);
        }
    }

    public void MoveToTarget(List<GameObject> targetList)
    {
        GameObject target = targetList[targetList.Count - 1];
        agent.SetDestination(target.transform.position);
    }
}
