using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCManager : MonoBehaviour
{ 
    public static NPCManager Instance { get; private set; }

    [SerializeField] GameObject NPCs;
    [SerializeField] GameObject Encounters;
    [SerializeField] GameObject Areas;
    [SerializeField] List<GameObject> models;
    [SerializeField] ParticleSystem poof;

    List<GameObject> npcList;
    Dictionary<int, List<Vector3>> doorPoints;
    int areaCounter = 0;
    int modelCounter = 0;
    int initialEncounterCount = 0;
    public bool spawn;

    [SerializeField]
    float timeBetweenSpawns = 3f;

    float timer = 0f;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        npcList = new List<GameObject>();
        doorPoints = new Dictionary<int, List<Vector3>>();
        spawn = true;

        initialEncounterCount = CountEncounters();

        //fill doorPoints for every key=area, value=list of points
        foreach (Transform area in Areas.transform)
        {
            List<Vector3> tempList = new List<Vector3>();

            foreach (Transform door in area)
            {
                tempList.Add(door.position);
            }

            areaCounter++;
            doorPoints.Add(areaCounter, tempList);
        }

        foreach (Transform child in NPCs.transform)
        {
            npcList.Add(child.gameObject);
        }

        Debug.Log("NPC count:" + npcList.Count);
        Debug.Log("Encounters:" + areaCounter);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (spawn && GameManager.Instance.CurrentCombatState == CombatState.enterCombat)
        {
            spawn = false;
            SendAllToExit();
        }
        else if (spawn)
        {
            timer += Time.deltaTime;
            float seconds = timer % 60;

            if (seconds >= timeBetweenSpawns) // update list
            {
                AddNPC();
                timer = 0;
            }
        }        
        
        foreach (GameObject npc in npcList)
        {
            if (npc.active)
            {
                if (npc.GetComponent<Attributes>().Health <= 0 )
                {
                    npc.GetComponent<NPCScript>().Die();
                }
                else
                {
                    npc.GetComponent<NPCScript>().NPCUpdate();
                }
            }
        }

        Debug.Log("currentArea: " + CurrentArea());
    }

    public void AddNPC()
    {
        foreach (GameObject npc in npcList)
        {
            if (!npc.active)
            {
                npc.SetActive(true);

                // get a random spawn and exit in the current area (not the same points)
                int random1 = Random.Range(0, doorPoints[CurrentArea()].Count - 1);
                int random2 = Random.Range(0, doorPoints[CurrentArea()].Count - 1);

                while (random1==random2)
                {
                    random2 = Random.Range(0, doorPoints[CurrentArea()].Count - 1);
                }

                Vector3 spawnPoint = doorPoints[CurrentArea()][random1];
                Vector3 exitPoint = doorPoints[CurrentArea()][random2];

                //GameObject model = models[Random.Range(0, models.Count - 1)];
                GameObject model = ChooseModel();
                npc.GetComponent<NPCScript>().InstantiateValues(spawnPoint, exitPoint, model);                
                break;
            }
        }
    }

    public void SendAllToExit()
    {
        Debug.Log("SEND ALL TO EXIT - WIP (Despawns all currently)");
        
        foreach (GameObject npc in npcList)
        {
            //npc.GetComponent<NPCScript>().SetTargetPosition(CalculateClosestExit(npc));
            // play poof?
            Instantiate(poof, npc.transform.position, npc.transform.rotation);
            poof.Play();
            npc.GetComponent<NPCScript>().Despawn();
        }
    }

    Vector3 CalculateClosestExit(GameObject npc)
    {
        Vector3 pos = npc.transform.position;
        Vector3 closest = Vector3.zero;

        foreach (Vector3 exit in doorPoints[CurrentArea()])
        {
            if (Vector3.Distance(exit, pos) < Vector3.Distance(closest, pos) || closest == Vector3.zero)
            {
                closest = exit;
            }
        }
        return closest;
    }

    int CurrentArea()
    {
        return (initialEncounterCount - CountEncounters() + 1);
    }

    int CountEncounters()
    {
        int count = 0;
        foreach (Transform child in Encounters.transform)
        {
            count++;
        }
        return count;
    }

    GameObject ChooseModel()
    {
        modelCounter++;
        if (modelCounter >= models.Count)
        {
            modelCounter = 0;
        }

        return models[modelCounter];
    }
}
