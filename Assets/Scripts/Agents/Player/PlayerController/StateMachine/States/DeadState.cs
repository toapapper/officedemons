using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// <para>
/// Character can not do anything during this state. Additionally it restricts state transitions when in combat.
/// </para> 
/// 
///  <para>
///  Author: Ossian
/// </para>
/// </summary>

// Last Edited: 2021-10-22

public class DeadState : AbstractPlayerState
{
    GameObject particleEffect;

    /// <summary>
    /// Simple override of the base that only calls the base if the criteria is met
    /// </summary>
    /// <param name="state"></param>
    public override void TransitionState(IPlayerState state)
    {
        if (state is ReviveState || state is OutOfCombatState)
        {
            base.TransitionState(state);
        }
    }

    /// <summary>
    /// Simple coroutine reviving this.gameObject after one second
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayedSelfRevive()
    {
        yield return new WaitForSeconds(1);
        Effects.Revive(gameObject);
        yield return null;
    }

    private Color originalColor; //is here temporarily i assume. This is because we have no proper animation to show one is dead other than to change the color
    public override void OnStateEnter()
    {
        Debug.Log("Enters DeadState" + this);
        originalColor = GetComponentInChildren<MeshRenderer>().material.color;

        int layerMask = 1 << 10;
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            particleEffect = Instantiate(Resources.Load("PentagramEffect"), new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z), transform.rotation * Quaternion.Euler(-90, 0, 0)) as GameObject;
            particleEffect.transform.parent = gameObject.transform;
        }

        GetComponentInChildren<MeshRenderer>().material.color = Color.black;
        if (GameManager.Instance.CurrentCombatState == CombatState.none)
        {
            StartCoroutine(DelayedSelfRevive());
        }

        gameObject.GetComponent<Animator>().SetTrigger("isDead");
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
    }

    public override void OnStateExit()
    {
        Debug.Log("Exits DeadState" + this);
        GetComponentInChildren<MeshRenderer>().material.color = originalColor;
        if (particleEffect)
        {
            Destroy(particleEffect);
        }
        gameObject.GetComponent<Animator>().SetTrigger("isRevived");
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        gameObject.GetComponent<NavMeshAgent>().enabled = true;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
    }
}
