using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>
/// Handles player input in the player selection menu
/// </para> 
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15/11 -21
public class PlayerSetupMenuController : MonoBehaviour
{
    private int playerIndex;
    
    //[SerializeField]
    //private TextMeshProUGUI titleText;
    [SerializeField]
    private GameObject readyPanel;
    [SerializeField]
    private GameObject menuPanel;
    [SerializeField]
    private Button readyButton;

    private bool inputEnabled;

    public void SetPlayerIndex(int playerIndex)
	{
        this.playerIndex = playerIndex;
        //titleText.SetText("Player " + (playerIndex + 1).ToString());
	}

    void Update()
    {
        if(inputEnabled == false)
		{
            inputEnabled = true;
        }
    }

    public void SetCharacterChoice(int characterIndex)
	{
        if (!inputEnabled) { return; }

        PlayerConfigurationManager.Instance.SetPlayerCharacter(playerIndex, characterIndex);
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
