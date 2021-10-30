//Initially written by Ossian, feel free to edit if needed

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// <para>
/// Singleton!<br/>
/// Manages all of the in game user interface.
/// </para>
///   
///  <para>
///  Author: Ossian
///  
/// </para>
///  
/// </summary>

// Last Edited: 20-10-21

public class UIManager : MonoBehaviour
{
    /// <summary>
    /// Innehåller alla element av uiet som GameManager behöver komma åt.
    /// Uppdaterar Saker i ui:et
    /// </summary>
    public static UIManager Instance;

    private Image timerBackgroundWithColorImage; //hemskt namn, jag vet..
    private bool playerCardsInitialized = false;

    [SerializeField] private FloatingTextManager floatingTextManager;

    [Header("Menus")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject firstButtonSelectedOnPause;

    [Header("Timer")]
    [SerializeField] private GameObject timer;
    [SerializeField] private GameObject timerBackgroundWithColor;
    [SerializeField] private GameObject timer_indicator;

    [SerializeField] private Color[] timerColors = { Color.white, Color.yellow, new Color(1, 1, 0), Color.red };
    [SerializeField] private TMP_Text timerText;

    [Header("Polis")]
    [SerializeField] private GameObject enemys_turn_card;

    
    private void Awake()
    {
        Instance = this;
        timerBackgroundWithColorImage = timerBackgroundWithColor.GetComponent<Image>();
    }
    
    private void Update()
    {
        #region fulInitialize här eftersom det typ alltid blir fel annars
        if (!playerCardsInitialized)
        {
            Debug.Log("Initialize player cards " + PlayerManager.players.Count);
            //för varje spelare som finns, enablea ett playercard och mata in rätt spelare där.
            for (int i = 0; i < PlayerManager.players.Count; i++)
            {
                EnablePlayerUI(i);
            }
            playerCardsInitialized = true;
        }
        #endregion

        //Uppdatera liv på spelarna

        //includes the displaying of enemys_turn_card
        #region TimerUpdates
        if (GameManager.Instance.CurrentCombatState == CombatState.none)
        {
            timer.SetActive(false);
            enemys_turn_card.SetActive(false);
        }
        else if (GameManager.Instance.CurrentCombatState == CombatState.player)
        {
            timer.SetActive(true);
            enemys_turn_card.SetActive(false);
            timerText.text = ("" + Mathf.Max(Mathf.Floor(GameManager.Instance.RoundTimer),0));

            float percent = 1 - GameManager.Instance.RoundTimer / GameManager.Instance.RoundTime;
            timerBackgroundWithColorImage.color = Utilities.MultiColorLerp(timerColors, percent);

            //float segmentedPercent = (Mathf.Floor(percent * 12) / 12) + 1;
            timerBackgroundWithColorImage.fillAmount = percent;
            timer_indicator.transform.rotation = Quaternion.Euler(0, 0, 360 * (1 - percent));
        }
        else if(GameManager.Instance.CurrentCombatState == CombatState.enemy)
        {
            enemys_turn_card.SetActive(true);
            timer.SetActive(false);
        }
        #endregion
    }

    /// <summary>
    /// Initializes a playercard ui-element for the player
    /// </summary>
    /// <param name="player"></param>
    public void EnablePlayerUI(GameObject player)
    {
        int index = PlayerManager.players.IndexOf(player);
        EnablePlayerUI(index);
    }

    /// <summary>
    /// Initializes a playercard ui-element for the player
    /// </summary>
    /// <param name="i">the index att which the player exists in the PlayerManager.players-list</param>
    public void EnablePlayerUI(int i)
    {
        //Debug.Log("UIcard init for player " + i);
        UIPlayerCard card = transform.Find("Canvas").transform.Find("playerCard" + i).GetComponent<UIPlayerCard>();
        
        if (card.gameObject.activeSelf)
        {
            //Debug.LogWarning("Card already active");
            return;
        }

        card.gameObject.SetActive(true);
        card.Initialize(PlayerManager.players[i]);

        //StaminaCircle stamCirc = transform.Find("Canvas").transform.Find("StaminaCircle" + i).GetComponent<StaminaCircle>();
        StaminaCircle stamCirc = transform.Find("StamCircle" + i).GetComponent<StaminaCircle>();
        stamCirc.gameObject.SetActive(true);
        stamCirc.SetPlayer(PlayerManager.players[i]);
    }

    /// <summary>
    /// Opens the paus-menu
    /// </summary>
    public void OpenMenu()
    {
        pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstButtonSelectedOnPause);
    }

    /// <summary>
    /// Closes the paus-menu
    /// </summary>
    public void CloseMenu()
    {
        pauseMenu.SetActive(false);
    }

    /// <summary>
    /// Display a new floatingtext element at the gameObjects position
    /// </summary>
    /// <param name="at"> the gameObject at which it will be shown </param>
    /// <param name="text"> text </param>
    /// <param name="color"> color to show </param>
    public void NewFloatingText(GameObject at, string text, Color color)
    {
        floatingTextManager.newText(at, text, color);
    }
}
