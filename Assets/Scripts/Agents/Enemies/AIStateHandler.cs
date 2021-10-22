using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// <para>
/// Agent handles what state to set depending on environment and status.
/// </para>
///   /// <para>
/// What class(Behaviour) the agent has changes how it makes decisions
/// </para>
///  <para>
///  Author: Tinea & Tim
///  
/// </para>
///  
/// </summary>
/// 

// Last Edited: 13/10/2021
public class AIStateHandler : MonoBehaviour
{
    Encounter encounter;
    Attributes attributes;
    GameObject rightHand, leftHand;
    FieldOfView fov;
    AIController aiController;

    // Start is called before the first frame update
    void Start()
    {
        encounter = transform.parent.gameObject.GetComponent<Encounter>();
        attributes = GetComponent<Attributes>();
        rightHand = this.gameObject.transform.GetChild(1).gameObject;
        leftHand = this.gameObject.transform.GetChild(2).gameObject;
        fov = GetComponent<FieldOfView>(); //weapon's fov
        aiController = GetComponent<AIController>();
    }
    /// <summary>
    /// Depending on what class it makes decisions differently 
    /// <param name="currentClass">
    /// Aggresive: Always moves towards target and tries to hit them no matter what <br/>
    /// Defensive: If the agent does not reach the nearest target then it will wait  <br/>
    /// Healer: Indented to heal if neccessary
    /// </param>
    /// </summary>
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
    /// <summary>
    /// How the Aggresive class get it's state
    /// </summary>
    private void AggresiveClassUpdate()
    {
        //Before the agents has had any state changes it is set to Unassigned
        if (aiController.CurrentState == AIStates.States.Unassigned)
        {
            GameObject closestPlayer = aiController.CalculateClosest(PlayerManager.players, aiController.Priorites);
            Vector3.RotateTowards(transform.forward, closestPlayer.transform.position, 1 * Time.deltaTime, 0.0f);
            //Turn towards nearest player
        }
        //DeathCheck
        if(aiController.CurrentState != AIStates.States.Dead)
        {
            if (fov.VisibleTargets.Count > 0) // <- If one or more targets is within fov range
            {
                //If there is then they are in our attack range so we attack
                aiController.CurrentState = AIStates.States.Attack;
            }
            //No target within range
            else
            {
                //If we have stamina move(Later on will move towards target but for now only sets the next action to move)
                if (attributes.Stamina > 0)
                {
                    aiController.CurrentState = AIStates.States.Move;
                }
                //No stamina wait
                else
                {
                    aiController.CurrentState = AIStates.States.Wait;
                }
            }
        }
    }
    /// <summary>
    /// How the Defensive class get it's state
    /// </summary>
    private void DefensiveClassUpdate()
    {
        //Before the agents has had any state changes it is set to Unassigned
        if (aiController.CurrentState == AIStates.States.Unassigned)
        {
            GameObject closestPlayer = aiController.CalculateClosest(PlayerManager.players, aiController.Priorites);
            Vector3.RotateTowards(transform.forward, closestPlayer.transform.position, 1 * Time.deltaTime, 0.0f);
            //Turn towards nearest player
        }
        //If we have low health and still alive then we go to LowHealthBehaviour instead to get our state
        if (HealthLow() && aiController.CurrentState != AIStates.States.Dead)
        {
            LowHealthBehaviour();
        }
        else
        {
            if (fov.VisibleTargets.Count > 0) // <- If one or more targets is within fov range
            {
                //If there is then they are in our attack range so we attack
                aiController.CurrentState = AIStates.States.Attack;
            }
            //No target within range
            else
            {
                //Checks if we have stamina and if the nearest target is reachable if not continue
                if (attributes.Stamina > 0 && aiController.FindClosestAndCheckIfReachable())
                {
                    aiController.CurrentState = AIStates.States.Move;
                }
                else
                {
                    //Either no stamina or no player is reachable
                    //Defensive unit won't move unless he thinks he can reach the closest player
                    //We wait
                    aiController.CurrentState = AIStates.States.Wait;
                }
            }
        }
    }
    /// <summary>
    /// How the Healer class get it's state
    /// </summary>
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


    /// <summary>
    /// Maybe will keep these because we might have so that ranged weapons can shoot over some obstacles
    /// </summary>
    /// <returns></returns>
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


    /// <summary>
    /// <param>
    /// Checks if the agent has low health.
    /// </param>
    /// <param>
    /// If there is a healer nearby then call for one else try to find cover 
    /// </param>
    /// <para>
    /// If no healer nor viable cover then just stand still and wait
    /// </para>
    /// </summary>
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

    /// <summary>
    /// Return true if on low health
    /// </summary>
    /// <returns></returns>
    bool HealthLow()
    {
        return attributes.Health <= attributes.StartHealth / 3; // A THIRD OF MAX HEALTH - CHANGE ?
    }
    /// <summary>
    /// Returns true if there is a healer nearby
    /// </summary>
    /// <returns></returns>
    bool HealerIsClose() //IMPLEMENTERA SEN
    {
        return false;
    }
    /// <summary>
    /// Returns ture if viable cover nearby
    /// </summary>
    /// <returns></returns>
    bool CoverNear() //IMPLEMENTERA SEN
    {
        return false;
    }





}
