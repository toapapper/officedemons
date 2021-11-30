using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VictoryExit : MonoBehaviour
{
    private bool alreadyWon;

    [SerializeField]
    private GameObject victoryUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!alreadyWon)
            {
                Debug.Log("attempting to win");
                Victory();
                alreadyWon = true;
            }
        }
    }

    private void Victory()
    {
        victoryUI.SetActive(true);
        Debug.Log(PlayerManager.players.Count);
        for (int i = 0; i < PlayerManager.players.Count; i++)
        {
            Debug.Log("HEJ0");

            GameObject playerObject = victoryUI.transform.Find("P" + i).gameObject;

            Debug.Log("HEJ1");

            playerObject.SetActive(true);
            Debug.Log("HEJ1.5");


            //Color playerColor = PlayerManager.players[i].GetComponent<Attributes>().PlayerColor;

            //playerObject.GetComponent<TextMeshPro>().color = new Color32(50, 50, 50, 255);
            Debug.Log("HEJ2");

            playerObject.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = PlayerManager.players[i].GetComponent<Attributes>().name;
            Debug.Log("HEJ3");

            //playerObject.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>().text = PlayerManager.players[i].GetComponent<Attributes>().KillCount.ToString();
            Debug.Log("HEJ4");

            Debug.Log("HEJ");
        }
    }

}
