using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

/// <summary>
/// Whose turn it is and when they are doing an action, none means no combat
/// </summary>
public enum CombatState
{
    none,
    player,
    playerActions,
    enemy,
    enemyActions
}

/// <summary>
/// <para>
/// A singleton managing if it's the enemies turn or the players turn or you just aren't in combat.<br/>
/// Also handles the turn timer and checks whether all is still.
/// </para>
///   
/// <para>
///  Author: Ossian
/// </para>
///  
/// </summary>

/*
 * Last Edited:
 * 15-10-2021
 */
public class GameManager : MonoBehaviour
{
    /// <summary> static current instance of GameManager </summary>
    public static GameManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private int roundTime = 10;//seconds

    private CombatState combatState = CombatState.none;
    private bool paused = false;
    private float roundTimer = 0;

    private bool enemiesTurnDone = false;
    private bool playerActionsDone = false;
    private bool enemiesActionsDone = false;
    private bool allStill = false;

    private Encounter currentEncounter;
    private List<GameObject> stillCheckList = new List<GameObject>();
    
    private MultipleTargetCamera mainCamera;

    public CombatState CurrentCombatState { get { return combatState; } }
    public bool Paused { get { return paused; } }
    public float RoundTimer { get { return roundTimer; } }
    public Encounter CurrentEncounter { get { return currentEncounter; } }
    public bool EnemiesTurnDone { set { enemiesTurnDone = value; } }
    public bool PlayerActionsDone { set { playerActionsDone = value; } }
    public bool EnemiesActionsDone { set { enemiesActionsDone = value; } }
    public List<GameObject> StillCheckList { get { return stillCheckList; } }
    [SerializeField] public bool AllStill
    {
        get { return allStill; }
        set { allStill = value; }
    }
    public int RoundTime { get { return roundTime; } }


    void Awake()
    {
        Instance = this;
        roundTimer = RoundTime;

        // Add maincamera to gamemanager
        mainCamera = Camera.main.GetComponent<MultipleTargetCamera>();
        //Debug.LogError("AAAAA " + mainCamera);
    }

    void Update()
    {
        #region are all the needed gameObjects still-check
        AllStill = true;
        Utilities.CleanList(stillCheckList);
        foreach(GameObject gObject in stillCheckList)
        {
            if (gObject.CompareTag("Player"))
            {
                if (gObject.GetComponent<NavMeshAgent>().velocity.magnitude > 0)
                {
                    AllStill = false;
                }
			}
            else if (gObject.CompareTag("Enemy"))
            {
                if (gObject.GetComponent<NavMeshAgent>().velocity.magnitude > 0)
                {
                    AllStill = false;
                }
            }
            //continue with more else ifs for different types of gameObjects
        }
        #endregion

        #region combatState-update
        if (CurrentCombatState == CombatState.player)
        {
            roundTimer -= Time.deltaTime;
            if(RoundTimer <= 0)
            {
                Debug.Log("PLAYER MOVE DONE");
                playerActionsDone = false;
                combatState = CombatState.playerActions;

                PlayerManager.Instance.EndTurn();
            }
        }
        else if(CurrentCombatState == CombatState.playerActions)
        {
            if (playerActionsDone)
            {
                Debug.Log("PLAYER ACTIONS DONE");
                combatState = CombatState.enemy;
                EnemiesTurnDone = false;
                currentEncounter.aIManager.BeginTurn();
            }
        }
        else if(CurrentCombatState == CombatState.enemy)
        {
            if (!enemiesTurnDone)
            {
                currentEncounter.aIManager.PerformTurn();
            }

            if (enemiesTurnDone)
            {
                Debug.Log("ENEMY MOVE DONE");
                EnemiesActionsDone = false;
                combatState = CombatState.enemyActions;
                currentEncounter.aIManager.PerformNextAction();
            }
        }
        else if (CurrentCombatState == CombatState.enemyActions)
        {
            if (enemiesActionsDone)
            {
                Debug.Log("ENEMY ACTIONS DONE");
                combatState = CombatState.player;
                PlayerManager.Instance.BeginTurn();
                roundTimer = RoundTime;
            }
        }
        #endregion
    }

    /// <summary>
    /// Start this encounter and begin combat
    /// </summary>
    /// <param name="encounter"></param>
    public void StartEncounter(Encounter encounter)
    {
        currentEncounter = encounter;
        currentEncounter.aIManager.BeginCombat();
        combatState = CombatState.player;
        roundTimer = RoundTime;
        PlayerManager.Instance.BeginCombat();
        // Add all objects in checklist to maincamera
        mainCamera.ObjectsInCamera = stillCheckList; 
    }

    /// <summary>
    /// End the current encounter and transition to being out of combat
    /// </summary>
    public void EndEncounter()
    {
        CurrentEncounter.EndEncounter();
        currentEncounter = null;
        combatState = CombatState.none;
        PlayerManager.Instance.EndCombat();
        roundTimer = RoundTime;
        // Remove everything but players from the camera
        mainCamera.ObjectsInCamera = PlayerManager.players;
    }

    /// <summary>
    /// Signals that its time to end the turn before the timer runs down to zero because all the players have locked in their actions
    /// </summary>
    public void AllPlayersLockedIn()
    {
        roundTimer = 0;
    }

    /// <summary>
    /// Toggles between pausing and unpausing the game
    /// </summary>
    public void OnPause()
    {
        if (!paused)
            Pause();
        else
            Unpause();
    }

    /// <summary>
    /// Pauses the game
    /// </summary>
    public void Pause()
    {
        if (paused)
            return;

        UIManager.Instance.OpenMenu();
        Time.timeScale = 0; //might be bad to set the timescale, but it's the most convenient as of now.

        paused = true;
    }

    /// <summary>
    /// Unpauses the game
    /// </summary>
    public void Unpause()
    {
        if (!paused)
            return;

        UIManager.Instance.CloseMenu();
        Time.timeScale = 1;

        paused = false;
    }
}
