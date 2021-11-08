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
		if (objectInHand)
		{
			//FOV.ViewRadius = objectInHand.ViewDistance;
			//FOV.ViewAngle = objectInHand.ViewAngle;
			Equip();
			//SetAimGradient();
			//if (throwAim != null)
			//{
			//	throwAim.gameObject.SetActive(true);
			//	throwAim.GetComponentInChildren<LineRenderer>().colorGradient = aimGradient;
			//	throwAim.gameObject.SetActive(false);
			//}
		}
	}

	public void Equip()
	{
		objectInHand.PickUpIn(gameObject);
		
		//foreach (Collider collider in objectInHand.GetComponentsInChildren<Collider>())
		//{
		//	collider.enabled = false;
		//}

		SetAimGradient();

		//objectInHand.SetAimGradient(aimGradient);
		//objectInHand.SetFOV(FOV);
		objectInHand.SetAim(FOV, FOVVisualization, throwAim.gameObject, aimGradient);
		

		//FOV.ViewAngle = objectInHand.ViewAngle;
		//FOV.ViewRadius = objectInHand.ViewDistance;
	}

	//Aim
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

		//objectInHand.SetAimGradient(aimGradient);
	}
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

	//Animation events
	public void DoSpecialAction()
	{
		objectInHand.DoSpecialAction(FOV);
	}
	public void DoPassiveSpecial()
	{
		objectInHand.DoPassiveSpecial();
	}
}
