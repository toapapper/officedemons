//Initially written by Ossian, feel free to add to it.

/*
 * Anteckningar:
 *
 * Pausa, hantera sï¿½dant.
 *
 * Hantera encounters, starta, lï¿½sa av nï¿½r de e avslutade etc.
 * enum combatState?: player, enemy, none
 * timer.
 * spelare.
 *
 * ha koll pï¿½ spelare, kanske statiskt sï¿½ kan hï¿½lla koll mellan menyer
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

    //nï¿½n static om vilka karaktï¿½rer spelare har och timer-settings, sï¿½ de kan laddas i bï¿½rjan av spelet(?)
    //Kanske ska sparas i en helt separat static-klass som kollas vid start av scen, mï¿½jligtvis hanterar denna klassen det.
    public CombatState combatState { get; private set; } = CombatState.none;
    public bool paused { get; private set; } = false;
    public float roundTimer { get; private set; } = 0;
    public Encounter currentEncounter { get; private set; }
    public bool enemiesTurnDone = false;
    public bool playerActionsDone = false;
    public bool enemiesActionsDone = false;

    public List<GameObject> stillCheckList = new List<GameObject>();

    [SerializeField]
    private bool allStill = false;
    public bool AllStill
    {
        get { return allStill; }
        set { allStill = value; }
    }
    [Header("Settings")]
    public int RoundTime = 10;//seconds


    public PlayerManager playerManager { get; private set; }

    /// <summary>
    /// fï¿½r att testa sï¿½ den vï¿½ntar tills allt stï¿½r stilla innan den gï¿½r vidare i rundan
    /// </summary>
    public GameObject testCube;

    public AIManager aiManager;


    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        aiManager = GameObject.Find("AIManager").GetComponent<AIManager>();
        roundTimer = RoundTime;
    }

    // Update is called once per frame
    void Update()
    {
        #region är alla/allt stilla-check
        AllStill = true;
        OssianUtils.CleanList(stillCheckList);
        foreach(GameObject gObject in stillCheckList)
        {
            if (gObject.CompareTag("Player"))
            {
                //if(gObject.GetComponent<CharacterController>().velocity.magnitude > 0)//fixa, det funkar inte. fråga johan hur det funkar med movement
                //{
                //    AllStill = false;
                //}
                if (gObject.GetComponent<NavMeshAgent>().velocity.magnitude > 0)
                {
                    AllStill = false;
                }
    //            if (gObject.GetComponent<Rigidbody>().velocity.magnitude > 0)
				//{
				//	AllStill = false;
				//}
			}
            else if (gObject.CompareTag("test"))//ENDAST Fï¿½R ATT TESTA
            {
                if(gObject.GetComponent<Rigidbody>().velocity.magnitude > 0)
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
            //else if projektil eller fiende eller whatever
        }
        #endregion

        #region combatState-update
        if (combatState == CombatState.player)
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

                aiManager.BeginTurn();
            }
        }
        else if(combatState == CombatState.enemy)
        {
            if (!enemiesTurnDone)
                aiManager.PerformTurn();


            if (enemiesTurnDone)
            {
                Debug.Log("ENEMY MOVE DONE");
                enemiesActionsDone = false;
                combatState = CombatState.enemyActions;
                aiManager.PerformNextAction();
            }
        }
        else if (combatState == CombatState.enemyActions)
        {
            if (enemiesActionsDone)
            {
                Debug.Log("ENEMY ACTIONS DONE");
                combatState = CombatState.player;
                playerManager.BeginTurn();
                roundTimer = RoundTime;
            }
        }
        #endregion
    }

    public void StartEncounter(Encounter encounter)
    {
        currentEncounter = encounter;
        aiManager.BeginCombat();
        combatState = CombatState.player;
        roundTimer = RoundTime;
        playerManager.BeginCombat();
    }

    public void EndEncounter()
    {
        currentEncounter.EndEncounter();
        currentEncounter = null;
        combatState = CombatState.none;
        playerManager.EndCombat();
        roundTimer = RoundTime;//fï¿½r snygghetens skull. Kanske bara borde disablea klockan iofs.
    }

    public void AllPlayersLockedIn()
    {
        roundTimer = 0;
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
        Time.timeScale = 0; //fult mï¿½hï¿½nda att anvï¿½nda timescale men ï¿½n sï¿½ lï¿½nge ï¿½r det simplast.

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
