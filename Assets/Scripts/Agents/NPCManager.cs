using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCManager : MonoBehaviour
{ 
    public static NPCManager Instance { get; private set; }

    [SerializeField] GameObject NPCs;

    [SerializeField] List<GameObject> spawnPoints;
                         
    [SerializeField] List<GameObject> exitPoints;

    [SerializeField] List<GameObject> models;

    List<GameObject> npcList;
    List<Vector3> spawnPointPositions;
    List<Vector3> exitPointPositions;

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
        spawnPointPositions = new List<Vector3>();
        exitPointPositions = new List<Vector3>();
        spawn = true;

        foreach (GameObject go in spawnPoints)
        {
            spawnPointPositions.Add(go.transform.position);
        }

        foreach (GameObject go in exitPoints)
        {
            exitPointPositions.Add(go.transform.position);
        }

        foreach (Transform child in NPCs.transform)
        {
            npcList.Add(child.gameObject);
        }
        Debug.Log("NPC count:" + npcList.Count);
    }

    // Update is called once per frame
    void Update()
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
                    npc.GetComponent<NPCScript>().Update();
                }
            }
        }
    }

    public void AddNPC()
    {
        foreach (GameObject npc in npcList)
        {
            if (!npc.active)
            {
                npc.SetActive(true);
                Vector3 spawnPoint = spawnPointPositions[Random.Range(0, spawnPoints.Count - 1)];
                Vector3 exitPoint = exitPointPositions[Random.Range(0, exitPoints.Count - 1)];
                GameObject model = models[Random.Range(0, models.Count - 1)];
                npc.GetComponent<NPCScript>().InstantiateValues(spawnPoint, exitPoint, model);                
                break;
            }
        }
    }

    public void SendAllToExit()
    {
        Debug.Log("SEND ALL TO EXIT");
        foreach(GameObject npc in npcList)
        {
            npc.GetComponent<NPCScript>().SetTargetPosition(CalculateClosestExit(npc));
        }
    }

    Vector3 CalculateClosestExit(GameObject npc)
    {
        Vector3 pos = npc.transform.position;
        Vector3 closest = Vector3.zero;

        foreach (Vector3 exit in exitPointPositions)
        {
            if (Vector3.Distance(exit, pos) < Vector3.Distance(closest, pos) || closest == Vector3.zero)
            {
                closest = exit;
            }
        }

        return closest;
    }
}
