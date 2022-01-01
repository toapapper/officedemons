using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCManager : MonoBehaviour
{
    [SerializeField] GameObject NPCs;

    [SerializeField] List<GameObject> spawnPoints;
                         
    [SerializeField] List<GameObject> exitPoints;

    List<GameObject> npcList;
    List<Vector3> spawnPointPositions;
    List<Vector3> exitPointPositions;

    [SerializeField]
    float timeBetweenSpawns = 3f;

    float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        npcList = new List<GameObject>();
        spawnPointPositions = new List<Vector3>();
        exitPointPositions = new List<Vector3>();

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
        timer += Time.deltaTime;
        float seconds = timer % 60;

        if (seconds >= timeBetweenSpawns) // update list
        {
            AddNPC();
            timer = 0;
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
                Vector3 spawnPoint = spawnPointPositions[Random.Range(0, spawnPoints.Count - 1)];
                Vector3 exitPoint = exitPointPositions[Random.Range(0, exitPoints.Count - 1)];

                npc.GetComponent<NPCScript>().InstantiateValues(spawnPoint, exitPoint);
                npc.SetActive(true);
                break;
            }
        }
    }
}
