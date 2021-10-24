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
    [SerializeField] private float imageAlpha = 1f;
    [Tooltip("Colors that the circle will fade between. It starts out as color[0] and ends as the last one but fades smoothly between them all.")]
    [SerializeField] private Color[] colours = { Color.green, Color.yellow, Color.red };

    [SerializeField] private Image baseCircle;
    [SerializeField] private Image attackCircle;
    [SerializeField] private Image pickupCircle;

    [SerializeField] private Color baseColor;
    [SerializeField] private Color attackColor;
    [SerializeField] private Color pickupColor;

    [SerializeField] private GameObject staminaIndicator;
    [SerializeField] private GameObject attackIndicator;
    [SerializeField] private GameObject pickUpIndicator;

    [SerializeField] private float temporaryAttackCost = .5f;
    [SerializeField] private float temporaryPickupCost = .5f;

    [SerializeField] private Canvas canvas;

    /// <summary>
    /// Sets player
    /// </summary>
    /// <param name="player"></param>
    public void SetPlayer(GameObject player)
    {
        this.player = player;
        playerAttributes = player.GetComponent<Attributes>();

        baseCircle.color = baseColor;
        attackCircle.color = attackColor;
        pickupCircle.color = pickupColor;
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

                baseCircle.enabled = true;

                staminaIndicator.SetActive(true);


                float stamPercent = playerAttributes.Stamina/playerAttributes.StartStamina;
                baseCircle.fillAmount = stamPercent;

                float indicatorPercent = baseCircle.fillClockwise ? 1 - stamPercent : stamPercent;
                staminaIndicator.transform.localRotation = Quaternion.Euler(0, 0, 360 * indicatorPercent);

                float attackPercent = temporaryAttackCost / playerAttributes.StartStamina;
                
                if(attackPercent < stamPercent)
                {
                    attackCircle.enabled = true;
                    attackIndicator.SetActive(true);
                    attackCircle.fillAmount = attackPercent;
                    float attackIndicatorPercent = attackCircle.fillClockwise ? 1 - attackPercent : attackPercent;
                    attackIndicator.transform.localRotation = Quaternion.Euler(0, 0, 360 * attackIndicatorPercent);
                }
                else
                {
                    attackCircle.enabled = false;
                    attackIndicator.SetActive(false);
                }

                baseCircle.color = stamPercent < attackPercent ? attackColor : baseColor;

                //baseCircle.color = Utilities.MultiColorLerp(colours, 1 - stamPercent);
                //baseCircle.color = new Color(baseCircle.color.r, baseCircle.color.g, baseCircle.color.b, imageAlpha);
            }
            else
            {
                baseCircle.enabled = false;
                attackCircle.enabled = false;
                pickupCircle.enabled = false;

                attackIndicator.SetActive(false);
                staminaIndicator.SetActive(false);
                pickUpIndicator.SetActive(false);
            }
        }
    }
}
