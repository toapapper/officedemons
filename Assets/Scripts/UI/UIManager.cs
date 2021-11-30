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
    /// Inneh�ller alla element av uiet som GameManager beh�ver komma �t.
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

    [Header("Weapon")]
    [SerializeField] private Sprite defaultWeapon;
    [SerializeField] private List<Sprite> numbers;

    private void Awake()
    {
        Instance = this;
        timerBackgroundWithColorImage = timerBackgroundWithColor.GetComponent<Image>();
    }
    
    private void Update()
    {
        #region fulInitialize h�r eftersom det typ alltid blir fel annars
        if (!playerCardsInitialized)
        {
            Debug.Log("Initialize player cards " + PlayerManager.players.Count);
            //f�r varje spelare som finns, enablea ett playercard och mata in r�tt spelare d�r.
            for (int i = 0; i < PlayerManager.players.Count; i++)
            {
                EnablePlayerUI(i);
            }
            playerCardsInitialized = true;
        }
        #endregion

        //Uppdatera liv p� spelarna

        UpdateWeaponUI();

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
        AkSoundEngine.StopAll();
        AkSoundEngine.PostEvent("Play_RadioMusic", gameObject);
        AkSoundEngine.PostEvent("Play_Ambience", gameObject);
        pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstButtonSelectedOnPause);
    }

    /// <summary>
    /// Closes the paus-menu
    /// </summary>
    public void CloseMenu()
    {
        AkSoundEngine.StopAll();
        AkSoundEngine.PostEvent("Play_Music", gameObject);
        AkSoundEngine.PostEvent("Play_AMBIENCE_Hall_Large__Entrance__Office_Building__Morning__Downtown_Chicago__USA__LOOP_LRLsRs", gameObject);
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


    /// <summary>
    /// Update the Images. Might do a check if there's any change before doing all this.
    /// Right now we do it every frame might wanna change it to only when we pickup/discard a weapon or shoot.
    /// </summary>
    private void UpdateWeaponUI()
    {
        for (int i = 0; i < PlayerManager.players.Count; i++)
        {
            UIPlayerCard card = transform.Find("Canvas").transform.Find("playerCard" + i).GetComponent<UIPlayerCard>();
            if (PlayerManager.players[i].GetComponent<WeaponHand>().objectInHand)
            {
                Sprite weaponSprite = PlayerManager.players[i].GetComponent<WeaponHand>().objectInHand.GetComponent<AbstractWeapon>().WeaponTexture;
                int durability = PlayerManager.players[i].GetComponent<WeaponHand>().objectInHand.GetComponent<AbstractWeapon>().Durability;
                if (durability > 9)
                {
                    float durabilityBig = durability / 10;
                    int durabilitySmall = durability - (int)durabilityBig * 10;
                    card.UpdateWeaponSprites(weaponSprite, GetDurabilitySprite((int)durabilityBig), GetDurabilitySprite(durabilitySmall));
                }
                else
                {
                    card.UpdateWeaponSprites(weaponSprite,numbers[10], GetDurabilitySprite(durability));
                }
            }
            else
            {
                card.UpdateWeaponSprites(defaultWeapon, numbers[10], numbers[10]);
            }
            card.UpdateChargeSprites(PlayerManager.players[i].GetComponentInChildren<AbstractSpecial>().Charges);
        }
    }


    /// <summary>
    /// numbers[10] == blank
    /// </summary>
    /// <param name="durability"></param>
    /// <returns></returns>
    private Sprite GetDurabilitySprite(int durability)
    {
        switch (durability)
        {
            case 0:
                return numbers[0];
            case 1:
                return numbers[1];
            case 2:
                return numbers[2];
            case 3:
                return numbers[3];
            case 4:
                return numbers[4];
            case 5:
                return numbers[5];
            case 6:
                return numbers[6];
            case 7:
                return numbers[7];
            case 8:
                return numbers[8];
            case 9:
                return numbers[9];
            default:
                return numbers[10];
        }
    }
}
