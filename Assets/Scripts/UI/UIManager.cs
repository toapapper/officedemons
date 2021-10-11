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

    [Header("Menus")]
    public GameObject pauseMenu;
    public GameObject firstButtonSelectedOnPause;

    [Header("Timer")]
    public GameObject timer;
    public GameObject timer_backGround_with_color;
    private Image timer_backGround_with_color_image; //hemskt, jag vet..
    public GameObject timer_indicator;

    //From 0 to 0.5 merge between color 0 and 1
    //from 0.5 to 0.75 between 1 and 2
    //and lastly from 0.75 to 1 between 2 and 3
    public Color timer_color0 = Color.white;
    public Color timer_color1 = Color.green;
    public Color timer_color2 = Color.yellow;
    public Color timer_color3 = Color.red;
    public Color[] timerColors = { Color.white, Color.yellow, new Color(1, 1, 0), Color.red };
    private TMP_Text timerText;

    [Header("Polis")]
    public GameObject enemys_turn_card;

    private bool playerCardsInitialized = false;

    // Start is called before the first frame update
    private void Start()
    {
        Debug.LogWarning("HELLO UI MANAGER HERE!");
        Instance = this;

        timerText = timer.GetComponentInChildren<TMP_Text>();
        timer_backGround_with_color_image = timer_backGround_with_color.GetComponent<Image>();

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
                EnablePlayerCard(i);
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
            timer_backGround_with_color_image.color = OssianUtils.MultiColorLerp(timerColors, percent);

            //float segmentedPercent = (Mathf.Floor(percent * 12) / 12) + 1;
            timer_backGround_with_color_image.fillAmount = percent;
            timer_indicator.transform.rotation = Quaternion.Euler(0, 0, 360 * (1 - percent));
        }
        else if(GameManager.Instance.combatState == CombatState.enemy)
        {
            enemys_turn_card.SetActive(true);
            timer.SetActive(false);
        }
        #endregion
    }

    public void EnablePlayerCard(GameObject player)
    {
        int index = PlayerManager.players.IndexOf(player);
        EnablePlayerCard(index);
    }
    public void EnablePlayerCard(int i)
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

        StaminaCircle stamCirc = transform.Find("Canvas").transform.Find("StaminaCircle" + i).GetComponent<StaminaCircle>();
        stamCirc.gameObject.SetActive(true);
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
