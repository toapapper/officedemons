using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialHand : MonoBehaviour
{
	private Animator animator;
	[SerializeField]
	private ThrowAim throwAim;
	[SerializeField]
	private FieldOfView FOV;
	[SerializeField]
	private GameObject FOVVisualization;

	public AbstractSpecial objectInHand;

	private Gradient aimGradient;

	public ThrowAim ThrowAim
	{
		get { return throwAim; }
		set { throwAim = value; }
	}

	private void Awake()
	{
		animator = GetComponent<Animator>();
		objectInHand = GetComponentInChildren<AbstractSpecial>();
	}

	private void Start()
	{
		SetAimGradient();
		if (objectInHand != null)
		{
			Equip();
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
		throwAim.gameObject.SetActive(true);
		GetComponentInChildren<LineRenderer>().colorGradient = aimGradient;
		throwAim.gameObject.SetActive(false);
	}

	public void Equip()
	{
		objectInHand.PickUpIn(gameObject);
		objectInHand.SetAim(FOV, aimGradient);
	}

	//Aim
	public void ToggleAimView(bool isActive)
	{
		if (objectInHand)
		{
			objectInHand.ToggleAim(isActive, FOVVisualization, throwAim.gameObject);
		}
	}

	//Attack
	public bool StartAttack()
	{
		if (objectInHand)
		{
			objectInHand.StartAttack(animator);
			return true;
		}
		return false;
	}
	public bool Attack()
	{
		if (objectInHand)
		{
			objectInHand.Attack(animator);
			return true;
		}
		return false;
	}
	public bool SetBombardForce(float bombardForce)
	{
		if (objectInHand && objectInHand is CoffeeSpecial)
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

	//TODO Start passive abillity 
	public void PerformPassiveAbility()
	{
		objectInHand.PerformPassiveAbility(animator);
	}

	//Animation events
	public void DoSpecialAction()
	{
		objectInHand.DoSpecialAction(FOV);
	}
}
