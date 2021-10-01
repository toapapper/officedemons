using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class AIManager : MonoBehaviour
{
    public List<GameObject> enemies;
    public List<GameObject> players;
    public UnityEvent doneEvent;
    public AIManager instance;
    
    private Queue<GameObject> actions;

    private void Start()
    {
        actions = new Queue<GameObject>();
        players = PlayerManager.players;
        doneEvent = new UnityEvent();
        doneEvent.AddListener(NextAgentAction);
    }



    private void Update()
    {
        if (GameManager.Instance.combatState == CombatState.enemy)
            PerformTurn();
    }


    private void NextAgentAction()
    {
        if (actions.Count > 0)
        {
            GameObject currentAgent = actions.Dequeue();
            currentAgent.GetComponent<AIController>().PerformAction();
        }
        else
        {
            StartCoroutine("WaitDone");
        }
    }

    IEnumerator WaitDone()
    {
        yield return new WaitForSeconds(.1f);

        while (true)
        {
            if (GameManager.Instance.AllStill)
            {
                GameManager.Instance.enemiesActionsDone = true;
                StopCoroutine("WaitDone");
            }
            yield return null;
        }
    }

    public void BeginTurn() //kanske lägga till mer? annars kanske ta bort metoden
    {
        enemies = GameManager.Instance.currentEncounter.GetEnemylist();
        Debug.Log("Begin turn ENEMY");
        actions.Clear();
        foreach (GameObject e in enemies)
        {
            e.GetComponent<Attributes>().Stamina = e.GetComponent<Attributes>().StartStamina;
            e.GetComponent<AIController>().CurrentState = AIStates.States.Unassigned;
        }
        Debug.Log("KOMMER FÖRBI CLEAR");


        // enemiesTurnDone = true när alla låst in sin action
    }

    public void PerformTurn()
    {
        Debug.Log("INNE I PERFORM TURN");
        bool allDone = true;

        foreach (GameObject e in enemies)
        {
            //Debug.Log("LockedAction(): " + e.GetComponent<AIController>().LockedAction());
            if (!e.GetComponent<AIController>().LockedAction())
            {
                Debug.Log("Locked == false");
                //e.GetComponent<AIController>().CurrentState = AIStates.States.Wait; // DEBUG
                e.GetComponent<AIController>().PerformBehaviour();
                
                allDone = false;
            }
        }
        // enemiesTurnDone = true när alla låst in sin action
        if (!GameManager.Instance.AllStill)
            allDone = false;

        if (allDone)
            GameManager.Instance.enemiesTurnDone = true;
       
    }

    public void PerformActions()
    {
        Debug.Log("actions count : " + actions.Count);
        while (actions.Count > 0)
        {
            GameObject agent = actions.Dequeue();
            Debug.Log("agent : " + agent);
            agent.GetComponent<AIController>().PerformAction();
        }
        GameManager.Instance.enemiesActionsDone = true;
    }
    public void SaveAction(GameObject agent)
    {
        Debug.Log("Action is in queue");
        actions.Enqueue(agent);
    }
  
}
