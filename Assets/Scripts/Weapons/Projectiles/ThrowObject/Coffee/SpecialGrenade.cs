using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpecialGrenade : MonoBehaviour
{
	protected GameObject thrower;
	//protected SpecialGrenade specialGrenadeObject;
	[SerializeField]
	protected GameObject FOVVisualization;
	//[SerializeField]
	//protected float explodeRadius = 2;
	//protected float grenadeDamage;
	//protected float grenadeExplodeForce;
	[SerializeField]
	protected float initialExplodeTime;
	protected float explodeTime;
	//protected List<WeaponEffects> effects;
	//protected List<WeaponEffects> ultiEffects;

	protected bool isObjectThrown;

	//public void CreateGrenade(GameObject thrower, Vector3 position, Vector3 direction, float grenadeThrowForce, float explodeRadius, float grenadeExplodeForce, float grenadeDamage, List<WeaponEffects> effects/*, List<WeaponEffects> ultiEffects*/)
	//{
	//	specialGrenadeObject = Instantiate(this, position, Quaternion.LookRotation(direction));
	//	specialGrenadeObject.thrower = thrower;
	//	specialGrenadeObject.GetComponent<FieldOfView>().ViewRadius = explodeRadius;
	//	specialGrenadeObject.grenadeDamage = grenadeDamage;
	//	specialGrenadeObject.grenadeExplodeForce = grenadeExplodeForce;
	//	specialGrenadeObject.GetComponent<Rigidbody>().AddForce(direction * grenadeThrowForce, ForceMode.Impulse);
	//	specialGrenadeObject.explodeTime = initialExplodeTime;
	//	GameManager.Instance.StillCheckList.Add(specialGrenadeObject.gameObject);

	//	specialGrenadeObject.effects = effects;
	//	//specialGrenadeObject.ultiEffects = ultiEffects;
	//}

	private void FixedUpdate()
	{
		if (isObjectThrown)
		{
			if (transform.position.y < 0.2f)
			{
				Explode();
			}
			else if (GetComponent<Rigidbody>().velocity.magnitude < 0.5f)
			{
				Explode();
			}
		}
		else
		{
			isObjectThrown = true;
		}
	}

	//protected void SetExplosion()
	//{
	//	FOVVisualization.SetActive(true);
	//	StartCoroutine(CountdownTime(explodeTime));
	//}

	//private IEnumerator CountdownTime(float time)
	//{
	//	yield return new WaitForSeconds(time);
	//	Explode();
	//}

	protected abstract void Explode();
	//protected virtual void Explode()
	//{
	//	List<GameObject> targetList = GetComponent<FieldOfView>().VisibleTargets;

	//	foreach (GameObject target in targetList)
	//	{
	//		Vector3 explosionForceDirection = target.transform.position - transform.position;
	//		explosionForceDirection.y = 0;
	//		explosionForceDirection.Normalize();

	//		Effects.RegularDamage(target, grenadeDamage * (1 + thrower.GetComponentInParent<StatusEffectHandler>().DmgBoost), thrower);
	//		Effects.ApplyForce(target, explosionForceDirection * grenadeExplodeForce);
	//		Effects.ApplyWeaponEffects(target, effects);
	//	}

	//	Destroy(gameObject);
	//}

	//private void OnCollisionEnter(Collision collision)
	//{
	//	Explode();
	//}
}
