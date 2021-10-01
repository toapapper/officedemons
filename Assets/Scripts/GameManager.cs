//Initially written by Ossian, feel free to add to it.

/*
 * Anteckningar:
 *
 * Pausa, hantera s�dant.
 *
 * Hantera encounters, starta, l�sa av n�r de e avslutade etc.
 * enum combatState?: player, enemy, none
 * timer.
 * spelare.
 *
 * ha koll p� spelare, kanske statiskt s� kan h�lla koll mellan menyer
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public enum CombatState
{
    none,
    player,
    playerActions,
    enemy,
    enemyActions
}

public class GameManager : MonoBehaviour
{
    /// <summary> static current instance of GameManager </summary>
    public static GameManager Instance { get; private set; }

    //n�n static om vilka karakt�rer spelare har och timer-settings, s� de kan laddas i b�rjan av spelet(?)
    //Kanske ska sparas i en helt separat static-klass som kollas vid start av scen, m�jligtvis hanterar denna klassen det.
    public CombatState combatState { get; private set; } = CombatState.none;
    public bool paused { get; private set; } = false;
    public float roundTimer { get; private set; } = 0;
    public Encounter currentEncounter { get; private set; }
    public bool enemiesTurnDone = false;


    public bool playerActionsDone = false;
    public bool enemiesActionsDone = false;

    public List<GameObject> stillCheckList;

    [SerializeField]
    private bool allStill = false;
    public bool AllStill
    {
        get { return allStill; }
        set { allStill = value; }
    }

    /// <summary> DONT TOUCH, unless you want the rounds to go quicker of course </summary>
    [Header("Settings")]
    public int RoundTime = 10;//seconds

    /// <summary>
    /// f�r att testa s� den v�ntar tills allt st�r stilla innan den g�r vidare i rundan
    /// </summary>
    public GameObject testCube;

    public PlayerManager playerManager { get; private set; }
    public AIManager aiManager;


    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        aiManager = GameObject.Find("AIManager").GetComponent<AIManager>();
        //stillCheckList = new List<GameObject>();
        stillCheckList.AddRange(PlayerManager.players);
        //stillCheckList.AddRange();
        Instance = this;
        roundTimer = RoundTime;
    }

    // Update is called once per frame
    void Update()
    {
        //�r alla/allt stilla-check
        AllStill = true;
        foreach(GameObject gObject in stillCheckList)
        {
            if (gObject.CompareTag("Player"))
            {
                if(gObject.GetComponent<CharacterController>().velocity.magnitude > 0)
                {
                    AllStill = false;
                }
            }
            else if (gObject.CompareTag("test"))//ENDAST F�R ATT TESTA
            {
                if(gObject.GetComponent<Rigidbody>().velocity.magnitude > 0)
                {
                    AllStill = false;
                }
            }
            else if (gObject.CompareTag("Enemy"))
            {
                if (gObject.GetComponent<Rigidbody>().velocity.magnitude > 0)
                {
                    AllStill = false;
                }
            }
            //else if projektil eller fiende eller whatever
        }

        if(combatState == CombatState.player)
        {
            roundTimer -= Time.deltaTime;
            if(roundTimer <= 0)
            {
                Debug.Log("PLAYER MOVE DONE");
                playerActionsDone = false;
                combatState = CombatState.playerActions;

                if(testCube != null)//TEST
                    testCube.GetComponent<Rigidbody>().AddForce(testCube.transform.up * 1000);

                playerManager.EndTurn();
            }
        }
        else if(combatState == CombatState.playerActions)
        {
            if (playerActionsDone)
            {
                Debug.Log("PLAYER ACTIONS DONE");
                combatState = CombatState.enemy;
                enemiesTurnDone = false;

                //aiManager.BeginTurn();
            }
        }
        else if(combatState == CombatState.enemy)
        {
            Debug.Log("INNE GAMEMANAGER CURRENTSTAE == ENEMY (MOVE)");

            aiManager.PerformTurn();

            enemiesTurnDone = true; // DeBuG


            if (enemiesTurnDone)
            {
                Debug.Log("ENEMY MOVE DONE");
                enemiesActionsDone = false;
                combatState = CombatState.enemyActions;
                roundTimer = RoundTime;

            }
        }
        else if (combatState == CombatState.enemyActions)
        {
            //aiManager.PerformActions();
            enemiesActionsDone = true;

            if (enemiesActionsDone)
            {
                Debug.Log("ENEMY ACTIONS DONE");
                combatState = CombatState.player;
                playerManager.BeginTurn();
                enemiesTurnDone = true;
            }
        }
    }

    public void StartEncounter(Encounter encounter)
    {
        currentEncounter = encounter;
        combatState = CombatState.player;
        roundTimer = RoundTime;
        playerManager.BeginCombat();
    }

    public void EndEncounter()
    {
        currentEncounter = null;
        combatState = CombatState.none;
        playerManager.EndCombat();
        roundTimer = RoundTime;//f�r snygghetens skull. Kanske bara borde disablea klockan iofs.
    }


    //toggles between pause and unpause
    public void OnPause()
    {
        if (!paused)
            Pause();
        else
            Unpause();
    }

    public void Pause()
    {
        if (paused)
            return;

        UIManager.Instance.OpenMenu();
        Time.timeScale = 0; //fult m�h�nda att anv�nda timescale men �n s� l�nge �r det simplast.

        paused = true;
    }

    public void Unpause()
    {
        if (!paused)
            return;

        UIManager.Instance.CloseMenu();
        Time.timeScale = 1;

        paused = false;
    }

    public List<GameObject> GetPlayers()
    {
        return PlayerManager.players;
    }
}
