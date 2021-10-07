using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaCircle : MonoBehaviour
{
    public float yOffset = 10f;//pixlar typ antar jag..
    public float xOffset = 5f;

    public GameObject player;
    private Attributes playerAttributes;
    public Image image;

    public Canvas canvas;

    private void Start()
    {
        //image = GetComponent<Image>();
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
        playerAttributes = player.GetComponent<Attributes>();
        switch (player.name)
        {
            case "Devin 1(Clone)":
                image.color = UIManager.Instance.devinColor;
                break;
            case "TerribleTim(Clone)":
                image.color = UIManager.Instance.timColor;
                break;
            case "SusanTheDestroyer(Clone)":
                image.color = UIManager.Instance.susanColor;
                break;
            case "ViciousVicky(Clone)":
                image.color = UIManager.Instance.vickyColor;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //position ovanför spelaren, implementera senare
        //if (player != null && Camera.main != null)
        //{
        //    Vector2 position = Camera.main.WorldToViewportPoint(player.transform.position);
        //    RectTransform canv = canvas.transform as RectTransform;
        //    Vector2 canvasPos = new Vector2();

        //    RectTransformUtility.ScreenPointToLocalPointInRectangle(canv, position, null, out canvasPos);
        //    transform.localPosition = canvasPos;

        //    Debug.Log(position);
        //}

        if (player != null)
        {
            float stamPercent = playerAttributes.Stamina/playerAttributes.StartStamina;
            image.fillAmount = stamPercent;
        }
    }
}
