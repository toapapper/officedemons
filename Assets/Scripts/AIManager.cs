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
    }


    public void BeginCombat()
    {
        enemies = GameManager.Instance.currentEncounter.GetEnemylist();
        GameManager.Instance.stillCheckList.AddRange(enemies);
    }

    public void BeginTurn() //kanske lägga till mer? annars kanske ta bort metoden
    {
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
        bool allDone = true;
        bool allDead = true;

        List<GameObject> killOnSight = new List<GameObject>();

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].GetComponent<Attributes>().Health <= players[i].GetComponent<Attributes>().StartHealth / 3
                && players[i].GetComponent<Attributes>().Health > 0)
            {
                killOnSight.Add(players[i]);
            }
        }


        foreach (GameObject e in enemies)
        {
            //Debug.Log("LockedAction(): " + e.GetComponent<AIController>().LockedAction());
            if (!e.GetComponent<AIController>().LockedAction())
            {
                //e.GetComponent<AIController>().CurrentState = AIStates.States.Wait; // DEBUG
                e.GetComponent<AIController>().Priorites = killOnSight;
                e.GetComponent<AIController>().PerformBehaviour();
                
                allDone = false;
            }

            if (e.GetComponent<Attributes>().Health > 0)
            {
                allDead = false;
            }
        }
        // enemiesTurnDone = true när alla låst in sin action
        if (!GameManager.Instance.AllStill)
            allDone = false;

        if (allDone)
            GameManager.Instance.enemiesTurnDone = true;

        if (allDead)
            GameManager.Instance.EndEncounter();
    }

    public void PerformNextAction()
    {
        if (actions.Count > 0)
        {
            GameObject agent = actions.Dequeue();
            agent.GetComponent<AIController>().PerformAction();
            StartCoroutine("WaitDone");
        }
        else
        {
            GameManager.Instance.enemiesActionsDone = true;
        }
    }
    IEnumerator WaitDone()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            if (GameManager.Instance.AllStill)
            {
                StopCoroutine("WaitDone");
                PerformNextAction();
            }
            yield return null;
        }
    }



    public void SaveAction(GameObject agent)
    {
        Debug.Log("Action is in queue");
        actions.Enqueue(agent);
    }
  
}
