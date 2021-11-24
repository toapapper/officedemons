using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialHand : MonoBehaviour
{
	private Animator animator;
	[SerializeField]
	private ThrowAim throwAim;
	[SerializeField]
	private FieldOfView fov;
	[SerializeField]
	private GameObject fovVisualization;

	[SerializeField]
	private AbstractSpecial objectInHand;
	private Gradient aimGradient;

	public AbstractSpecial ObjectInHand { get { return objectInHand; } }
	public ThrowAim ThrowAim { get { return throwAim; } }
	public FieldOfView FOV { get { return fov; } }
	public GameObject FOVVisualization { get { return fovVisualization; } }
	public Animator Animator { get { return animator; } }



	private void Awake()
	{
		animator = GetComponent<Animator>();
		objectInHand = GetComponentInChildren<AbstractSpecial>();
	}

	private void Start()
	{
		SetColorGradient();
		if (objectInHand != null)
		{
			Equip();
		}
		SetAimColor();
	}

	private void SetColorGradient()
	{
		GradientColorKey[] colorKey = new GradientColorKey[2];
		colorKey[0].color = GetComponent<Attributes>().PlayerColor;
		GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];
		alphaKey[0].alpha = 1;
		alphaKey[1].time = 1;
		alphaKey[1].alpha = 0.5f;
		aimGradient = new Gradient();
		aimGradient.SetKeys(colorKey, alphaKey);
	}
	private void SetAimColor()
	{
		fovVisualization.GetComponent<Renderer>().material.color = aimGradient.colorKeys[0].color;
		throwAim.gameObject.SetActive(true);
		GetComponentInChildren<LineRenderer>().colorGradient = aimGradient;
		throwAim.DeActivate();
		if (objectInHand)
		{
			objectInHand.SetAimColor(aimGradient);
		}
	}

	public void Equip()
	{
		objectInHand.PickUpIn(gameObject);
		//SetAimColor();
		objectInHand.SetFOVSize();

		//objectInHand.SetAim(fov, aimGradient);
	}

	//Aim
	public void ToggleAimView(bool isActive)
	{
		if (objectInHand)
		{
			objectInHand.ToggleAim(isActive/*, fovVisualization, throwAim.gameObject*/);
		}
	}

	//Attack
	public bool StartAttack()
	{
		if (objectInHand)
		{
			objectInHand.StartAttack(/*animator*/);
			return true;
		}
		return false;
	}
	public bool Attack()
	{
		if (objectInHand)
		{
			objectInHand.Attack(/*animator*/);
			return true;
		}
		return false;
	}
	public bool SetBombardForce(float bombardForce)
	{
		if (objectInHand && objectInHand is CoffeeCupSpecial)
		{
			throwAim.initialVelocity = bombardForce;
			return true;
		}
		return false;
	}
	public void CancelAction()
	{
		animator.SetTrigger("isCancelAction");
	}

	public void StartTurnEffect()
	{
		objectInHand.StartTurnEffect();
	}
	public void TakeDamageEffect()
	{
		objectInHand.TakeDamageEffect();
	}
	public void GiveRegularDamageEffect()
	{
		objectInHand.GiveRegularDamageEffect();
	}
	public void KillEffect()
	{
		objectInHand.KillEffect();
	}
	public void RevivedEffect()
	{
		objectInHand.RevivedEffect();
	}
	//public void AddCharge()
	//{
	//	objectInHand.AddCharge();
	//}

	//Animation events
	public void DoSpecialAction()
	{
		objectInHand.DoSpecialAction(/*fov*/);
	}
	public void DoSpecialActionEnd()
	{
		objectInHand.DoSpecialActionEnd();
	}
}
