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


    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text jobbText;

    [SerializeField] private Image weapon;
    [SerializeField] private List<Image> durability = new List<Image>();
    [SerializeField] private List<Image> chargeObjects = new List<Image>();
    [SerializeField] private List<Sprite> chargeSprites = new List<Sprite>();

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

        nameText.text = attributes.Name.ToString();
        jobbText.text = attributes.JobTitle;
        colorBar.color = attributes.PlayerColor;
        if (attributes.Name != Characters.Susan)
        {
            chargeObjects[3].enabled = false;
            chargeObjects[4].enabled = false;
            maxCharges = 3;
        }
    }
    

    void Update()
    {
        float ratio =  (float)player.GetComponent<Attributes>().Health / (float)player.GetComponent<Attributes>().StartHealth;
        colorBar.fillAmount = ratio;
    }

    /// <summary>
    /// I'm doing the if statments to save on performance. A check should be softer than an image change
    /// </summary>
    /// <param name="weaponSprite"></param>
    /// <param name="durabilitySprite1">this is the first number aka 10 20 30 40 50</param>
    /// <param name="durabilitySprite2">this is the second number aka 1 2 3 4 5</param>
    public void UpdateWeaponSprites(Sprite weaponSprite, Sprite durabilitySprite1, Sprite durabilitySprite2)
    {
        if (weapon.sprite != weaponSprite)
        {
            weapon.sprite = weaponSprite;
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
