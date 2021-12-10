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

    Transform towerTransform;

    void Start()
    {
        aiManager = transform.parent.GetComponentInChildren<AIManager>();
        towerTransform = this.gameObject.transform.GetChild(0);
    }

    public void PerformBehaviour()
    {
        // child "tiger-head" is the rotating part
        StateUpdate();
        switch (CurrentState)
        {
            case TankStates.Rotate:
                Debug.LogError("ROTATING");
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
        //
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
        Vector3 targetDirection = targetPosition - towerTransform.position;

        // The step size is equal to speed times frame time.
        float singleStep = rotationSpeed * Time.deltaTime;

        //maxRotation -= singlestep

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(towerTransform.forward, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(towerTransform.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        
        towerTransform.rotation = Quaternion.LookRotation(newDirection);
        towerTransform.rotation = Quaternion.Euler(0, towerTransform.eulerAngles.y, 0);
    }

    private bool RotationFinished() //add maxrotation?
    {
        //return Vector3.Angle(new Vector3(0, towerTransform.rotation.y, 0), new Vector3(0, targetRotation.y, 0)) <= 1;
        return (Quaternion.Angle(towerTransform.rotation, targetRotation) <= 1f);
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
                    targetRotation = Quaternion.LookRotation((TargetPosition - towerTransform.position).normalized);
                    targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
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
        ActionIsLocked = false;
    }

    public GameObject GetTargetPlayer(List<GameObject> players) // maybe change to always rotate but not shoot 
    {
        GameObject target = null;

        float minHealth = Mathf.Infinity;
        Vector3 myPosition = new Vector3(towerTransform.position.x, towerTransform.position.y+ 10, towerTransform.position.z);                  //  hard coded

        // of all players possible to shoot, chose the one with lowest health
        foreach (GameObject player in players)
        {
            // Raycast to player
            RaycastHit hit = new RaycastHit();
            Vector3 direction = (player.transform.position - myPosition).normalized;

            if (Physics.Raycast(myPosition, direction, out hit))
            {
                if (hit.transform.gameObject.tag == "Player")
                {
                    if (player.gameObject.GetComponent<Attributes>().Health <= minHealth)
                    {
                        target = player;
                        minHealth = player.GetComponent<Attributes>().Health;
                    }
                }
            }
        }

        return target;
    }
}
