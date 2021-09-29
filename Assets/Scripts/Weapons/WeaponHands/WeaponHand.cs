using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Code by: Johan Melkersson
/// </summary>
public class WeaponHand : MonoBehaviour
{
	[SerializeField]
	private GameObject handObject;
	private AbstractWeapon objectInHand;
	private FieldOfView fov;
	[SerializeField]
	private GameObject FovView;
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
	public void StartAttack()
	{
		if (objectInHand != null)
		{
			objectInHand.StartAttack(animator);
		}
		else
		{
			animator.SetTrigger("isStartHandAttack");
		}
	}
	public void Attack()
	{
		if (objectInHand != null)
		{
			objectInHand.Attack(animator);
		}
		else
		{
			animator.SetTrigger("isHandAttack");
			Debug.Log("HandHit" + HandHitdamage);
		}
	}
	public void CancelAction()
	{
		animator.SetTrigger("isCancelAction");
	}

	public void StartThrow()
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

	public void ToggleAimView(bool isActive)
	{
		if (objectInHand != null && objectInHand is RangedWeapon)
		{
			//TODO
		}
		else
		{
			FovView.SetActive(isActive);
		}
	}

	public void ReleaseThrow()
	{
		if (objectInHand != null)
		{
			objectInHand.ReleaseThrow(throwForce);

			throwForce = 0;
			objectInHand = null;
			fov.viewAngle = handHitAngle;
			fov.viewRadius = handHitDistance;

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
