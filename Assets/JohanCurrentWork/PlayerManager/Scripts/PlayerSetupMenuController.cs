using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetupMenuController : MonoBehaviour
{
    private int playerIndex;

    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private GameObject readyPanel;
    [SerializeField]
    private GameObject menuPanel;
    [SerializeField]
    private Button readyButton;

    private float ignoreInutTime = 1.5f;
    private bool inputEnabled;

    public void SetPlayerIndex(int playerIndex)
	{
        this.playerIndex = playerIndex;
        titleText.SetText("Player " + (playerIndex + 1).ToString());
        ignoreInutTime += Time.time;
	}

    // Update is called once per frame
    void Update()
    {
        if(inputEnabled == false && Time.time > ignoreInutTime)
		{
            inputEnabled = true;
        }
    }

    public void SetColor(Material color)
	{
		if (!inputEnabled) { return; }

        PlayerConfigurationManager.Instance.SetPlayerColor(playerIndex, color);
        menuPanel.SetActive(false);
        readyPanel.SetActive(true);
        readyButton.Select();
	}

    public void ReadyPlayer()
	{
        if (!inputEnabled) { return; }

        PlayerConfigurationManager.Instance.ReadyPlayer(playerIndex);
        readyButton.gameObject.SetActive(false);
    }
}
