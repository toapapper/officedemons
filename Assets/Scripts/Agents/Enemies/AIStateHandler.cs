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
            GameObject closestPlayer = aiController.CalculateClosest(PlayerManager.players, encounter.GetComponentInChildren<AIManager>().killPriority);
            Vector3.RotateTowards(transform.forward, closestPlayer.transform.position, 1 * Time.deltaTime, 0.0f);
            //Turn towards nearest player
        }
        //DeathCheck       
        if (aiController.CurrentState != AIStates.States.Dead && attributes.Health > 0)
        {
            if (HealthLow() && !HasAdvantage() && attributes.Stamina > 0 && !HasReachedTargetPosition()) // if low health and disadvantage and has stamina
            {
                aiController.CurrentState = AIStates.States.FindCover;

            }
            else if (gameObject.GetComponent<WeaponHand>().objectInHand == null && attributes.Stamina > 0 && aiController.GetClosestWeapon(0, 100) != null)
            {
                //Might remove it later but for now just a quick fix
                aiController.UpdateClosestPlayer();
                //Check if there's a ranged weapon is closer than the closest enemy to try and shoot him from range
                GameObject rangedWeapon = aiController.GetClosestWeapon(10, 100);
                if ( rangedWeapon != null && aiController.CalculateDistance(aiController.ClosestPlayer) <= aiController.CalculateDistance(rangedWeapon))
                {
                    WalkTowardsWeapon(10);
                }
                else
                {

                    WalkTowardsWeapon(0);
                }
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
            Vector3 direction = (aiController.targetPlayer.transform.position - transform.position).normalized;
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(transform.position, direction, out hit))
            {
                if (hit.transform.gameObject.tag == "Player")
                {
                    // if not too far away
                    if ((hit.transform.position - transform.position).magnitude <= fov.ViewRadius * 4) // maybe change fov.ViewRadius since bullets can travel further
                    {
                        return true;
                    }
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
                    if (target.tag == "Player")
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

    // Check if players are fewer than AI, if players are unarmed but AI have weapons
    public bool HasAdvantage()
    {
        int armedPlayers = 0;
        int alivePlayers = 0;
        int armedEnemies = 0;
        int aliveEnemies = encounter.GetComponentInChildren<AIManager>().enemyList.Count;

        //count how many players are alive and armed
        foreach (GameObject player in encounter.GetComponentInChildren<AIManager>().playerList)
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

        foreach (GameObject enemy in encounter.GetComponentInChildren<AIManager>().enemyList)
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

    private bool HasReachedTargetPosition()
    {
        return aiController.TargetPosition == gameObject.transform.position;
    }


    private void WalkTowardsWeapon(float minimumWeaponRange)
    {
        GameObject weapon = aiController.GetClosestWeapon(minimumWeaponRange, float.MaxValue);
        aiController.TargetPosition = weapon.transform.position;
        aiController.Target = weapon;
        aiController.CurrentState = AIStates.States.Move;
    }

    private void WalkTowardsWeapon(GameObject weapon)
    {
        aiController.TargetPosition = weapon.transform.position;
        aiController.Target = weapon;
        aiController.CurrentState = AIStates.States.Move;
    }
}
