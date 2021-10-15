using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaCircle : MonoBehaviour
{

    private GameObject player;
    private Attributes playerAttributes;

    [SerializeField] private Vector3 offset = new Vector3(0f, -.5f, 0f);
    [SerializeField] private float imageAlpha = .7f;
    [SerializeField] private Color[] colours = { Color.green, Color.yellow, Color.red };
    [SerializeField] private Image image;
    [SerializeField] private Canvas canvas;


    public void SetPlayer(GameObject player)
    {
        this.player = player;
        playerAttributes = player.GetComponent<Attributes>();
    }

    private void Update()
    {
        if (player != null)
        {
            if(GameManager.Instance.CurrentCombatState == CombatState.player)
            {
                canvas.transform.position = player.transform.position + offset;

                image.enabled = true;
                float stamPercent = playerAttributes.Stamina/playerAttributes.StartStamina;
                image.fillAmount = stamPercent;

                image.color = OssianUtils.MultiColorLerp(colours, 1 - stamPercent);
            }
            else
            {
                image.enabled = false;
            }
        }
    }
}
