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
        //Before the agents has had any state changes it is set to Unassigned
        if (aiController.CurrentState == AIStates.States.Unassigned)
        {
            GameObject closestPlayer = aiController.CalculateClosest(PlayerManager.players, encounter.GetComponentInChildren<AIManager>().KillPriority);
            Vector3.RotateTowards(transform.forward, closestPlayer.transform.position, 1 * Time.deltaTime, 0.0f);
            aiController.TargetPlayer = closestPlayer;
            aiController.Target = closestPlayer;
            aiController.TargetPosition = closestPlayer.transform.position;
            //Turn towards nearest player
        }
        //DeathCheck       
        if (aiController.CurrentState != AIStates.States.Dead && attributes.Health > 0)
        {
            if (HealthLow() && !HasAdvantage() && attributes.Stamina > 0 && !HasReachedTargetPosition()) // if low health and disadvantage and has stamina
            {
                aiController.CurrentState = AIStates.States.FindCover;

            }
            else if (aiController.CurrentState != AIStates.States.Move && !aiController.IsArmed() && attributes.Stamina > 0)
            {
                aiController.CurrentState = AIStates.States.SearchingForWeapon;
            }
            else if (PlayerIsInRange()) // <- If one or more players are within fov range
            {
                aiController.CurrentState = AIStates.States.Attack;
                aiController.ActionIsLocked = true;
            }
            //No target within range and health is fine 
            else
            {
                //If we have stamina move(Later on will move towards target but for now only sets the next action to move)
                if (attributes.Stamina > 0 && gameObject.transform.position != aiController.TargetPosition)
                {
                    aiController.CurrentState = AIStates.States.Move;
                }
                //No stamina -> wait/attack
                else
                {
                    if (PlayerIsInRange())
                    {
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

    private bool PlayerIsInRange()
    {
        if (aiController.HoldingRangedWeapon())
        {
            Vector3 direction = (aiController.TargetPlayer.transform.position - transform.position).normalized;
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(transform.position, direction, out hit))
            {
                if (hit.transform.gameObject.tag == "Player")
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
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

    private bool HasReachedTargetPosition()
    {
        return aiController.TargetPosition == gameObject.transform.position;
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
            if (player.GetComponent<WeaponHand>().transform.GetChild(0).gameObject.GetType() == typeof(MeleeWeapon) || player.GetComponent<WeaponHand>().transform.GetChild(0).gameObject.GetType() == typeof(RangedWeapon))
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
            if (enemy.GetComponent<AIController>().IsArmed())
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
