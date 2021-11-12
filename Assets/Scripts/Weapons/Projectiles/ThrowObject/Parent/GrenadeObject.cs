using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// The grenade object/projectile
/// </para>
///   
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15/10-30
public class GrenadeObject : MonoBehaviour
{
	protected GameObject thrower;
	private GrenadeObject grenadeObject;
	[SerializeField]
	private GameObject FOVVisualization;
	[SerializeField]
	protected float explodeRadius = 3;
	protected float grenadeDamage;
	protected float grenadeExplodeForce;
	[SerializeField]
	private float initialExplodeTime;
	private float explodeTime;
	protected List<WeaponEffects> effects;

	protected bool isObjectThrown;

	public void CreateGrenade(GameObject thrower, Vector3 position, Vector3 direction, float grenadeThrowForce, float grenadeExplodeForce, float grenadeDamage, List<WeaponEffects> effects)
	{
		grenadeObject = Instantiate(this, position, Quaternion.LookRotation(direction));
		grenadeObject.thrower = thrower;
		grenadeObject.GetComponent<FieldOfView>().ViewRadius = explodeRadius;
		grenadeObject.grenadeDamage = grenadeDamage;
		grenadeObject.grenadeExplodeForce = grenadeExplodeForce;
		grenadeObject.GetComponent<Rigidbody>().AddForce(direction * grenadeThrowForce, ForceMode.Impulse);
		grenadeObject.explodeTime = initialExplodeTime;
		GameManager.Instance.StillCheckList.Add(grenadeObject.gameObject);

		this.effects = effects;
	}

	protected void SetExplosion()
	{
		FOVVisualization.SetActive(true);
		StartCoroutine(CountdownTime(explodeTime));
	}

	private IEnumerator CountdownTime(float time)
	{
		yield return new WaitForSeconds(time);
		Explode();
	}

	private void Explode()
	{
		//thrower.GetComponent<StatusEffectHandler>().DmgBoost;
		List<GameObject> targetList = GetComponent<FieldOfView>().VisibleTargets;

		foreach (GameObject target in targetList)
		{
			Vector3 explosionForceDirection = target.transform.position - transform.position;
			explosionForceDirection.y = 0;
			explosionForceDirection.Normalize();

			Effects.RegularDamage(target, grenadeDamage * (1 + thrower.GetComponentInParent<StatusEffectHandler>().DmgBoost), thrower);
			Effects.ApplyForce(target, explosionForceDirection * grenadeExplodeForce);
			Effects.ApplyWeaponEffects(target, effects);
		}

		Destroy(gameObject);
	}
}
