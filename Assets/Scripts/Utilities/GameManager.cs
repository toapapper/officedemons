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
    enemyActions,
    enterCombat
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

    [SerializeField]private CombatState combatState = CombatState.none;
    private bool paused = false;
    private float roundTimer = 0;

    private bool enemiesTurnDone = false;
    private bool playerEnterCombatDone = false;
    private bool playerActionsDone = false;
    private bool enemiesActionsDone = false;
    private bool allStill = false;

    private Encounter currentEncounter;
    private List<GameObject> stillCheckList = new List<GameObject>();
    private List<GameObject> groundEffectObjects = new List<GameObject>();

    private MultipleTargetCamera mainCamera;

    public CombatState CurrentCombatState { get { return combatState; } }
    public bool Paused { get { return paused; } }
    public float RoundTimer { get { return roundTimer; } }
    public Encounter CurrentEncounter { get { return currentEncounter; } }
    public bool EnemiesTurnDone { set { enemiesTurnDone = value; } }
    public bool PlayerActionsDone { set { playerActionsDone = value; } }
    public bool PlayerEnterCombatDone { set { playerEnterCombatDone = value; } }
    public bool EnemiesActionsDone { set { enemiesActionsDone = value; } }
    public List<GameObject> StillCheckList { get { return stillCheckList; } }
    public List<GameObject> GroundEffectObjects { get { return groundEffectObjects; } }
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
    }

    void Update()
    {
        #region are all the needed gameObjects still-check
        allStill = true;
        Utilities.CleanList(stillCheckList);
        foreach(GameObject gObject in stillCheckList)
        {
            if (gObject.CompareTag("Projectile"))
            {
                allStill = false;
            }
        }
        #endregion


        //Ful "kolla om alla fiender är döda"-check
        if(CurrentCombatState != CombatState.none)
        {

            if(currentEncounter.GetEnemylist().Count <= 0)
            {
                EndEncounter();
            }
        }
		else
		{
			for (int i = 0; i < GroundEffectObjects.Count; i++)
			{
                GroundEffectObjects[i].GetComponent<GroundEffectObject>().UpdateTime();

            }
		}

        #region combatState-update
        if(CurrentCombatState == CombatState.enterCombat)
		{
            if (playerEnterCombatDone)
			{
                playerEnterCombatDone = false;
                currentEncounter.aIManager.BeginCombat();
                combatState = CombatState.player;
                roundTimer = RoundTime;
                PlayerManager.Instance.BeginTurn();
                // Add all objects in checklist to maincamera
                //mainCamera.ObjectsInCamera = stillCheckList;
                //Add encounter corner points to camera to fix it to encounter
                mainCamera.ObjectsInCamera = currentEncounter.GetCameraPoints();
            }
        }
        else if (CurrentCombatState == CombatState.player)
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
                Debug.Log("ENEMIES MOVES ARE DONE");
                EnemiesActionsDone = false;
                combatState = CombatState.enemyActions;

                if(currentEncounter != null)//to fix the nullreference error that happens when an encounter is ended
                {
                    currentEncounter.aIManager.PerformNextAction();
                }
            }
        }
        else if (CurrentCombatState == CombatState.enemyActions)
        {
            if (enemiesActionsDone)
            {
                Debug.Log("ENEMIES ACTIONS ARE DONE");
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
		combatState = CombatState.enterCombat;
        PlayerManager.Instance.BeginCombat();
    }

    /// <summary>
    /// End the current encounter and transition to being out of combat
    /// </summary>
    public void EndEncounter()
    {
        Debug.Log("ENDENCOUNTER");

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
