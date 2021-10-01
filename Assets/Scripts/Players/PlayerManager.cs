using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    public static List<GameObject> players;

    public static UnityEvent doneEvent;

    [SerializeField]
    GameObject viciousVicky;
    [SerializeField]
    GameObject terribleTim;
    [SerializeField]
    GameObject susanTheDestroyer;
    [SerializeField]
    GameObject devin;

    public static PlayerManager instance;
    private Queue<GameObject> actions;

    private void Awake()
    {
        players = new List<GameObject>();
        doneEvent = new UnityEvent();
        doneEvent.AddListener(NextPlayerAction);
    }

    private void NextPlayerAction()
    {
        if(actions.Count > 0)
        {
            GameObject currentPlayer = actions.Dequeue();
            currentPlayer.GetComponent<PlayerStateController>().StartCombatAction();
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
                GameManager.Instance.playerActionsDone = true;
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

    public void EndCombat()
    {
        Debug.Log("End combat");

        foreach (GameObject p in players)
        {
            p.GetComponent<PlayerStateController>().StartOutOfCombat();
            Debug.Log(p.ToString());
        }
    }

    public void BeginTurn()
    {
        Debug.Log("Begin turn");
        actions.Clear();
        foreach (GameObject p in players)
        {
            p.GetComponent<PlayerStateController>().StartTurn();
            Debug.Log(p.ToString());
        }
    }

    public void EndTurn()
    {
        Debug.Log("End turn");

        foreach (GameObject p in players)
        {
            Debug.Log(p.ToString());
            p.GetComponent<PlayerStateController>().StartWaitForTurn();//borde antagligen göra actions och så istället
        }

        NextPlayerAction();
    }

    private void Start()
    {
        instance = this;
        actions = new Queue<GameObject>();
    }



    public void ActionDone(GameObject player)
    {
        actions.Enqueue(player);
    }
}
