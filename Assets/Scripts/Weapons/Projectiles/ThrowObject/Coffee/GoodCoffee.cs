using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodCoffee : SpecialGrenade
{
	private GoodCoffee specialGrenadeObject;
	protected List<StatusEffectType> effects;
	protected float grenadeHeal;

	public void CreateGrenade(GameObject thrower, Vector3 position, Vector3 direction, float grenadeThrowForce, float explodeRadius, /*float grenadeExplodeForce,*/ float grenadeHeal, List<StatusEffectType> effects/*, List<WeaponEffects> ultiEffects*/)
	{
		specialGrenadeObject = Instantiate(this, position, Quaternion.LookRotation(direction));
		specialGrenadeObject.thrower = thrower;
		specialGrenadeObject.GetComponent<FieldOfView>().ViewRadius = explodeRadius;
		specialGrenadeObject.grenadeHeal = grenadeHeal;
		//specialGrenadeObject.grenadeExplodeForce = grenadeExplodeForce;
		specialGrenadeObject.GetComponent<Rigidbody>().AddForce(direction * grenadeThrowForce, ForceMode.Impulse);
		specialGrenadeObject.explodeTime = initialExplodeTime;
		GameManager.Instance.StillCheckList.Add(specialGrenadeObject.gameObject);

		specialGrenadeObject.effects = effects;
	}
	

	protected override void Explode()
	{
		List<GameObject> targetList = GetComponent<FieldOfView>().VisibleTargets;

		foreach (GameObject target in targetList)
		{
			if(target.tag == "Player")
			{
				Effects.Heal(target, grenadeHeal * (1 + thrower.GetComponentInParent<StatusEffectHandler>().DmgBoost));
				foreach (StatusEffectType effect in effects)
				{
					Effects.ApplyStatusEffect(target, effect);
				}
			}
		}

		Destroy(gameObject);
	}
}
