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

        #region Character specifics
        if(nameText != null && jobbText != null)
        {
            if (player.name == "Devin 1(Clone)")
            {
                colorBar.color = UIManager.Instance.devinColor;
                nameText.text = "Devin";
                jobbText.text = "Programmer";
            }
            else if (player.name == "SusanTheDestroyer(Clone)")
            {
                colorBar.color = UIManager.Instance.susanColor;
                nameText.text = "Susan";
                jobbText.text = "Paper pusher";
            }
            else if (player.name == "TerribleTim(Clone)")
            {
                colorBar.color = UIManager.Instance.timColor;
                nameText.text = "Tim the terrible";
                jobbText.text = "programmer, terrible";
            }
            else if (player.name == "ViciousVicky(Clone)")
            {
                colorBar.color = UIManager.Instance.vickyColor;
                nameText.text = "Vicky Vicous";
                jobbText.text = "we don't really know";
            }
        }
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
