using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    GameManager gm;

    [SerializeField]
    PlayerManager pm;

    [SerializeField]
    Collider trigger1, trigger2, trigger3, trigger4, encounter;

    [SerializeField]
    GameObject tutorialUI;

    [SerializeField] Image allDirectionsArrowMiddle, allDirectionsArrowLeft, yButtonIcon, bButtonIcon, xButtonIcon, aButtonMiddleIcon, moveIcon, aimIcon, arrowDirectionIcon, aButtonRightIcon;

    const float rate = 1;
    float counter = 0;
    Color fullColor, colorStart, colorEnd;

    List<GameObject> weaponGlowEffects;

    public enum TutorialState { Move, PickUp, Attack, Special, Revive, DirectionHint, Encounter};
    private TutorialState CurrentTutorialState;

    // Start is called before the first frame update
    void Start()
    {
        CurrentTutorialState = TutorialState.DirectionHint;
        DisableAllIcons();
        //MoveTutorial();

        fullColor = arrowDirectionIcon.color;
        colorEnd = arrowDirectionIcon.color;
        colorStart = colorEnd;
        colorEnd.a = 0;

        weaponGlowEffects = new List<GameObject>();
        foreach (Transform child in trigger1.transform)
        {
            weaponGlowEffects.Add(child.gameObject);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentTutorialState)
        {
            case TutorialState.DirectionHint:
                BlinkUpdate();
                break;

            case TutorialState.Encounter:
                // Encounter update - see in GM if it's our turn
                // maybe let someone die? give one enemy strong wepaon with durability 1
                break;
        }
    }

    void BlinkUpdate()
    {
        counter += Time.deltaTime * rate;
        arrowDirectionIcon.color = Color.Lerp(colorStart, colorEnd, Mathf.PingPong(counter * 2, 1));

        if (counter >= 1)
        {
            counter = 0;
            colorEnd = fullColor;
            colorStart = colorEnd;
            colorEnd.a = 0;
        }
    }

    public void HandleTriggerEnter(GameObject trigger, Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log(other.gameObject.name + " entered " + trigger.name);

            DisableAllIcons();
            UpdateState(trigger);
            Debug.Log("CurrentTutorialState: " + CurrentTutorialState.ToString());

            //Move, PickUp, Attack, Special, Revive, DirectionHint, Encounter
            switch (CurrentTutorialState)
            {
                case TutorialState.Move:
                    MoveTutorial();
                    break;

                case TutorialState.PickUp:
                    if (trigger.name == "glow_effect")
                    {
                        trigger.SetActive(false);
                    }
                    PickUpTutorial();
                    break;

                case TutorialState.Attack:
                    AttackTutorial();
                    break;

                case TutorialState.Special:
                    SpecialTutorial();
                    break;

                case TutorialState.Revive:
                    ReviveTutorial();
                    break;

                case TutorialState.DirectionHint:
                    HintDirection();
                    break;

                case TutorialState.Encounter:
                    EncounterTutorial();
                    break;
            }
        }
    }

    public void HandleTriggerExit(GameObject trigger, Collider other)
    {
        //DisableAllIcons();
    }

    private void MoveTutorial()
    {
        allDirectionsArrowMiddle.enabled = true;
        moveIcon.enabled = true;
    }

    private void PickUpTutorial()
    {
        xButtonIcon.enabled = true;
    }

    private void AttackTutorial()
    {
        aButtonMiddleIcon.enabled = true;
    }

    private void SpecialTutorial()
    {
        bButtonIcon.enabled = true;
        aimIcon.enabled = true;
        allDirectionsArrowLeft.enabled = true;
    }

    private void ReviveTutorial()
    {
        yButtonIcon.enabled = true;
    }

    private void HintDirection()
    {
        arrowDirectionIcon.enabled = true;
    }

    private void EncounterTutorial()
    {
        aButtonRightIcon.enabled = true;
        aimIcon.enabled = true;
    }

    private void UpdateState(GameObject trigger)
    {
        //Move (default), PickUp, Attack, Special, Revive, Encounter
        switch (trigger.gameObject.name)
        {
            case "trigger0":
                CurrentTutorialState = TutorialState.Move;
                break;

            case "trigger1":
                CurrentTutorialState = TutorialState.PickUp;
                break;

            case "trigger2":
                CurrentTutorialState = TutorialState.Special;
                break;

            case "trigger3":
                CurrentTutorialState = TutorialState.Attack;
                break;

            case "trigger4":
                CurrentTutorialState = TutorialState.DirectionHint;
                break;

            case "Encounter":
                CurrentTutorialState = TutorialState.Encounter;
                break;
        }
    }

    private void DisableAllIcons()
    {
        yButtonIcon.enabled = false;
        bButtonIcon.enabled = false;
        xButtonIcon.enabled = false;
        aButtonMiddleIcon.enabled = false;
        aButtonRightIcon.enabled = false;
        moveIcon.enabled = false;
        aimIcon.enabled = false;
        arrowDirectionIcon.enabled = false;
        allDirectionsArrowLeft.enabled = false;
        allDirectionsArrowMiddle.enabled = false;
    }
}
