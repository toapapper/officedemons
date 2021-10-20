using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>
/// Container and handler for the stamina-indicator shown below the player in combat.
/// </para>
///   
///  <para>
///  Author: Ossian
///  
/// </para>
///  
/// </summary>

// Last Edited: 20-10-21

public class StaminaCircle : MonoBehaviour
{

    private GameObject player;
    private Attributes playerAttributes;

    [SerializeField] private Vector3 offset = new Vector3(0f, -.5f, 0f);
    [Tooltip("How transparent the circle will be.")]
    [SerializeField] private float imageAlpha = .7f;
    [Tooltip("Colors that the circle will fade between. It starts out as color[0] and ends as the last one but fades smoothly between them all.")]
    [SerializeField] private Color[] colours = { Color.green, Color.yellow, Color.red };
    [SerializeField] private Image image;
    [SerializeField] private Canvas canvas;

    /// <summary>
    /// Sets player
    /// </summary>
    /// <param name="player"></param>
    public void SetPlayer(GameObject player)
    {
        this.player = player;
        playerAttributes = player.GetComponent<Attributes>();
    }

    /// <summary>
    /// Updates fillAmount to show amount of stamina left, and updates position so it's below the player at all times
    /// </summary>
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
