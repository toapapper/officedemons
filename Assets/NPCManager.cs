using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCManager : MonoBehaviour
{
    [SerializeField] GameObject npcTemplate;

    [SerializeField] List<Vector3> spawnPoints;
                         
    [SerializeField] List<Vector3> exitPoints;

    List<GameObject> npcList;

    int random;

    // Start is called before the first frame update
    void Start()
    {
        npcList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // update list

        foreach (GameObject npc in npcList)
        {
            npc.GetComponent<NPCScript>().Update();
        }
    }

    public void RemoveNPC(GameObject npc)
    {

    }

    public void AddNPC()
    {
        Vector3 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count -1)];
        Vector3 exitPoint = exitPoints[Random.Range(0, exitPoints.Count - 1)];

        GameObject newNPC = Instantiate(npcTemplate);
        newNPC.GetComponent<NPCScript>().InstantiateValues(spawnPoint, exitPoint);
    }
}
