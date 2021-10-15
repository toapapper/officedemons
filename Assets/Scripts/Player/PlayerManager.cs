using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static List<GameObject> players;

    //for debugging and solving strange problems....
    //is anyway just a reference to PlayerManager.players
    public List<GameObject> localPlayerList;

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

    public bool joinAnyTime = false;

    private void Awake()
    {
        instance = this;
        players = new List<GameObject>();
        localPlayerList = players;
        actions = new Queue<GameObject>();
        doneEvent = new UnityEvent();
        doneEvent.AddListener(NextPlayerAction);
    }


    private void Update()
    {
        if(GameManager.Instance.combatState == CombatState.player && actions.Count == players.Count)
        {
            GameManager.Instance.AllPlayersLockedIn();
        }
    }

    /// <summary>
    /// För att kunna spawna nya spelare in game när folk joinar. kallas av playerconfigurationmanager om joinAnyTime är sann.
    /// </summary>
    /// <param name="playerconfig"></param>
    public void SpawnNewPlayer(PlayerConfiguration playerconfig)
    {
        #region select character, it's just random atm
        GameObject playerChar = devin;
        float rand = Random.value * 3;
        rand = Mathf.Round(rand);

        switch (rand)
        {
            case 0:
                playerChar = terribleTim;
                break;
            case 1:
                playerChar = susanTheDestroyer;
                break;
            case 2:
                playerChar = devin;
                break;
            case 3:
                playerChar = viciousVicky;
                break;
        }
        #endregion

        GameObject player = Instantiate(playerChar, new Vector3(0,0,0), Quaternion.identity, transform);
        player.GetComponent<PlayerInputHandler>().InitializePlayer(playerconfig);
        player.GetComponent<PlayerInputHandler>().recentlySpawned = true;
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

    public List<GameObject> GetPlayers()
    {
        return players;
    }


    public void ActionDone(GameObject player)
    {
        actions.Enqueue(player);
    }
}
