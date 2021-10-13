//Initially written by Ossian, feel free to edit if needed

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /// <summary>
    /// Innehåller alla element av uiet som GameManager behöver komma åt.
    /// Uppdaterar Saker i ui:et
    /// </summary>
    public static UIManager Instance;

    private Image timerBackgroundWithColorImage; //hemskt, jag vet..
    private bool playerCardsInitialized = false;
    
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


    // Start is called before the first frame update
    private void Awake()
    {
        Debug.LogWarning("HELLO UI MANAGER HERE!");
        Instance = this;

        timerBackgroundWithColorImage = timerBackgroundWithColor.GetComponent<Image>();
    }

    // Update is called once per frame
    private void Update()
    {
        #region fulInitialize här eftersom det typ alltid blir fel annars
        if (!playerCardsInitialized)
        {
            Debug.Log("Initialize player cards " + PlayerManager.instance.localPlayerList.Count);
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
        if (GameManager.Instance.combatState == CombatState.none)
        {
            timer.SetActive(false);
            enemys_turn_card.SetActive(false);
        }
        else if (GameManager.Instance.combatState == CombatState.player)
        {
            timer.SetActive(true);
            enemys_turn_card.SetActive(false);
            timerText.text = ("" + Mathf.Max(Mathf.Floor(GameManager.Instance.roundTimer),0));

            float percent = 1 - GameManager.Instance.roundTimer / GameManager.Instance.RoundTime;
            timerBackgroundWithColorImage.color = OssianUtils.MultiColorLerp(timerColors, percent);

            //float segmentedPercent = (Mathf.Floor(percent * 12) / 12) + 1;
            timerBackgroundWithColorImage.fillAmount = percent;
            timer_indicator.transform.rotation = Quaternion.Euler(0, 0, 360 * (1 - percent));
        }
        else if(GameManager.Instance.combatState == CombatState.enemy)
        {
            enemys_turn_card.SetActive(true);
            timer.SetActive(false);
        }
        #endregion
    }

    public void EnablePlayerUI(GameObject player)
    {
        int index = PlayerManager.players.IndexOf(player);
        EnablePlayerUI(index);
    }
    public void EnablePlayerUI(int i)
    {
        Debug.Log("UIcard init for player " + i);
        UIPlayerCard card = transform.Find("Canvas").transform.Find("playerCard" + i).GetComponent<UIPlayerCard>();
        if (card.gameObject.activeSelf)
        {
            Debug.LogWarning("Card already active");
            return;
        }

        card.gameObject.SetActive(true);
        card.Initialize(PlayerManager.players[i]);

        //StaminaCircle stamCirc = transform.Find("Canvas").transform.Find("StaminaCircle" + i).GetComponent<StaminaCircle>();
        StaminaCircle stamCirc = transform.Find("StamCircle" + i).GetChild(0).GetComponent<StaminaCircle>();
        stamCirc.gameObject.SetActive(true);
        Debug.LogWarning(PlayerManager.players[i]);
        stamCirc.SetPlayer(PlayerManager.players[i]);
    }

    public void OpenMenu()
    {
        pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstButtonSelectedOnPause);
    }

    public void CloseMenu()
    {
        pauseMenu.SetActive(false);
    }
}
