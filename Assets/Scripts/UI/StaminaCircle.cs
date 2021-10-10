using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaCircle : MonoBehaviour
{
    [Tooltip("offset in real space")]
    public Vector2 offset = new Vector2(0f, 1.5f);
    public float imageAlpha = .7f;

    public Color[] colours = { Color.green, Color.yellow, Color.red };

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
        //image.color = playerAttributes.PlayerColor;
        //image.color = new Color(image.color.r, image.color.g, image.color.b, imageAlpha);
    }

    // Update is called once per frame
    void Update()
    {
        //position ovanför spelaren, implementera senare
        if (player != null && Camera.main != null)
        {
            Vector2 position = Camera.main.WorldToViewportPoint(new Vector3(player.transform.position.x + offset.x, player.transform.position.y, player.transform.position.z + offset.y));

            transform.position = position * canvas.pixelRect.size;
        }

        if (player != null)
        {
            if(GameManager.Instance.combatState == CombatState.player)
            {
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
