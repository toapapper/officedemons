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

// Last Edited: 20-10-21


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
    [SerializeField] private Image durability;


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
    }
    

    void Update()
    {
        float ratio =  (float)player.GetComponent<Attributes>().Health / (float)player.GetComponent<Attributes>().StartHealth;
        colorBar.fillAmount = ratio;
    }


    public void UpdateWeaponSprites(Sprite weaponSprite, Sprite durabilitySprite)
    {
        weapon.sprite = weaponSprite;
        durability.sprite = durabilitySprite;
    }
}
