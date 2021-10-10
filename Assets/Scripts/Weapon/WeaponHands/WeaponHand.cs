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
	private ThrowAim throwAim;
	private GameObject laserAim;

	[SerializeField]
	private GameObject handObject;
	[SerializeField]
	private FieldOfView FOV;
	[SerializeField]
	private GameObject FOVVisualization;
	[SerializeField]
	private int handHitDamage = 5;
	[SerializeField]
	private int handHitForce = 5;
	[SerializeField]
	private float handHitDistance = 1.5f;
	[SerializeField]
	private float handHitAngle = 100f;

	public AbstractWeapon objectInHand;
	[SerializeField]
	private Gradient laserSightGradient;
	
	private float throwForce;

	public ThrowAim ThrowAim
	{
		get { return throwAim; }
		set { throwAim = value; }
	}



	private void Awake()
	{
		actions = GetComponent<Actions>();
		animator = GetComponent<Animator>();
		FOV.viewRadius = handHitDistance;
		FOV.viewAngle = handHitAngle;
	}

    private void Start()
    {
		if (objectInHand != null)
		{
			FOV.viewRadius = objectInHand.ViewDistance;
			FOV.viewAngle = objectInHand.ViewAngle;
		}
		else
		{
			FOV.viewRadius = handHitDistance;
			FOV.viewAngle = handHitAngle;
		}
	}

    public void Equip(GameObject newObject)
	{
		newObject.GetComponent<AbstractWeapon>().PickUpIn(handObject);
		objectInHand = newObject.GetComponent<AbstractWeapon>();
		foreach (Collider collider in objectInHand.GetComponentsInChildren<Collider>())
		{
			collider.enabled = false;
		}
		//objectInHand.GetComponentInChildren<Collider>().enabled = false;

		//For test
		//if (objectInHand is RangedWeapon || objectInHand is BombardWeapon)
		//{
		//	ToggleAimView(true);
		//}

		FOV.viewAngle = objectInHand.ViewAngle;
		FOV.viewRadius = objectInHand.ViewDistance;
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
			Debug.Log("HandHit" + handHitDamage);
		}
	}
	public bool StartBombard()
	{
		if (objectInHand != null && objectInHand is BombardWeapon)
		{
			objectInHand.StartAttack(animator);
			return true;
		}
		return false;
	}
	public bool SetBombardForce(float bombardForce)
	{
		if(objectInHand != null && objectInHand is BombardWeapon)
		{
			throwAim.initialVelocity = bombardForce;
			return true;
		}
		return false;
	}
	public bool PerformBombard(float bombardForce)
	{
		if (objectInHand != null && objectInHand is BombardWeapon)
		{
			throwAim.initialVelocity = bombardForce;
			objectInHand.Attack(animator);
			return true;
		}
		return false;
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
		if (objectInHand != null)
		{
			if(objectInHand is RangedWeapon)
			{
				objectInHand.ToggleAim(isActive, laserSightGradient);
			}
			else if(objectInHand is BombardWeapon)
			{
				throwAim.gameObject.SetActive(isActive);
				if (isActive)
				{
					GetComponentInChildren<LineRenderer>().colorGradient = laserSightGradient;
				}
			}
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
			objectInHand.ReleaseThrow(throwForce);
			foreach(Collider collider in objectInHand.GetComponentsInChildren<Collider>())
			{
				collider.enabled = true;
			}
			//objectInHand.GetComponentInChildren<Collider>().enabled = true;

			//For test
			if (objectInHand is RangedWeapon)
			{
				objectInHand.ToggleAim(false, laserSightGradient);
			}

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
			if(objectInHand is RangedWeapon || objectInHand is BombardWeapon)
			{
				objectInHand.ReleaseProjectile();
			}
			else if(objectInHand is MeleeWeapon)
			{
				actions.Hit(objectInHand.transform.position, objectInHand.Damage, objectInHand.HitForce);
			}
		}
		else
		{
			actions.Hit(handObject.transform.position, handHitDamage, handHitForce);
		}
	}
}
