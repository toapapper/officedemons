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

    FieldOfView fov;
    AIController aiController;

    // Start is called before the first frame update
    void Start()
    {
        encounter = transform.parent.gameObject.GetComponent<Encounter>();
        attributes = GetComponent<Attributes>();
        fov = GetComponent<FieldOfView>(); //weapon's fov
        aiController = GetComponent<AIController>();
    }


    /// <summary>
    /// Updates the state of the agent
    /// </summary>
    public void StateUpdate()
    {
        if (attributes.Health <= 0)
        {
            aiController.CurrentState = AIStates.States.Dead;
        }
        else if(GetComponent<Attributes>().statusEffectHandler.Paralyzed || (attributes.Stamina <= 0 && !CanAttackPlayer()))
        {
            aiController.CurrentState = AIStates.States.Wait;
            aiController.ActionIsLocked = true;
        }
        else
        {
            //Before the agents has had any state changes it is set to Unassigned
            if (aiController.CurrentState == AIStates.States.Unassigned)
            {
                aiController.SetTarget(aiController.GetTargetPlayer(encounter.GetComponentInChildren<AIManager>().PlayerList), AIController.TargetTypes.Player);
                Vector3.RotateTowards(transform.forward, aiController.TargetPosition, 1 * Time.deltaTime, 0.0f);
            }
            //DeathCheck
            if (aiController.CurrentState != AIStates.States.Dead && attributes.Health > 0)
            {
                if (HealthLow() && !HasAdvantage() && attributes.Stamina > 0 && !aiController.ReachedTargetPosition())  // if low health and disadvantage and has stamina
                {
                    aiController.CurrentState = AIStates.States.FindCover;
                }
                else if (aiController.CurrentState != AIStates.States.Move && !aiController.IsArmed() && attributes.Stamina > 0)
                {
                    aiController.CurrentState = AIStates.States.SearchingForWeapon;
                }
                else if (aiController.TargetType == AIController.TargetTypes.ShootSpot && aiController.ReachedTargetPosition())
                {
                    aiController.Target = aiController.GetTargetPlayer(encounter.GetComponentInChildren<AIManager>().PlayerList);
                    aiController.TargetType = AIController.TargetTypes.Player;
                }
                
                else if (!TooCloseToAttack() && aiController.TargetType == AIController.TargetTypes.Player && CanAttackPlayer())
                {
                    aiController.CurrentState = AIStates.States.Attack;
                    aiController.ActionIsLocked = true;
                }
                else
                {
                    if (aiController.TargetType == AIController.TargetTypes.ShootSpot && aiController.ReachedTargetPosition())
                    {
                        aiController.Target = aiController.GetTargetPlayer(encounter.GetComponentInChildren<AIManager>().PlayerList);
                        aiController.TargetType = AIController.TargetTypes.Player;
                    }

                    //If we have stamina move
                    if (attributes.Stamina > 0 && !aiController.ReachedTargetPosition())
                    {
                        aiController.CurrentState = AIStates.States.Move;
                    }
                    
                    //No stamina -> wait/attack
                    else
                    {
                        if (aiController.TargetType == AIController.TargetTypes.Player && CanAttackPlayer())
                        {
                            aiController.CurrentState = AIStates.States.Attack;
                            aiController.ActionIsLocked = true;
                        }
                        else
                        {
                            
                            if (attributes.Stamina > 0 && TooCloseToAttack() && aiController.TargetType == AIController.TargetTypes.Player)
                            {
                                aiController.GetShootPosition();
                                aiController.CurrentState = AIStates.States.Move;
                            }
                            else if (aiController.TargetType == AIController.TargetTypes.Item && aiController.ReachedTargetPosition())
                            {
                                aiController.CurrentState = AIStates.States.Move;
                            }
                            else if (!TooCloseToAttack() && aiController.TargetType == AIController.TargetTypes.Player && CanAttackPlayer())
                            {
                                Vector3.RotateTowards(transform.forward, aiController.TargetPosition, 1 * Time.deltaTime, 0.0f);
                                aiController.CurrentState = AIStates.States.Attack;
                                aiController.ActionIsLocked = true;
                            }
                            else
                            { 
                                aiController.CurrentState = AIStates.States.Wait;
                                aiController.ActionIsLocked = true;
                            }
                        }
                    }
                }
            }
        }
        Debug.Log("state: " + aiController.CurrentState);
    }

    private bool TooCloseToAttack()
    {
        if (aiController.HoldingMeleeWeapon())
        {
            return false;
        }
        return (Vector3.Distance(gameObject.transform.position, aiController.TargetPosition)) < 2;
    }

    private bool CanAttackPlayer()
    {
        if (aiController.HoldingRangedWeapon())
        {
            foreach(GameObject player in encounter.GetComponentInChildren<AIManager>().PlayerList)
            {
                // Raycast to every player and se eif move not needed
                Vector3 direction = (aiController.Target.transform.position - transform.position).normalized;
                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(transform.position, direction, out hit))
                {
                    if (hit.transform.gameObject.tag == "Player")
                    {
                        if(Vector3.Angle(transform.forward, hit.transform.position - transform.position) < 10f)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        else
        {
            fov.FindVisibleTargets();
            
            if (fov.VisibleTargets.Count > 0)
            {
                //if player in range
                foreach (GameObject target in fov.VisibleTargets)
                {
                    if (target != null && target.tag == "Player")
                    {
                        return true;
                    }
                }
            }

            if (aiController.Target != null && aiController.TargetType == AIController.TargetTypes.Player && Vector3.Distance(aiController.TargetPosition, transform.position) <= GetComponentInChildren<AbstractWeapon>().ViewDistance + 1)
            {
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// Return true if on low health
    /// </summary>
    /// <returns></returns>
    bool HealthLow()
    {
        return attributes.Health <= attributes.StartHealth / 2; //
    }

    // Check if players are fewer than AI, if players are unarmed but AI have weapons
    public bool HasAdvantage()
    {
        int armedPlayers = 0;
        int alivePlayers = 0;
        int armedEnemies = 0;
        int aliveEnemies = encounter.GetComponentInChildren<AIManager>().EnemyList.Count;

        //count how many players are alive and armed
        foreach (GameObject player in encounter.GetComponentInChildren<AIManager>().PlayerList)
        {
            if (gameObject.GetComponentInChildren<RangedWeapon>() || gameObject.GetComponentInChildren<MeleeWeapon>())
            {
                armedPlayers++;
            }

            if (player.GetComponent<Attributes>().Health > 0)
            {
                alivePlayers++;
            }
        }

        foreach (GameObject enemy in encounter.GetComponentInChildren<AIManager>().EnemyList)
        {
            if (enemy.name != "tank" && enemy.GetComponent<AIController>().IsArmed())
            {
                armedEnemies++;
            }
        }

        if (armedPlayers < armedEnemies || aliveEnemies > alivePlayers) // Add more complexity ?
        {
            return true;
        }
        return false;
    }
}
