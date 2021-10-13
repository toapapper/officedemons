using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaCircle : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0f, -.5f, 0f);

    private GameObject player;
    private Attributes playerAttributes;

    [SerializeField] private float imageAlpha = .7f;
    [SerializeField] private Color[] colours = { Color.green, Color.yellow, Color.red };
    [SerializeField] private Image image;
    [SerializeField] private Canvas canvas;
    //[SerializeField] private GameObject canvasObject;

    private void Start()
    {
        //image = GetComponent<Image>();
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
        playerAttributes = player.GetComponent<Attributes>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (player != null && Camera.main != null)
        //{
        //    Vector2 position = Camera.main.WorldToViewportPoint(new Vector3(player.transform.position.x + offset.x, player.transform.position.y, player.transform.position.z + offset.y));

        //    transform.position = position * canvas.pixelRect.size;
        //}

        if (player != null)
        {
            if(GameManager.Instance.combatState == CombatState.player)
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
