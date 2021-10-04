using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Code by: Johan Melkersson
/// </summary>
public class WeaponHand : MonoBehaviour
{
	private Actions actions;
	private Animator animator;

	[SerializeField]
	private GameObject handObject;
	[SerializeField]
	private FieldOfView FOV;
	[SerializeField]
	private GameObject FOVVisualization;
	[SerializeField]
	private int HandHitdamage = 5;
	private float handHitDistance = 1.5f;
	private float handHitAngle = 100f;

	public AbstractWeapon objectInHand;
	[SerializeField]
	private Gradient laserSightGradient;
	
	private float throwForce;

	

	private void Awake()
	{
		actions = GetComponent<Actions>();
		animator = GetComponent<Animator>();
		FOV.viewRadius = handHitDistance;
		FOV.viewAngle = handHitAngle;
	}

	public void Equip(GameObject newObject)
	{
		newObject.GetComponent<AbstractWeapon>().PickUpIn(handObject);
		objectInHand = newObject.GetComponent<AbstractWeapon>();
		objectInHand.GetComponentInChildren<Collider>().enabled = false;
		if(objectInHand is RangedWeapon)
		{
			objectInHand.ToggleLaserAim(true, laserSightGradient);
		}
		else
		{
			FOV.viewAngle = objectInHand.ViewAngle;
			FOV.viewRadius = objectInHand.ViewDistance;
		}
	}

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

	public bool StartThrow()
	{
		if (objectInHand != null)
		{
			animator.SetTrigger("isAimThrow");
			return true;
		}
		return false;
	}
	public bool Throw(float throwForce)
	{
		if (objectInHand != null)
		{
			this.throwForce = throwForce;
			animator.SetTrigger("isThrow");
			return true;
		}
		return false;
	}
	public void ToggleAimView(bool isActive)
	{
		if (objectInHand != null && objectInHand is RangedWeapon)
		{
			//TODO
			objectInHand.ToggleLaserAim(isActive, laserSightGradient);
		}
		else
		{
			FOVVisualization.SetActive(isActive);
		}
	}

	public void ReleaseThrow()
	{
		if (objectInHand != null)
		{
			objectInHand.GetComponentInChildren<Collider>().enabled = true;
			objectInHand.ReleaseThrow(throwForce);

			throwForce = 0;
			objectInHand = null;
			FOV.viewAngle = handHitAngle;
			FOV.viewRadius = handHitDistance;
		}
	}

	public void DoDamage()
	{
		if (objectInHand != null)
		{
			if(objectInHand is RangedWeapon)
			{
				objectInHand.Shoot();
			}
			else
			{
				//actions.Attack(objectInHand);
				actions.Hit(objectInHand.Damage, objectInHand.transform.position);
			}
		}
		else
		{
			actions.Hit(HandHitdamage, transform.position);
		}
	}
}
