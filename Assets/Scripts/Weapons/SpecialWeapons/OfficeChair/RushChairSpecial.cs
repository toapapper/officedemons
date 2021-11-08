using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushChairSpecial : OfficeChairSpecial
{
	[SerializeField]
	private GameObject laserAim;
	[SerializeField]
	private float rushForce = 100f;

	public bool isProjectile;


	public override void SetAim(FieldOfView fov, Gradient gradient)
	{
		laserAim.SetActive(true);
		GetComponentInChildren<LineRenderer>().colorGradient = gradient;
		laserAim.SetActive(false);
	}
	public override void ToggleAim(bool isActive, GameObject FOVView, GameObject throwAim)
	{
		laserAim.SetActive(isActive);
	}

	public override void StartAttack(Animator animator)
	{
		animator.SetTrigger("isStartSpecialRush");
	}
	public override void Attack(Animator animator)
	{
		animator.SetTrigger("isSpecialRush");
	}

	public override void DoSpecialAction(FieldOfView fov)
	{
		holderAgent.GetComponent<Rigidbody>().AddForce(holderAgent.transform.forward * rushForce, ForceMode.VelocityChange);
		isProjectile = true;
		GameManager.Instance.StillCheckList.Add(holderAgent);
		//Vector3 forward = transform.parent.parent.forward;
		//forward.y = 0;
		//forward.Normalize();
		//Vector3 right = new Vector3(forward.z, 0, -forward.x);

		//Vector3 direction = (Quaternion.AngleAxis(-GetComponentInParent<SpecialHand>().ThrowAim.initialAngle, right) * forward).normalized;
		//float throwForce = GetComponentInParent<SpecialHand>().ThrowAim.initialVelocity;

		//grenade.GetComponent<GrenadeObject>().CreateGrenade(transform.position, direction, throwForce, HitForce, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), effects);
	}
	public void OnTriggerEnter(Collider other)
	{
		if (isProjectile)
		{
			if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
			{
				Vector3 forceDirection = other.transform.position - transform.position;
				forceDirection.y = 0;
				forceDirection.Normalize();

				Effects.Damage(other.gameObject, Damage);
				Effects.ApplyForce(other.gameObject, forceDirection * HitForce);
				Effects.ApplyWeaponEffects(other.gameObject, effects);
			}
			isProjectile = false;
			GameManager.Instance.StillCheckList.Remove(holderAgent);
		}
	}
}
