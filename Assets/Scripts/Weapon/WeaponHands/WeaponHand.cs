using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Control characters weapon hand
/// </para>
///   
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15/10-21
public class WeaponHand : MonoBehaviour
{
	private Actions actions;
	private Animator animator;
	[SerializeField]
	private ThrowAim throwAim;

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

	private Gradient aimGradient;
	
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

		SetAimGradient();

		if (throwAim != null)
		{
			throwAim.gameObject.SetActive(true);
			throwAim.GetComponentInChildren<LineRenderer>().colorGradient = aimGradient;
			throwAim.gameObject.SetActive(false);
		}
	}

	//Aim
	private void SetAimGradient()
	{
		aimGradient = new Gradient();
		GradientColorKey[] colorKey = new GradientColorKey[2];
		colorKey[0].color = GetComponent<Attributes>().PlayerColor;
		GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];
		alphaKey[0].alpha = 1;
		alphaKey[1].time = 1;
		alphaKey[1].alpha = 0;
		aimGradient.SetKeys(colorKey, alphaKey);
	}
	public void ToggleAimView(bool isActive)
	{
		if (objectInHand)
		{
			objectInHand.ToggleAim(isActive, FOVVisualization, throwAim.gameObject);
		}
		else
		{
			FOVVisualization.SetActive(isActive);
		}
	}
	//public void ToggleThrowAim()
	//{
	//	if (objectInHand)
	//	{
	//		//objectInHand.ToggleThrowAim(isActive);
	//	}
	//}

	//Pick up
	public void Equip(GameObject newObject)
	{
		newObject.GetComponent<AbstractWeapon>().PickUpIn(handObject);
		objectInHand = newObject.GetComponent<AbstractWeapon>();
		foreach (Collider collider in objectInHand.GetComponentsInChildren<Collider>())
		{
			collider.enabled = false;
		}
		objectInHand.SetAimGradient(aimGradient);

		FOV.viewAngle = objectInHand.ViewAngle;
		FOV.viewRadius = objectInHand.ViewDistance;
	}

	//Attack
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
		}
	}

	//Bombard attack
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
	public bool PerformBombard()
	{
		if (objectInHand != null && objectInHand is BombardWeapon)
		{
			objectInHand.Attack(animator);
			return true;
		}
		return false;
	}

	//Throw weapon
	public bool StartThrow()
	{
		if (objectInHand != null)
		{
			animator.SetTrigger("isAimThrow");
			return true;
		}
		return false;
	}
	public bool SetThrowForce(float throwForce)
	{
		if (objectInHand != null)
		{
			this.throwForce = throwForce;
			return true;
		}
		return false;
	}
	public bool Throw()
	{
		if (objectInHand != null)
		{
			animator.SetTrigger("isThrow");
			return true;
		}
		return false;
	}

	public void CancelAction()
	{
		animator.SetTrigger("isCancelAction");
	}

	//Animation events
	public void DoAction()
	{
		if (objectInHand)
		{
			objectInHand.DoAction(FOV);
		}
		else if (FOV.visibleTargets.Count > 0)
		{
			foreach (GameObject target in FOV.visibleTargets)
			{
				Effects.Damage(target, handHitDamage);
				Effects.ApplyForce(target, (target.transform.position - FOV.transform.position).normalized * handHitForce);
			}
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

			throwForce = 0;
			objectInHand = null;
			FOV.viewAngle = handHitAngle;
			FOV.viewRadius = handHitDistance;			
		}
	}
}
