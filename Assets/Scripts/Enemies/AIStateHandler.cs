using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Handles states for AI agent
// State is then used to call action from Action in Encounter

// States: FindCover, CallForHealing, Attack, Move, Wait 

public class AIStateHandler : MonoBehaviour
{
    Encounter encounter;
    Actions actions;
    Attributes attributes;
    GameObject rightHand, leftHand;
    FieldOfView fov; 
    //AIfov;//Agentens FOV 360 grader obstructed av obstacles /270 grader????


    // Start is called before the first frame update
    void Start()
    {
        encounter = transform.parent.gameObject.GetComponent<Encounter>();
        actions = GetComponent<Actions>();
        attributes = GetComponent<Attributes>();
        rightHand = this.gameObject.transform.GetChild(1).gameObject;
        leftHand = this.gameObject.transform.GetChild(2).gameObject;
        fov = GetComponent<FieldOfView>(); //weapon's fov
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeState() // Kallas på om turn OCH om State == Move
    {
        //GameObject weapon = rightHand.transform.GetChild(0).gameObject;

        // if state == TakeCover || Move

        if (HealthLow())
        {
            LowHealthBehaviour();
        }
        else
        {
            // Turn to target

            //if (fov.visibletargets.Count <= 0) // <- om target finns i line of sight för vapnet
            //{
            //      State = Attack
            //}
            //else 
            //{
            //if (Stamina > 0)
            //{
            //  State = Move // rör sig mot target tills man target finns i line of sight    // kanske ta hänsyn till sin stamina och då ta  ett annat beslut?
            //}
            //else
            //{
            //  State = Wait
            //    
            //}
        }
    }

    bool HoldingRangedWeapon()
    {
        if (rightHand.transform.GetChild(0).gameObject.GetType() == typeof(RangedWeapon))
        {
            return true;
        }
        return false;
    }

    bool HoldingMeleeWeapon()
    {
        if (rightHand.transform.GetChild(0).gameObject.GetType() == typeof(MeleeWeapon))
        {
            return true;
        }
        return false;
    }

    void LowHealthBehaviour()
    {
        //if(HealerIsClose())
        //{
        //    State = CallforHealing
        //}
        //else if(CoverNear)
        //{ 
        //    State = TakeCover 
        //    
        //}
        // else
        //{
        //  State = Attack
        //}
    }

    bool HealthLow()
    {
        return attributes.Health <= attributes.StartHealth / 3; // A THIRD OF MAX HEALTH - CHANGE ?
    }
}
