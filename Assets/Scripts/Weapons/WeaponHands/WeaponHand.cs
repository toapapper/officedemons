using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHand : MonoBehaviour
{
	[SerializeField]
	private GameObject handObject;
	private AbstractWeapon objectInHand;

	[SerializeField]
	private int damage = 5;

	private Actions actions;

    public float throwForce;

	private Animator animator;

	private void Awake()
	{
		actions = GetComponent<Actions>();
		animator = GetComponent<Animator>();
	}

	public void Equip(GameObject newObject)
	{
		newObject.GetComponent<AbstractWeapon>().PickUpIn(handObject);
		objectInHand = newObject.GetComponent<AbstractWeapon>();
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
			actions.Attack(objectInHand);
		}
		else
		{
			Debug.Log("HandHit" + damage);
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
}
