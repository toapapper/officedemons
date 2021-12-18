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
    Collider trigger1;

    [SerializeField]
    Collider trigger2;

    [SerializeField]
    GameObject tutorialUI;

    [SerializeField] Image arrowsIcon, yButtonIcon, bButtonIcon, xButtonIcon, aButtonIcon, moveIcon, aimIcon; 

    public enum TutorialState { Move, PickUp, Attack, Special, Revive, Encounter};
    private TutorialState CurrentTutorialState;

    // Start is called before the first frame update
    void Start()
    {
        CurrentTutorialState = TutorialState.Move;
        DisableAllIcons();
        MoveTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        

        
    }

    public void HandleTrigger(GameObject trigger, Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log(other.gameObject.name + " entered " + trigger.name);

            DisableAllIcons();
            UpdateState(trigger);

            //Move, PickUp, Attack, Special, Revive, Encounter
            switch (CurrentTutorialState)
            {
                case TutorialState.Move:
                    MoveTutorial();
                    break;

                case TutorialState.PickUp:
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

                case TutorialState.Encounter:
                    break;
            }
        }
    }

    private void MoveTutorial()
    {
        arrowsIcon.enabled = true;
        moveIcon.enabled = true;

    }

    private void PickUpTutorial()
    {
        aButtonIcon.enabled = true;
    }

    private void AttackTutorial()
    {
        aButtonIcon.enabled = true;
        aimIcon.enabled = true;
    }

    private void SpecialTutorial()
    {
        bButtonIcon.enabled = true;
        aimIcon.enabled = true;
    }

    private void ReviveTutorial()
    {
        yButtonIcon.enabled = true;
    }

    private void EncounterTutorial()
    {

    }

    private void UpdateState(GameObject trigger)
    {
        //Move (default), PickUp, Attack, Special, Revive, Encounter
        switch (trigger.gameObject.name)
        {
            case "trigger1":
                CurrentTutorialState = TutorialState.PickUp;
                break;

            case "trigger2":
                CurrentTutorialState = TutorialState.Attack;
                break;
        }
    }

    private void DisableAllIcons()
    {
        arrowsIcon.enabled = false;
        yButtonIcon.enabled = false;
        bButtonIcon.enabled = false;
        xButtonIcon.enabled = false;
        aButtonIcon.enabled = false;
        moveIcon.enabled = false;
        aimIcon.enabled = false;
    }
}
