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

    public void GetState(Class currentClass)
    {
        switch (currentClass)
        {
            case Class.Aggresive:
                AggresiveClassUpdate();
                break;
            case Class.Defensive:
                DefensiveClassUpdate();
                break;
            case Class.Healer:
                HealerClassUpdate();
                break;
            default:
                break;
        }
    }

    private void AggresiveClassUpdate()
    {
        //GameObject weapon = rightHand.transform.GetChild(0).gameObject;
        if (aiController.CurrentState == AIStates.States.Unassigned)
        {
            GameObject closestPlayer = aiController.CalculateClosest(PlayerManager.players, aiController.Priorites);
            Vector3.RotateTowards(transform.forward, closestPlayer.transform.position, 1 * Time.deltaTime, 0.0f);
            //Turn towards nearest player
        }
        if(aiController.CurrentState != AIStates.States.Dead)
        {
            if (fov.visibleTargets.Count > 0) // <- om target finns i line of sight för vapnet
            {
                //Debug.Log("TARGET FOUND");
                aiController.CurrentState = AIStates.States.Attack;
            }
            else
            {
                if (attributes.Stamina > 0)
                {
                    aiController.CurrentState = AIStates.States.Move; // rör sig mot target tills man target finns i line of sight    // kanske ta hänsyn till sin stamina och då ta  ett annat beslut?
                }
                else
                {
                    //Debug.Log("Stamina depleted");
                    aiController.CurrentState = AIStates.States.Wait;
                }
            }
        }
    }

    private void DefensiveClassUpdate()
    {
        if (aiController.CurrentState == AIStates.States.Unassigned)
        {
            GameObject closestPlayer = aiController.CalculateClosest(PlayerManager.players, aiController.Priorites);
            Vector3.RotateTowards(transform.forward, closestPlayer.transform.position, 1 * Time.deltaTime, 0.0f);
            //Turn towards nearest player
        }
        if (HealthLow() && aiController.CurrentState != AIStates.States.Dead)
        {
            LowHealthBehaviour();
        }
        else
        {
            if (fov.visibleTargets.Count > 0) // <- om target finns i line of sight för vapnet
            {
                //Debug.Log("TARGET FOUND");
                aiController.CurrentState = AIStates.States.Attack;
            }
            else
            {
                if (attributes.Stamina > 0 && aiController.FindClosestAndCheckIfReachable())
                {
                    aiController.CurrentState = AIStates.States.Move;
                }
                else
                {
                    //Debug.Log("Stamina depleted");
                    //Defensive unit won't move unless he thinks he can reach the closest player
                    Debug.LogError("NO PLAYER REACHABLE");
                    aiController.CurrentState = AIStates.States.Wait;
                }
            }
        }
    }

    private void HealerClassUpdate()
    {
        //Implement this
        //for (int i = 0; i < enemies.Count; i++)
        //{
        //    if (enemies[i].aiController.CurrentState = AIStates.States.CallForHealing)
        //    {
        //        //AgentsCallingForHelp.add(enemies[i]);
        //        //GameObject closestAgentCallingForHelp = GetClosest(CallingForHelpAgentsList);
        //        if (fov.visibleTargets.Contains(closestAgentCallingForHelp))
        //        {
        //            //Heal
        //        }
        //        else
        //        {
        //            //Move towards closestAgentCallingForHelp
        //        }
        //    }

        //}
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
            aiController.CurrentState = AIStates.States.Wait;
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
