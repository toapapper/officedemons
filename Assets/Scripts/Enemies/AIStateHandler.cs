using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Handles states for AI agent
// Written by: Tinea & Tim

public class AIStateHandler : MonoBehaviour
{
    Encounter encounter;
    Actions actions;
    Attributes attributes;
    GameObject rightHand, leftHand;
    FieldOfView fov;
    AIController aiController;

    // Start is called before the first frame update
    void Start()
    {
        encounter = transform.parent.gameObject.GetComponent<Encounter>();
        actions = GetComponent<Actions>();
        attributes = GetComponent<Attributes>();
        rightHand = this.gameObject.transform.GetChild(1).gameObject;
        leftHand = this.gameObject.transform.GetChild(2).gameObject;
        fov = GetComponent<FieldOfView>(); //weapon's fov
        aiController = GetComponent<AIController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateState() // Kallas på om turn OCH om State == Move
    {
        //GameObject weapon = rightHand.transform.GetChild(0).gameObject;
        if (aiController.CurrentState == AIStates.States.Unassigned)
        {
            //rotera mot närmsta spelaren på NavMesh
            NavMeshAgent navmesh = GetComponent<NavMeshAgent>();

            // Closest TArget = Encounter.navMeshAGents.Closest() //                                                  <------------------
            //transform.LookAt(ClosestTarget)
        }

        if (HealthLow())
        {
            LowHealthBehaviour();
        }
        else
        {
            Debug.Log("HALLÅÅÅÅÅÅÅÅÅÅÅÅÅÅÅ");
            // turn to player


            if (fov.visibleTargets.Count > 0) // <- om target finns i line of sight för vapnet
            {
                aiController.CurrentState = AIStates.States.Attack;
            }
            else
            {
                if (attributes.Stamina > 0)
                {
                    Debug.Log("MOVING");
                    aiController.CurrentState = AIStates.States.Move; // rör sig mot target tills man target finns i line of sight    // kanske ta hänsyn till sin stamina och då ta  ett annat beslut?
                }
                else
                {
                    Debug.Log("Stamina depleted");
                    aiController.CurrentState = AIStates.States.Wait;
                }
            }
        }
    }

        //REMOVE?
    bool HoldingRangedWeapon()
    {
        if (rightHand.transform.GetChild(0).gameObject.GetType() == typeof(RangedWeapon))
        {
            return true;
        }
        return false;
    }

        //REMOVE?
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
        if(HealerIsClose())
        {
            aiController.CurrentState = AIStates.States.CallForHealing;
        }
        else if(CoverNear())
        {
            aiController.CurrentState = AIStates.States.FindCover;
        }
        else
        {
            aiController.CurrentState = AIStates.States.Attack;
        }
    }

    bool HealthLow()
    {
        return attributes.Health <= attributes.StartHealth / 3; // A THIRD OF MAX HEALTH - CHANGE ?
    }

    bool HealerIsClose() //IMPLEMENTERA SEN
    {
        return false;
    }

    bool CoverNear() //IMPLEMENTERA SEN
    {
        return false;
    }
}
