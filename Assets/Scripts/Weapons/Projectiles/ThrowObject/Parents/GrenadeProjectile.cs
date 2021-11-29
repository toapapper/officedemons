using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// The abstract grenade class
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15-11-19
public abstract class GrenadeProjectile : MonoBehaviour
{
    [SerializeField]
    protected GameObject particleEffect;
    [SerializeField]
    protected FieldOfView FOV;

    protected GameObject thrower;
    protected bool isObjectThrown;

    protected float healthModifyAmount;
    protected float explosionForce;

    protected List<WeaponEffects> weaponEffects;
    protected List<StatusEffectType> statusEffects;

    private void Awake()
	{
        GameManager.Instance.StillCheckList.Add(gameObject);
    }

	protected virtual void FixedUpdate()
	{
        if (transform.position.y < -10f)
        {
            DestroyGrenade();
        }
    }

	protected IEnumerator CountdownTime(float time)
    {
        yield return new WaitForSeconds(time);

        Explode();
    }

    protected virtual void Explode()
    {
        Instantiate(particleEffect, transform.position, Quaternion.Euler(90, 0, 0));
        ImpactAgents();
        DestroyGrenade();
    }

    protected abstract void ImpactAgents();

	private void DestroyGrenade()
	{
        GameManager.Instance.StillCheckList.Remove(gameObject);
        Destroy(gameObject);
    }
}
