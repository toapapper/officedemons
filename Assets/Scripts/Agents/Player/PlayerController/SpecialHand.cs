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

	private AbstractSpecial objectInHand;

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
		FOV.ViewRadius = objectInHand.ViewDistance;
		FOV.ViewAngle = objectInHand.ViewAngle;

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
		objectInHand.SetAimGradient(aimGradient);
	}
	public void ToggleAimView(bool isActive)
	{
		objectInHand.ToggleAim(isActive, FOVVisualization, throwAim.gameObject);
	}

	//Attack
	public void StartAttack()
	{
		objectInHand.StartAttack(animator);
	}
	public void Attack()
	{
		objectInHand.Attack(animator);
	}

	//Bombard attack
	public bool StartBombard()
	{
		if (objectInHand is BombardWeapon)
		{
			objectInHand.StartAttack(animator);
			return true;
		}
		return false;
	}
	public bool SetBombardForce(float bombardForce)
	{
		if (objectInHand is BombardWeapon)
		{
			throwAim.initialVelocity = bombardForce;
			return true;
		}
		return false;
	}
	public bool PerformBombard()
	{
		if (objectInHand is BombardWeapon)
		{
			objectInHand.Attack(animator);
			return true;
		}
		return false;
	}

	public void CancelAction()
	{
		animator.SetTrigger("isCancelAction");
	}

	//Animation events
	public void DoSpecialAction()
	{
		objectInHand.DoAction(FOV);
	}
}
