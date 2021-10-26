using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// <para>
/// Is a singleton!<br/>
/// Handles the players. contains a static list containing all the active players.<br/>
/// alerts the players when it's time to enter combat, exit combat, or wait for their turn to do an action, or just wait for the enemies to be done with their turn.
/// </para>
///   
///  <para>
///  Author: Ossian
///  
/// </para>
///  
/// </summary>

/*
 * Last Edited:
 * 15-10-2021
 */

public class PlayerManager : MonoBehaviour
{
    public static List<GameObject> players;
    public static PlayerManager Instance;

    [SerializeField] private GameObject viciousVicky;
    [SerializeField] private GameObject terribleTim;
    [SerializeField] private GameObject susanTheDestroyer;
    [SerializeField] private GameObject devin;

    [SerializeField] private bool joinAnyTime = false;
    
    /// <summary>  checked by playerConfigurationManager to know if it should run SpawnNewPlayer when a new controller joins </summary>
    public bool JoinAnyTime { get { return joinAnyTime; } } 

    private Queue<GameObject> actions;

    private void Awake()
    {
        Instance = this;
        players = new List<GameObject>();
        actions = new Queue<GameObject>();
    }


    private void Update()
    {
        int deadPlayers = 0;
        foreach(GameObject player in players)
        {
            //Im not sure this is the best way to check if a player is alive or not
            IPlayerState playerState = player.GetComponent<PlayerStateController>().CurrentState;
            if (playerState is DeadState || playerState is ReviveState)
            {
                deadPlayers++;
            }
        }
        if(GameManager.Instance.CurrentCombatState == CombatState.player && actions.Count == players.Count - deadPlayers)
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

        Vector3 spawnPos = GameObject.Find("WorldCenter").transform.position;
        if (spawnPos.Equals(null))
        {
            spawnPos = Vector3.zero;
        }

        GameObject player = Instantiate(playerChar, spawnPos, Quaternion.identity, transform);
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
                break;
            }
        }
        yield return null;
    }

    /// <summary>
    /// Signals each player that combat starts and then immediately after goes on to BeginTurn()
    /// </summary>
    public void BeginCombat()
    {
        Debug.Log("Begin combat");
        foreach (GameObject p in players)
        {
            p.GetComponent<PlayerStateController>().StartCombat();
        }

        BeginTurn();
    }

    public void EndCombat()
    {
        Debug.Log("End combat");

        foreach (GameObject p in players)
        {
            p.GetComponent<PlayerStateController>().StartOutOfCombat();
        }
    }

    public void BeginTurn()
    {
        Debug.Log("Begin turn");
        actions.Clear();
        foreach (GameObject p in players)
        {
            p.GetComponent<PlayerStateController>().StartTurn();
        }
    }

    public void EndTurn()
    {
        Debug.Log("End turn");

        foreach (GameObject p in players)
        {
            p.GetComponent<PlayerStateController>().StartWaitForTurn();
        }

        NextPlayerAction();
    }

    public void ActionDone(GameObject player)
    {
        actions.Enqueue(player);
    }
}
