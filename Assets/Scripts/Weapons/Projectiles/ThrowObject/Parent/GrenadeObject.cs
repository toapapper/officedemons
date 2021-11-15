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
	private GrenadeObject grenadeObject;
	[SerializeField]
	private GameObject FOVVisualization;
	[SerializeField]
	private float explodeRadius = 3;
	private float grenadeDamage;
	private float grenadeExplodeForce;
	[SerializeField]
	private float initialExplodeTime;
	private float explodeTime;
	private List<WeaponEffects> effects;

	protected bool isObjectThrown;
	[SerializeField]
	protected GameObject particleEffect;

	public void CreateGrenade(Vector3 position, Vector3 direction, float grenadeThrowForce, float grenadeExplodeForce, float grenadeDamage, List<WeaponEffects> effects)
	{
		grenadeObject = Instantiate(this, position, Quaternion.LookRotation(direction));
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
		List<GameObject> targetList = GetComponent<FieldOfView>().VisibleTargets;

		foreach (GameObject target in targetList)
		{
			Vector3 explosionForceDirection = target.transform.position - transform.position;
			explosionForceDirection.y = 0;
			explosionForceDirection.Normalize();

			Effects.Damage(target, grenadeDamage);
			Effects.ApplyForce(target, explosionForceDirection * grenadeExplodeForce);
			Effects.ApplyWeaponEffects(target, effects);
		}
		Instantiate(particleEffect, transform.position, Quaternion.Euler(90, 0,0));

		Destroy(gameObject);
	}
}
