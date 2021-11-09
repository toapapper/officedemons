using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    List<Transform> positions;
    // Start is called before the first frame update
    void Start()
    {
        //GameManager.Instance.CheckpointList.Add(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveCheckpoint()
    {
        GameManager.Instance.CurrentCheckpoint = this;
    }

    public void LoadCheckpoint()
    {
        int playerCounter = 0;
        foreach (GameObject player in PlayerManager.players)
        {
            Vector3 newPos = new Vector3(positions[playerCounter].position.x, player.transform.position.y, positions[playerCounter].position.z);
            player.transform.position = newPos;
            playerCounter++;
            //player.GetComponent<Attributes>().Health = player.GetComponent<Attributes>().StartHealth;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SaveCheckpoint();
        }
    }
}
