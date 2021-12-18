using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    GameObject arrowsIcon, yButtonIcon, bButtonIcon, xButtonIcon, aButtonIcon, moveIcon, aimIcon; 

    public enum TutorialState { Move, PickUp, Attack, Special, Revive, Encounter};
    private TutorialState CurrentTutorialState;

    // Start is called before the first frame update
    void Start()
    {
        CurrentTutorialState = TutorialState.Move;
        DisableAllIcons();
        MoveTutorial();

        arrowsIcon = tutorialUI.transform.FindChild("ArrowsAllDirections").gameObject;
        yButtonIcon = tutorialUI.transform.FindChild("Ybutton").gameObject;
        bButtonIcon = tutorialUI.transform.FindChild("Bbutton").gameObject;
        xButtonIcon = tutorialUI.transform.FindChild("Xbutton").gameObject;
        aButtonIcon = tutorialUI.transform.FindChild("Abutton").gameObject;
        moveIcon = tutorialUI.transform.FindChild("Move").gameObject;
        aimIcon = tutorialUI.transform.FindChild("Aim").gameObject;
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
        arrowsIcon.SetActive(true);
        moveIcon.SetActive(true);

    }

    private void PickUpTutorial()
    {
        aButtonIcon.SetActive(true);
    }

    private void AttackTutorial()
    {
        aButtonIcon.SetActive(true);
        aimIcon.SetActive(true);
    }

    private void SpecialTutorial()
    {
        bButtonIcon.SetActive(true);
        aimIcon.SetActive(true);
    }

    private void ReviveTutorial()
    {
        yButtonIcon.SetActive(true);
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
        arrowsIcon.SetActive(false);
        yButtonIcon.SetActive(false);
        bButtonIcon.SetActive(false);
        xButtonIcon.SetActive(false);
        aButtonIcon.SetActive(false);
        moveIcon.SetActive(false);
        aimIcon.SetActive(false);
    }
}
