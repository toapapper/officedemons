using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    private AIManager aiManager;
    private const float maxRotation = 45f;
    private const float rotationSpeed = 0.5f;

    private Vector3 targetPosition;
    public Vector3 TargetPosition
    {
        get { return targetPosition; }
        set { targetPosition = value; }
    }

    Quaternion targetRotation;

    private bool inActiveCombat;
    public bool InActiveCombat
    {
        get { return inActiveCombat; }
        set { inActiveCombat = value; }
    }

    public enum TankStates { Wait, Shoot, Rotate, Unassigned, Dead };
    private TankStates currentState;
    public TankStates CurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }

    private bool actionIsLocked;
    public bool ActionIsLocked
    {
        get { return actionIsLocked; }
        set { actionIsLocked = value; }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void PerformBehaviour()
    {
        // child "tiger-head" is the rotating part
        StateUpdate();

        switch (CurrentState)
        {
            case TankStates.Rotate:
                RotateTowards(TargetPosition);
                break;
        }
    }

    public void PerformAction()
    {
        switch (CurrentState)
        {
            case TankStates.Shoot:
                //SHOOTERINO
                break;
        }
    }

    private void Shoot()
    {

    }

    public void Die()
    {
        aiManager.RemoveAction(gameObject);
        aiManager.EnemyList.Remove(gameObject);

        // Make object black and smoke
    }



    private void RotateTowards(Vector3 targetPosition)
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = targetPosition - transform.position;

        // The step size is equal to speed times frame time.
        float singleStep = rotationSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(transform.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private bool RotationFinished()
    {
        return (Quaternion.Angle(transform.rotation, targetRotation) <= 0.01f);
    }

    // The decisions are made here
    private void StateUpdate()
    {
        switch (CurrentState)
        {
            case TankStates.Unassigned:

                // find traget 
                GameObject target = GetTargetPlayer(aiManager.PlayerList);

                if (target != null)
                {
                    TargetPosition = target.transform.position;
                    targetRotation = Quaternion.LookRotation((TargetPosition - transform.position).normalized);
                    CurrentState = TankStates.Rotate;
                }
                // if no player found that you can shoot, wait
                else
                {
                    CurrentState = TankStates.Wait;
                }

                break;

            case TankStates.Rotate:
                if (RotationFinished())
                {
                    if (InLineOfSight())
                    {
                        CurrentState = TankStates.Shoot;
                    }
                    else
                    {
                        CurrentState = TankStates.Wait;
                    }
                }
                break;

            case TankStates.Wait:
                //aiManager.SaveAction(this.gameObject);
                ActionIsLocked = true;
                break;

            case TankStates.Shoot:
                aiManager.SaveAction(this.gameObject);
                ActionIsLocked = true;
                break;
        }
    }

    private bool InLineOfSight() // IMPLEMENTERA
    {
        return true;
    }

    public void ResetValues()
    {
        CurrentState = TankStates.Unassigned;
        targetPosition = Vector3.zero;
        targetRotation = Quaternion.identity;
    }

    // IMPLEMENT
    public GameObject GetTargetPlayer(List<GameObject> players) // maybe only target?
    {
        GameObject target = null;

        float minHealth = Mathf.Infinity;

        // of all players possible to shoot, chose the one with lowest health
        foreach (GameObject player in players)
        {
            // Raycast to player
            // if hit is player and <= minHealth
            //update target
        }

        return target;
    }
}
