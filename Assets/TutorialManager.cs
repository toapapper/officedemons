using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    GameManager gm;

    [SerializeField]
    PlayerConfigurationManager pcm;

    [SerializeField]
    Collider trigger1, trigger2, trigger3, trigger4, encounterTrigger, endTrigger;

    [SerializeField]
    GameObject tutorialUI;

    [SerializeField]
    GameObject charges;

    [SerializeField]
    Encounter encounter;

    [SerializeField] GameObject move, attack, pickup, special, revive, hint;

    const float rate = 1;
    float counter = 0;
    int chargesCounter = 2;
    Color fullColor, colorStart, colorEnd;

    List<GameObject> weaponGlowEffects, chargesActiveIcons, chargesEmptyIcons, players;

    public enum TutorialState { Move, PickUp, Attack, Special, Revive, DirectionHint, Encounter , End};
    private TutorialState CurrentTutorialState;

    // Start is called before the first frame update
    void Start()
    {
        //CurrentTutorialState = TutorialState.DirectionHint;

        //MoveTutorial();

        fullColor = hint.GetComponentInChildren<Image>().color;
        colorEnd = hint.GetComponentInChildren<Image>().color;
        colorStart = colorEnd;
        colorEnd.a = 0;

        weaponGlowEffects = new List<GameObject>();
        foreach (Transform child in trigger1.transform)
        {
            weaponGlowEffects.Add(child.gameObject);
        }

        chargesActiveIcons = new List<GameObject>();
        chargesEmptyIcons = new List<GameObject>();
        foreach (Transform child in charges.transform)
        {
            if (child.gameObject.name == "charge")
            {
                chargesActiveIcons.Add(child.gameObject);
            }
            else
            {
                chargesEmptyIcons.Add(child.gameObject);
            }
        }

        DisableAllIcons();
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentTutorialState)
        {
            case TutorialState.Move:
                BlinkUpdate();
                break;

            case TutorialState.Special:
                SpecialUpdate();
                break;

            case TutorialState.DirectionHint:
                BlinkUpdate();
                break;

            case TutorialState.Encounter:
                EncounterUpdate();
                break;
        }
    }

    void BlinkUpdate()
    {
        hint.SetActive(true);
        counter += Time.deltaTime * rate;
        hint.GetComponentInChildren<Image>().color = Color.Lerp(colorStart, colorEnd, Mathf.PingPong(counter * 2, 1));

        if (counter >= 1)
        {
            counter = 0;
            colorEnd = fullColor;
            colorStart = colorEnd;
            colorEnd.a = 0;
        }
    }

    void SpecialUpdate()
    {
        counter += Time.deltaTime * rate;

        if (counter >= 1)
        {
            counter = 0;

            if (chargesCounter < 0)
            {
                //all full and restart countdown
                foreach (GameObject icon in chargesActiveIcons)
                {
                    icon.SetActive(true);
                }
                chargesCounter = 3;
            }
            else
            {
                // change one more to gray
                chargesActiveIcons[chargesCounter].SetActive(false);
            }
            chargesCounter--;
        }
    }

    void EncounterUpdate()
    {
        foreach (GameObject p in players)
        {
            if (p.GetComponent<Attributes>().Health <= 0)
            {
                revive.SetActive(true);
                break;
            }
            else
            {
                revive.SetActive(false);
            }
        }

        if (gm.CurrentCombatState == CombatState.enemy)
        {
            EnemyHealthDecrease();
        }
    }

    public void HandleTriggerEnter(GameObject trigger, Collider other)
    {
        if (other.tag == "Player")
        {
            DisableAllIcons();
            UpdateState(trigger);

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
                    if (trigger.name == "glow_effect")
                    {
                        trigger.SetActive(false);
                    }
                    AttackTutorial();
                    break;

                case TutorialState.Special:
                    chargesCounter = 2;
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

                case TutorialState.End:
                    EndTutorial();
                    break;
            }
            
        }
        else if (CurrentTutorialState == TutorialState.Attack && other.tag == "Projectile")
        {
            if (trigger.name == "glow_effect")
            {
                trigger.SetActive(false);
            }
        }
    }

    public void HandleTriggerExit(GameObject trigger, Collider other)
    {
        //if (other.tag == "Player")
        //{
        //    DisableAllIcons();
        //    trigger.SetActive(false);
        //}
    }

    private void MoveTutorial()
    {
        move.SetActive(true);
    }

    private void PickUpTutorial()
    {
        pickup.SetActive(true);
    }

    private void AttackTutorial()
    {
        attack.SetActive(true);
    }

    private void SpecialTutorial()
    {
        special.SetActive(true);

        foreach (GameObject go in chargesActiveIcons)
        {
            go.SetActive(true);
        }

        foreach (GameObject go in chargesEmptyIcons)
        {
            go.SetActive(true);
        }
    }

    private void ReviveTutorial()
    {
        revive.SetActive(true);
    }

    private void HintDirection()
    {
        hint.SetActive(true);
    }

    private void EncounterTutorial()
    {
        DisableAllIcons();
        //yButtonIcon.enabled = true;
        players = new List<GameObject>();
        foreach (GameObject player in PlayerManager.players)
        {
            players.Add(player);
        }
        
    }

    void EndTutorial()
    {
        Invoke("GoToMainMenu", 2f); 
    }

    void GoToMainMenu()
    {
        SceneManagment.Instance.GetMainMenu();
    }

    private void EnemyHealthDecrease()
    {
        List<GameObject> enemies = encounter.GetEnemylist();
        foreach (GameObject e in enemies)
        {
            e.GetComponent<Attributes>().Health = 1;
        }
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
                CurrentTutorialState = TutorialState.Special;
                break;

            case "trigger2":
                CurrentTutorialState = TutorialState.PickUp;
                break;

            case "trigger3":
                CurrentTutorialState = TutorialState.Attack;
                break;

            case "trigger4":
                CurrentTutorialState = TutorialState.DirectionHint;
                break;

            case "trigger5":
                CurrentTutorialState = TutorialState.Encounter;
                break;

            case "trigger6":
                CurrentTutorialState = TutorialState.End;
                break;
        }
    }

    private void DisableAllIcons()
    {
        move.SetActive(false);
        pickup.SetActive(false);
        attack.SetActive(false);
        special.SetActive(false);
        revive.SetActive(false);
        hint.SetActive(false);

        foreach (GameObject go in chargesActiveIcons)
        {
            go.SetActive(false);
        }

        foreach (GameObject go in chargesEmptyIcons)
        {
            go.SetActive(false);
        }
    }
}
