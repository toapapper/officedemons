using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    private AIManager aiManager;
    private const float maxRotationX = 20f;
    private const float rotationSpeed = 0.5f;

    private float timer = 0.0f;
    private const float maxTurnTime = 4f;

    private Vector3 targetPosition;
    public Vector3 TargetPosition
    {
        get { return targetPosition; }
        set { targetPosition = value; }
    }

    Quaternion targetTowerRotation;
    Quaternion targetPipeRotation;

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
    Transform pipeTransform;

    void Start()
    {
        gameObject.name = "tank";
        gameObject.transform.GetChild(0).name = "tank";
        gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).name = "tank";

        aiManager = transform.parent.GetComponentInChildren<AIManager>();
        towerTransform = this.gameObject.transform.GetChild(0);
        pipeTransform = towerTransform.GetChild(0);
    }

    public void PerformBehaviour()
    {
        // child "tiger-head" is the rotating part
        StateUpdate();

        timer += Time.deltaTime;
        float seconds = timer % 60;

        if (timer >= maxTurnTime)
        {
            Debug.Log("TANK exceeded max turn time of " + maxTurnTime + " seconds, chose WAIT");
            CurrentState = TankStates.Wait;
        }

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
                Shoot();
                break;
        }
    }

    private void Shoot()
    {
        GetComponentInChildren<TankShotWeapon>().DoAction();
    }

    public void Die()
    {
        aiManager.RemoveAction(gameObject);
        //aiManager.EnemyList.Remove(gameObject);

        // Make object black and smoke
        GetComponent<DestructibleObjects>().Explode();
        CurrentState = TankStates.Dead;
    }



    private void RotateTowards(Vector3 targetPosition) // lägg till pipan i x led // max rotation för x 20 grader
    {
        // Determine which direction to rotate towards
        Vector3 targetTowerDirection = targetPosition - towerTransform.position;
        Vector3 targetPipeDirection = targetPosition - pipeTransform.position;

        // The step size is equal to speed times frame time.
        float singleStep = rotationSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newTowerDirection = Vector3.RotateTowards(towerTransform.forward, targetTowerDirection, singleStep, 0.0f);
        Vector3 newPipeDirection = Vector3.RotateTowards(pipeTransform.forward, targetPipeDirection, singleStep, 0.0f);
        
        //Tower
        towerTransform.rotation = Quaternion.LookRotation(newTowerDirection);
        towerTransform.rotation = Quaternion.Euler(0, towerTransform.eulerAngles.y, 0);

        //Pipe
        pipeTransform.localRotation = Quaternion.LookRotation(newPipeDirection);
        pipeTransform.localRotation = Quaternion.Euler(pipeTransform.eulerAngles.x, 0, 0);
    }

    private bool RotationFinished() //add maxrotation?
    {
        return ((Quaternion.Angle(towerTransform.rotation, targetTowerRotation) <= 1f) && (Quaternion.Angle(pipeTransform.localRotation, targetPipeRotation) <= 1f ));
    }

    // The decisions are made here
    private void StateUpdate()
    {
        if (GetComponent<Attributes>().Health <= 0)
        {
            CurrentState = TankStates.Dead;
            ActionIsLocked = true;
        }

        switch (CurrentState)
        {
            case TankStates.Unassigned:

                // find traget 
                GameObject target = GetTargetPlayer(aiManager.PlayerList);
                
                if (target != null)
                {
                    TargetPosition = target.transform.position;
                    targetTowerRotation = Quaternion.LookRotation((TargetPosition - towerTransform.position).normalized);
                    targetTowerRotation = Quaternion.Euler(0, targetTowerRotation.eulerAngles.y, 0);

                    //TargetPipeRotation
                    targetPipeRotation = Quaternion.LookRotation((TargetPosition - pipeTransform.position).normalized);
                    targetPipeRotation = Quaternion.Euler(targetPipeRotation.eulerAngles.x, 0, 0);

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
                    Debug.Log("Rotation FInished");
                    if (InLineOfSight() && !TooCloseToShoot(towerTransform.position, TargetPosition)) 
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
                aiManager.SaveAction(this.gameObject);
                ActionIsLocked = true;
                break;

            case TankStates.Shoot:
                aiManager.SaveAction(this.gameObject);
                ActionIsLocked = true;
                break;
        }

        
    }

    private bool InLineOfSight() // IMPLEMENTERA ?
    {
        return true;
    }

    public void ResetValues()
    {
        timer = 0.0f;
        CurrentState = TankStates.Unassigned;
        targetPosition = Vector3.zero;
        targetTowerRotation = Quaternion.identity;
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
                    if (player.gameObject.GetComponent<Attributes>().Health <= minHealth && !TooCloseToShoot(myPosition, player.transform.position))
                    {
                        target = player;
                        minHealth = player.GetComponent<Attributes>().Health;
                    }
                }
            }
        }

        return target;
    }

    private bool TooCloseToShoot(Vector3 myPosition, Vector3 targetPosition)
    {
        return Vector3.Distance(myPosition, targetPosition) < 3;
    }
}
