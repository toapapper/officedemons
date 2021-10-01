//Initially written by Ossian, feel free to add to it.

/*
 * Anteckningar:
 * 
 * Pausa, hantera sådant.
 *  
 * Hantera encounters, starta, läsa av när de e avslutade etc.
 * enum combatState?: player, enemy, none
 * timer.
 * spelare.
 * 
 * ha koll på spelare, kanske statiskt så kan hålla koll mellan menyer
 * 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CombatState
{
    none,
    player,
    playerActions,
    enemy
}

public class GameManager : MonoBehaviour
{
    /// <summary> static current instance of GameManager </summary>
    public static GameManager Instance { get; private set; } 

    //nån static om vilka karaktärer spelare har och timer-settings, så de kan laddas i början av spelet(?)   
    //Kanske ska sparas i en helt separat static-klass som kollas vid start av scen, möjligtvis hanterar denna klassen det.
    public CombatState combatState { get; private set; } = CombatState.none;
    public bool paused { get; private set; } = false;
    public float roundTimer { get; private set; } = 0;
    public Encounter currentEncounter { get; private set; }
    public bool enemiesTurnDone = false;


    public bool playerActionsDone = false;

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
    /// för att testa så den väntar tills allt står stilla innan den går vidare i rundan
    /// </summary>
    public GameObject testCube;

    public PlayerManager playerManager { get; private set; }
    

    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        //stillCheckList = new List<GameObject>();
        stillCheckList.AddRange(PlayerManager.players);
        Instance = this;
        roundTimer = RoundTime;
    }

    // Update is called once per frame
    void Update()
    {
        //är alla/allt stilla-check
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
            else if (gObject.CompareTag("test"))//ENDAST FÖR ATT TESTA
            {
                if(gObject.GetComponent<Rigidbody>().velocity.magnitude > 0)
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
                combatState = CombatState.enemy;
                currentEncounter.EnemiesTurn();
                enemiesTurnDone = false;
            }
        }
        else if(combatState == CombatState.enemy)
        {
            if (enemiesTurnDone)
            {
                playerManager.BeginTurn();
                enemiesTurnDone = false;
                combatState = CombatState.player;
                roundTimer = RoundTime;
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
        roundTimer = RoundTime;//för snygghetens skull. Kanske bara borde disablea klockan iofs.
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
        Time.timeScale = 0; //fult måhända att använda timescale men än så länge är det simplast.

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
    
   
}
