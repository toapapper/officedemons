using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RevivePrompt : MonoBehaviour
{
    [SerializeField] private Image revivePrompt;
    [SerializeField] private TextMeshProUGUI pPlayer;
    private Vector2 startValue, currentValue;
    private float min, max;
    private bool expanding = true;



    private void Start()
    {
        startValue = revivePrompt.transform.localScale;
        max = startValue.x * 1.1f;
        min = startValue.x * 0.9f;
    }

    // Update is called once per frame
    void Update()
    {
        bool showPrompt = false;

        for (int i = 0; i < PlayerManager.players.Count; i++)
        {
            if (GameManager.Instance.CurrentCombatState == CombatState.player &&
                PlayerManager.players[i] != gameObject.transform.parent.gameObject &&
                gameObject.transform.parent.GetComponent<Attributes>().Health <= 0 &&
                PlayerManager.players[i].GetComponent<Attributes>().Health >= 0 &&
                Vector3.Distance(PlayerManager.players[i].transform.position, gameObject.transform.position) < 5)
            {

                revivePrompt.gameObject.SetActive(true);
                showPrompt = true;
                currentValue = revivePrompt.transform.localScale;
                Pulsate();
            }
        }
        if (!showPrompt)
        {
            revivePrompt.gameObject.SetActive(false);
        }
    }


    private void Pulsate()
    {
        if (expanding)
        {
            if (currentValue.x >= max)
            {
                expanding = false;
                return;
            }
            revivePrompt.transform.localScale = new Vector3(revivePrompt.transform.localScale.x + 0.005f * Time.deltaTime, revivePrompt.transform.localScale.y + 0.005f * Time.deltaTime, revivePrompt.transform.localScale.z);
        }
        else
        {
            if (currentValue.x <= min)
            {
                expanding = true;
                return;
            }
            revivePrompt.transform.localScale = new Vector3(revivePrompt.transform.localScale.x - 0.005f * Time.deltaTime, revivePrompt.transform.localScale.y - 0.005f * Time.deltaTime, revivePrompt.transform.localScale.z);
        }
    }


    public void OneTimeUse(int playerNumber, Color playerColor)
    {
        pPlayer.text = "P" + (playerNumber + 1);
        pPlayer.color = playerColor;
    }

}
