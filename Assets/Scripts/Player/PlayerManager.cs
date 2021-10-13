using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static List<GameObject> players;
    public static PlayerManager Instance;

    [SerializeField] private GameObject viciousVicky;
    [SerializeField] private GameObject terribleTim;
    [SerializeField] private GameObject susanTheDestroyer;
    [SerializeField] private GameObject devin;

    [SerializeField] private bool joinAnyTime = false; 
    public bool JoinAnyTime { get { return joinAnyTime; } } //checked by playerConfigurationManager to know if it should run SpawnNewPlayer when a new controller joins

    private Queue<GameObject> actions;

    private void Awake()
    {
        Instance = this;
        players = new List<GameObject>();
        actions = new Queue<GameObject>();
    }


    private void Update()
    {
        if(GameManager.Instance.CurrentCombatState == CombatState.player && actions.Count == players.Count)
        {
            GameManager.Instance.AllPlayersLockedIn();
        }
    }

    /// <summary>
    /// Spawns a new player, for use with "join any time" - joining
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

    /// <summary>
    /// Signal the next player in line to do their action.<br/> 
    /// If no players remain, wait untill all is still in the scene and then signal the gamemanager
    /// </summary>
    public void NextPlayerAction()
    {
        if(actions.Count > 0)
        {
            GameObject currentPlayer = actions.Dequeue();
            currentPlayer.GetComponent<PlayerStateController>().StartCombatAction();
        }
        else
        {
            StartCoroutine(WaitDone());
        }
    }

    /// <summary>
    /// Waits for .1 seconds and untill all gameObjects are still. It then signals the gamemanager that all players are done
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitDone()
    {
        yield return new WaitForSeconds(.1f);

        while (true)
        {
            if (GameManager.Instance.AllStill)
            {
                GameManager.Instance.PlayerActionsDone = true;
                StopCoroutine("WaitDone");
            }
            yield return null;
        }
    }

    public void BeginCombat()
    {
        Debug.Log("Begin combat");
        foreach (GameObject p in players)
        {
            p.GetComponent<PlayerStateController>().StartCombat();
            Debug.Log(p.ToString());
        }

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

    public void ActionDone(GameObject player)
    {
        actions.Enqueue(player);
    }
}
