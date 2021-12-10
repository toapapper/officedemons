using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// <para>
/// Container and handler for all the components of the player card ui element.
/// </para>
///   
///  <para>
///  Author: Ossian
///  
/// </para>
///  
/// </summary>

// Last Edited: 23-11-21


public class UIPlayerCard : MonoBehaviour
{
    /// <summary>
    /// spelare tillhörande kortet
    /// </summary>
    [SerializeField] private GameObject player;
    [SerializeField] private Image colorBar;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI jobbText;
    [SerializeField] private TextMeshProUGUI weaponText;

    [SerializeField] private Image weapon;
    [SerializeField] private Image portrait;
    [SerializeField] private List<Image> durability = new List<Image>();
    [SerializeField] private List<Image> chargeObjects = new List<Image>();

    public List<Image> ChargeObjects
    {
       get{ return chargeObjects; }
    }

    [SerializeField] private List<Sprite> chargeSprites = new List<Sprite>();

    [SerializeField] private List<Image> effects = new List<Image>();
    [SerializeField] private List<Sprite> effectsSprite = new List<Sprite>();


    private int maxCharges = 5;
    private int oldCharges;

    /// <summary>
    /// Initialize the playercard. Reading the relevant attributes of its asigned player and filling them in.
    /// </summary>
    /// <param name="player"> The player - gameobject this card belongs to</param>
    public void Initialize(GameObject player)
    {
        this.player = player;
        Attributes attributes = player.GetComponent<Attributes>();

        string characterName = attributes.Name.ToString().Replace("_", " ");
        nameText.text = characterName;
        jobbText.text = attributes.JobTitle;
        colorBar.color = attributes.PlayerColor;
        portrait.sprite = attributes.portrait;
        if (attributes.Name != Characters.Susan_The_Destroyer)
        {
            chargeObjects[3].enabled = false;
            chargeObjects[4].enabled = false;
            maxCharges = 3;
        }
        string name = gameObject.name.Substring(gameObject.name.Length - 1);
        player.GetComponentInChildren<PlayerUIExtras>().GetPlayerSymbol(Int32.Parse(name),player.GetComponent<Attributes>().PlayerColor);
    }


    void Update()
    {
        float ratio = (float)player.GetComponent<Attributes>().Health / (float)player.GetComponent<Attributes>().StartHealth;
        colorBar.fillAmount = ratio;
    }

    /// <summary>
    /// I'm doing the if statments to save on performance. A check should be softer than an image change
    /// </summary>
    /// <param name="weaponSprite"></param>
    /// <param name="durabilitySprite1">this is the first number aka 10 20 30 40 50</param>
    /// <param name="durabilitySprite2">this is the second number aka 1 2 3 4 5</param>
    public void UpdateWeaponSprites(Sprite weaponSprite,string weaponName, Sprite durabilitySprite1, Sprite durabilitySprite2)
    {
        if (weapon.sprite != weaponSprite)
        {
            weapon.sprite = weaponSprite;
            weaponText.text = weaponName;
        }
        if (durability[0].sprite != durabilitySprite1)
        {
            durability[0].sprite = durabilitySprite1;
        }
        if (durability[1].sprite != durabilitySprite2)
        {
            durability[1].sprite = durabilitySprite2;
        }
    }

    public void UpdateEffectSprites(int[] spriteValue)
    {
        for (int i = 0; i < 4; i++)
        {
            if (spriteValue[i] == 0)
            {
                effects[i].sprite = effectsSprite[0];
            }
            else
            {
                Debug.Log("changing sprite of i:" + i);
                effects[i].sprite = effectsSprite[spriteValue[i]];

            }
        }
    }


    /// <summary>
    /// To update the current charges a player has
    /// </summary>
    /// <param name="activeCharges"></param>
    public void UpdateChargeSprites(int activeCharges)
    {
        if (activeCharges != oldCharges)
        {
            oldCharges = activeCharges;
            for (int i = 0; i < maxCharges; i++)
            {
                chargeObjects[i].sprite = chargeSprites[1];
            }
            switch (activeCharges)
            {
                case 0:
                    for (int i = 0; i < maxCharges; i++)
                    {
                        chargeObjects[i].sprite = chargeSprites[0];
                    }
                    break;
                case 1:
                    for (int i = 1; i < maxCharges; i++)
                    {
                        chargeObjects[i].sprite = chargeSprites[0];
                    }
                    break;
                case 2:
                    for (int i = 2; i < maxCharges; i++)
                    {
                        chargeObjects[i].sprite = chargeSprites[0];
                    }
                    break;
                case 3:
                    for (int i = 3; i < maxCharges; i++)
                    {
                        chargeObjects[i].sprite = chargeSprites[0];
                    }
                    break;
                case 4:
                    for (int i = 4; i < maxCharges; i++)
                    {
                        chargeObjects[i].sprite = chargeSprites[0];
                    }
                    break;
                default:
                    break;
            }
        }
    }



}
