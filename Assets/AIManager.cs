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

    private void Awake()
    {
        players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        doneEvent = new UnityEvent();
        doneEvent.AddListener(NextAgentAction);
        
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

    //Extremt fult implementerade allihopa, men men.. Ossian som har skrivit allt detta f�rresten om ni inte pallar kolla i github efter vem som skrivit detta f�nster vars enda utsikt �r en gr� kyrkog�rd i form av kod.
    public void BeginCombat()
    {
        Debug.Log("Begin combat");
        foreach (GameObject p in players)
        {
            p.GetComponent<PlayerStateController>().StartCombat();

            Debug.Log(p.ToString());
        }
        //disabled player movement, now automatically move them somewhere,maybe, should perhaps be contained in the encounter where to

        //And begin the first turn
        BeginTurn();
    }

    //public void EndCombat()
    //{
    //    Debug.Log("End combat");
    //
    //    foreach (GameObject p in players)
    //    {
    //        p.GetComponent<PlayerStateController>().StartOutOfCombat();
    //        Debug.Log(p.ToString());
    //    }
    //}
    //

    public void BeginTurn() //kanske lägga till mer? annars kanske ta bort metoden
    {
        Debug.Log("Begin turn ENEMY");
        actions.Clear();
        Debug.Log("KOMMER FÖRBI CLEAR");


        // enemiesTurnDone = true när alla låst in sin action
    }

    public void PerformTurn()
    {
        Debug.Log("INNE I PERFORM TURN");
        bool allDone = true;
        foreach (GameObject e in enemies)
        { 
            if (!e.GetComponent<AIController>().LockedAction())
            {
                Debug.Log("allDone == false");

                e.GetComponent<AIController>().PerformBehaviour();
                allDone = false;
            }
        }

        // enemiesTurnDone = true när alla låst in sin action
        if (!allDone)
        {
            PerformTurn();
        }
        else
        {
            GameManager.Instance.enemiesTurnDone = true;
        }
        
    }

    public void PerformActions()
    {
        while (actions.Count > 0)
        {
            GameObject agent = actions.Dequeue();
            agent.GetComponent<AIController>().PerformAction();
        }
        GameManager.Instance.enemiesActionsDone = true;
    }

    private void Start()
    {
        instance = this;
        actions = new Queue<GameObject>();
    }

    public void SaveAction(GameObject agent)
    {
        actions.Enqueue(agent);
    }

    
}
