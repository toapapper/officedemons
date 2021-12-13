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
	private FieldOfView fov;
	[SerializeField]
	private GameObject fovVisualization;
	[SerializeField]
	private int handHitDamage = 10;
	[SerializeField]
	private int handHitForce = 5;
	[SerializeField]
	private float handHitDistance = 1.5f;
	[SerializeField]
	private float handHitAngle = 100f;

	public AbstractWeapon objectInHand;

	private Gradient aimGradient;

	private float throwForce;

	public FieldOfView FOV { get { return fov; } }
	public GameObject FOVVisualization { get { return fovVisualization; } }
	public ThrowAim ThrowAim { get { return throwAim; } set { throwAim = value; } }
	public Animator Animator { get { return animator; } }


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

		fovVisualization.GetComponent<Renderer>().material.color = aimGradient.colorKeys[0].color;
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
			objectInHand.ToggleAim(isActive/*, fovVisualization*/);
		}
		else
		{
			fovVisualization.SetActive(isActive);
		}
	}

	//Pick up
	public void Equip(GameObject newObject)
	{
		objectInHand = newObject.GetComponentInChildren<AbstractWeapon>(); /////
		objectInHand.PickUpIn(handObject);
		
		objectInHand.SetAimGradient(aimGradient);
		FOV.ViewAngle = objectInHand.ViewAngle;
		FOV.ViewRadius = objectInHand.ViewDistance;
	}

	//Unequip weapon
	public void DropWeapon()
    {
		if(objectInHand != null)
        {
			objectInHand.Drop();
			objectInHand = null;
			FOV.ViewRadius = handHitDistance;
			FOV.ViewAngle = handHitAngle;
		}
	}

	public void Disarm()
	{
		if (objectInHand != null)
		{
			float dirX = Random.value;
			float dirZ = Random.value;
			float force = Random.value * 10;
			Vector3 velocity = new Vector3(dirX, 1, dirZ).normalized * force;

			objectInHand.Disarm(velocity);
			objectInHand = null;
			FOV.ViewRadius = handHitDistance;
			FOV.ViewAngle = handHitAngle;
		}
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
			AkSoundEngine.PostEvent("Play_MeleeSwingsPack_96khz_Stereo_NormalSwings39", gameObject);
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
	//public bool SetBombardForce(float bombardForce)
	//{
	//	if(objectInHand != null && objectInHand is BombardWeapon)
	//	{
	//		throwAim.initialSpeed = bombardForce;
	//		return true;
	//	}
	//	return false;
	//}
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
		animator.SetTrigger("isAimThrow");
		return true;
	}
	public bool SetThrowForce(float throwForce)
	{
		this.throwForce = throwForce;
		return true;
	}
	public bool Throw()
	{
		animator.SetTrigger("isThrow");
		return true;
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
			objectInHand.DoAction(/*FOV*/);
		}
		else 
		{
			
			if (FOV.VisibleTargets.Count > 0)
			{
				foreach (GameObject target in FOV.VisibleTargets)
				{
					AkSoundEngine.PostEvent("Play_Blunt_thud", gameObject);
					Effects.RegularWeaponDamage(target, handHitDamage, gameObject);
					Effects.ApplyForce(target, (target.transform.position - FOV.transform.position).normalized * handHitForce);

					float rand = Random.value;
					if (rand < SlipperyDropChance)
					{
						Effects.Disarm(target);
					}
				}
			}
		}
	}
	public void ReleaseThrow()
	{
		if (objectInHand != null)
		{
			objectInHand.ReleaseThrow(throwForce);
			//foreach(Collider collider in objectInHand.GetComponentsInChildren<Collider>())
			//{
			//	collider.enabled = true;
			//}

			throwForce = 0;
			objectInHand = null;
			FOV.ViewAngle = handHitAngle;
			FOV.ViewRadius = handHitDistance;
		}
	}
}
