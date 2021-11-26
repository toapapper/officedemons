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

// Last Edited: 15/10-29
public class WeaponHand : MonoBehaviour
{
	public const float SlipperyDropChance = .3f;

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
		animator = GetComponent<Animator>();
		objectInHand = GetComponentInChildren<AbstractWeapon>();
	}

    private void Start()
    {
		SetAimGradient();

		if (objectInHand != null)
		{
			Equip(objectInHand.gameObject);
		}
		else
		{
			FOV.ViewRadius = handHitDistance;
			FOV.ViewAngle = handHitAngle;
		}
    }

    private void SetAimGradient()
	{
		GradientColorKey[] colorKey = new GradientColorKey[2];
		colorKey[0].color = GetComponent<Attributes>().PlayerColor;
		GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];
		alphaKey[0].alpha = 1;
		alphaKey[1].time = 1;
		alphaKey[1].alpha = 0.5f;
		aimGradient = new Gradient();
		aimGradient.SetKeys(colorKey, alphaKey);

		FOVVisualization.GetComponent<Renderer>().material.color = aimGradient.colorKeys[0].color;
		if (throwAim != null)
		{
			throwAim.gameObject.SetActive(true);
			throwAim.GetComponentInChildren<LineRenderer>().colorGradient = aimGradient;
			throwAim.DeActivate();
		}
	}

	public void ToggleAimView(bool isActive)
	{
		if (objectInHand != null)
		{
			objectInHand.ToggleAim(isActive, FOVVisualization/*, throwAim.gameObject*/);
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
	public void Equip(GameObject newObject)
	{
		objectInHand = newObject.GetComponent<AbstractWeapon>();
		objectInHand.PickUpIn(handObject);
		
		objectInHand.SetAimGradient(aimGradient);
		FOV.ViewAngle = objectInHand.ViewAngle;
		FOV.ViewRadius = objectInHand.ViewDistance;
	}
	//Pick up
	//public void Equip(GameObject newObject)
	//{
	//	objectInHand = newObject.GetComponent<AbstractWeapon>();
	//	objectInHand.PickUpIn(handObject);
	//	foreach (Collider collider in objectInHand.GetComponentsInChildren<Collider>())
	//	{
	//		collider.enabled = false;
	//	}
	//	objectInHand.SetAimGradient(aimGradient);

	//	FOV.ViewAngle = objectInHand.ViewAngle;
	//	FOV.ViewRadius = objectInHand.ViewDistance;
	//}

	//Unequip weapon
	public void DropWeapon()
    {
		if(objectInHand == null)
        {
			return;
        }

		objectInHand.Drop();
		//foreach (Collider collider in objectInHand.GetComponentsInChildren<Collider>())
		//{
		//	collider.enabled = true;
		//}
		objectInHand = null;
		FOV.ViewRadius = handHitDistance;
		FOV.ViewAngle = handHitAngle;
	}

	//Attack
	public void StartAttack()
	{
		if (objectInHand)
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
		if (objectInHand)
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
			throwAim.initialSpeed = bombardForce;
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
		else if (FOV.VisibleTargets.Count > 0)
		{
			foreach (GameObject target in FOV.VisibleTargets)
			{
				Effects.RegularDamage(target, handHitDamage, gameObject);
				Effects.ApplyForce(target, (target.transform.position - FOV.transform.position).normalized * handHitForce);

				float rand = Random.value;
				if (rand < SlipperyDropChance)
				{
					Effects.Disarm(target);
				}
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
			FOV.ViewAngle = handHitAngle;
			FOV.ViewRadius = handHitDistance;
		}
	}
}
