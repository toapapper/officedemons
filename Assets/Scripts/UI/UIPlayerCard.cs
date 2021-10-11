using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerCard : MonoBehaviour
{
    /// <summary>
    /// spelare tillhörande kortet
    /// </summary>
    public GameObject player;
    public Image colorBar;
    public UIHeartContainer heartContainer;

    //Use later, ge olika beroende på vilken char, ang vapen, ge när vapen plockas upp
    //public GameObject fotoPicture;
    //public GameObject weapon

    public TMP_Text nameText;
    public TMP_Text jobbText;

    public void Initialize(GameObject player)
    {
        heartContainer.gameObject.SetActive(true);
        heartContainer.SetPlayer(player);

        Attributes attributes = player.GetComponent<Attributes>();

        nameText.text = attributes.Name.ToString();
        jobbText.text = attributes.JobTitle;
        colorBar.color = attributes.PlayerColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
