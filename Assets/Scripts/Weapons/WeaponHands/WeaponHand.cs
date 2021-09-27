using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHand : MonoBehaviour
{
	[SerializeField]
	private GameObject handObject;
	private AbstractWeapon objectInHand;
	private FieldOfView fov;
	[SerializeField]
	private float handHitDistance = 1.5f;
	private float handHitAngle = 100f;

	[SerializeField]
	private int HandHitdamage = 5;

	private Actions actions;

    public float throwForce;

	private Animator animator;

	private void Awake()
	{
		actions = GetComponent<Actions>();
		animator = GetComponent<Animator>();
		fov = GetComponent<FieldOfView>();
		fov.viewRadius = handHitDistance;
		fov.viewAngle = handHitAngle;
	}

	public void Equip(GameObject newObject)
	{
		newObject.GetComponent<AbstractWeapon>().PickUpIn(handObject);
		objectInHand = newObject.GetComponent<AbstractWeapon>();
		fov.viewAngle = objectInHand.ViewAngle;
		fov.viewRadius = objectInHand.ViewDistance;
		
	}
	//TODO
	//public void StartAttack()
	//{
	//	if (objectInHand != null)
	//	{
	//		objectInHand.StartAttack(animator);
	//	}
	//	else
	//	{
	//		Debug.Log("HandHit" + damage);
	//	}
	//}
	public void Attack()
	{
		if (objectInHand != null)
		{
			objectInHand.Attack(animator);
		}
		else
		{
			Debug.Log("HandHit" + HandHitdamage);
		}
	}

	public void AimThrow()
	{
		if (objectInHand != null)
		{
			animator.SetTrigger("isAimThrow");
		}
	}
	public void Throw(float throwForce)
	{
		if (objectInHand != null)
		{
			this.throwForce = throwForce;
			animator.SetTrigger("isThrow");
		}
	}
	public void ReleaseThrow()
	{
		if (objectInHand != null)
		{
			objectInHand.ReleaseThrow(throwForce);

			throwForce = 0;
			objectInHand = null;
		}
	}

	public void DoDamage()
	{
		if (objectInHand != null)
		{
			actions.Attack(objectInHand);
		}
		else
		{
			actions.Hit(HandHitdamage);
		}
	}
}
