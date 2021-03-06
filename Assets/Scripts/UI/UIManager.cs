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
///  Original Author: Ossian
///  A lot of work here has been done by other developers
///  
/// </para>
///  
/// </summary>

// Last Edited: 20-10-21

public class UIManager : MonoBehaviour
{
    /// <summary>
    /// Inneh?ller alla element av uiet som GameManager beh?ver komma ?t.
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

    [Header("evilCount")]
    [SerializeField] private TMP_Text[] evilCounts;
    private bool[] activeEvilCounts = new bool[4];

    private void Awake()
    {
        Instance = this;
        timerBackgroundWithColorImage = timerBackgroundWithColor.GetComponent<Image>();

        
    }
    
    private void Update()
    {
        #region fulInitialize h?r eftersom det typ alltid blir fel annars
        if (!playerCardsInitialized)
        {
            Debug.Log("Initialize player cards " + PlayerManager.players.Count);
            //f?r varje spelare som finns, enablea ett playercard och mata in r?tt spelare d?r.
            for (int i = 0; i < PlayerManager.players.Count; i++)
            {
                EnablePlayerUI(i);
            }
            playerCardsInitialized = true;
        }
        #endregion

        //Uppdatera liv p? spelarna

        UpdateWeaponUI();

        //Set color.alpha to zero on evilCounts if in combat. Else set to one
        //Terribly optimized
        for(int i = 0; i < 4; i++)
        {
            if (activeEvilCounts[i])
            {
                Color color = evilCounts[i].color;
                if(GameManager.Instance.CurrentCombatState == CombatState.none)
                {
                    color.a = 1;
                    evilCounts[i].text = $"P{i+1} Evil:{ PlayerManager.players[i].GetComponent<Attributes>().EvilPoints}";
                }
                else
                {
                    color.a = 0;
                }
                evilCounts[i].color = color;
            }
        }


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
        //StaminaCircle stamCirc = transform.Find("StamCircle" + i).GetComponent<StaminaCircle>();
        //stamCirc.gameObject.SetActive(true);
        //stamCirc.SetPlayer(PlayerManager.players[i]);
        
        PlayerManager.players[i].GetComponentInChildren<PlayerUIExtras>().GetPlayerSymbol(i, PlayerManager.players[i].GetComponent<Attributes>().PlayerColor);

        //Initialization of the evil counter text elements.
        evilCounts[i].gameObject.SetActive(true);
        activeEvilCounts[i] = true;
        Color color = PlayerManager.players[i].GetComponent<Attributes>().PlayerColor;
        float H, S, V;
        Color.RGBToHSV(color, out H, out S, out V);
        color = Color.HSVToRGB(H, 1, V);//Makes a somewhat nicer color in the same hue and value as the players color
        evilCounts[i].color = color;
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
    public void NewFloatingText(GameObject at, string text, Color color, float sizeMultiPlier = 1f)
    {
        floatingTextManager.newText(at, text, color, sizeMultiPlier);
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
                    card.UpdateWeaponSprites(weaponSprite, PlayerManager.players[i].GetComponent<WeaponHand>().objectInHand.name, GetDurabilitySprite((int)durabilityBig), GetDurabilitySprite(durabilitySmall));
                }
                else
                {
                    card.UpdateWeaponSprites(weaponSprite, PlayerManager.players[i].GetComponent<WeaponHand>().objectInHand.name, numbers[10], GetDurabilitySprite(durability));
                }
            }
            else
            {
                card.UpdateWeaponSprites(defaultWeapon,"", numbers[10], numbers[10]);
            }
            card.UpdateChargeSprites(PlayerManager.players[i].GetComponentInChildren<AbstractSpecial>().Charges);

            #region super
            //Super charges icons. Massive if statment because I did not want to move supercharged and exploding to abstract special
            //so have to make a check for both versions
            if (PlayerManager.players[i].GetComponent<Attributes>().VariantName == Variant.Stapler)
            {
                if (PlayerManager.players[i].GetComponentInChildren<Stapler>().SuperCharged == true)
                {
                    PlayerManager.players[i].GetComponentInChildren<PlayerUIExtras>().SuperCharged(false);
                }
                else
                {
                    PlayerManager.players[i].GetComponentInChildren<PlayerUIExtras>().ChargeUpdate(card.ChargeObjects);
                }
            }
            else if (PlayerManager.players[i].GetComponent<Attributes>().VariantName == Variant.TerribleBreath)
            {
                if (PlayerManager.players[i].GetComponentInChildren<TerribleBreath>().SuperCharged == true)
                {
                    PlayerManager.players[i].GetComponentInChildren<PlayerUIExtras>().SuperCharged(false);
                }
                else
                {
                    PlayerManager.players[i].GetComponentInChildren<PlayerUIExtras>().ChargeUpdate(card.ChargeObjects);
                }
            }
            else if (PlayerManager.players[i].GetComponent<Attributes>().VariantName == Variant.PaperShredder)
            {
                if (PlayerManager.players[i].GetComponentInChildren<PaperShredder>().ReadyToExplode)
                {
                    PlayerManager.players[i].GetComponentInChildren<PlayerUIExtras>().SuperCharged(true);
                }
                else
                {
                    PlayerManager.players[i].GetComponentInChildren<PlayerUIExtras>().ChargeUpdate(card.ChargeObjects);
                }
            }
            else if (PlayerManager.players[i].GetComponent<Attributes>().VariantName == Variant.StarThrower)
            {
                if (PlayerManager.players[i].GetComponentInChildren<StarThrower>().ReadyToExplode)
                {
                    PlayerManager.players[i].GetComponentInChildren<PlayerUIExtras>().SuperCharged(true);
                }
                else
                {
                    PlayerManager.players[i].GetComponentInChildren<PlayerUIExtras>().ChargeUpdate(card.ChargeObjects);
                }
            }
            else
            {
                PlayerManager.players[i].GetComponentInChildren<PlayerUIExtras>().ChargeUpdate(card.ChargeObjects);
            }
            #endregion


            //Loop through all statuseffecttypes to see if activeEffects contains them
            Dictionary<StatusEffectType, StatusEffect> activeEffects = PlayerManager.players[i].GetComponent<Attributes>().statusEffectHandler.ActiveEffects;
            List<StatusEffectType> effectList = new List<StatusEffectType>();
            for (int j = 0; j < (int)StatusEffectType.vulnerable; j++)
            {
                StatusEffectType si = (StatusEffectType)j;

                if (activeEffects.ContainsKey(si))
                {
                    effectList.Add(si);
                }
            }
            card.UpdateEffectSprites(ConvertEffectsIntoSprites(effectList));
        }
    }



    private int[] ConvertEffectsIntoSprites(List<StatusEffectType> list)
    {
        int[] sprites = new int[4];

        for (int i = 0; i < 4; i++)
        {
            int index = 0;
            sprites[i] = index;
            if (i < list.Count)
            {
                index = (int)list[i];
                sprites[i] = index + 1;
            }
        }
        return sprites;
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
