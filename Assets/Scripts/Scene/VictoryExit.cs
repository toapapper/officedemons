using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VictoryExit : MonoBehaviour
{
    private bool alreadyWon;

    [SerializeField] private GameObject victoryUI;
    [SerializeField] private List<TextMeshProUGUI> playerColor;
    [SerializeField] private List<TextMeshProUGUI> playerNames;
    [SerializeField] private List<TextMeshProUGUI> playerKills;

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
            GameObject playerObject = victoryUI.transform.Find("P" + i).gameObject;

            playerObject.SetActive(true);

            playerColor[i].color = PlayerManager.players[i].GetComponent<Attributes>().PlayerColor;

            string name = PlayerManager.players[i].GetComponent<Attributes>().Name.ToString().Replace("_"," ");
            
            playerNames[i].text = name;

            playerKills[i].text = PlayerManager.players[i].GetComponent<Attributes>().KillCount.ToString();
        }
    }
}
