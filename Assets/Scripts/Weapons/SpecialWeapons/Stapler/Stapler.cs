using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stapler : AbstractSpecial
{
	[SerializeField]
	private GameObject weaponMuzzle;
	[SerializeField]
	private GameObject laserAim;
	[SerializeField]
	private float bulletFireForce = 20;
	[SerializeField]
	protected GameObject bullet;

	[Tooltip("The maximum amount of degrees away from the direction aimed that the projectile might fly")]
	[SerializeField]
	protected float inaccuracy = 3;

	[SerializeField]
	protected GameObject AimCone;

	/// <summary>
	/// The degrees by which the shot might change direction to either side. The effect of poison is included here
	/// </summary>
	public float Inaccuracy
	{
		get
		{
			float modval = 0;
			if (this.holderAgent != null)
			{
				modval = this.holderAgent.GetComponent<StatusEffectHandler>().InAccuracyMod;
			}
			return Mathf.Clamp(inaccuracy + modval, 0, 89);
		}
		set { inaccuracy = Mathf.Clamp(value, 0, 89); }
	}

	public override void SetAimColor(Gradient gradient)
	{
		laserAim.SetActive(true);
		GetComponentInChildren<LineRenderer>().colorGradient = gradient;
		laserAim.SetActive(false);
	}

	public override void ToggleAim(bool isActive)
	{
		laserAim.SetActive(isActive);

		UpdateAimCone();
		AimCone.SetActive(isActive);
	}
	/// <summary>
	/// The maximum amount of degrees from the aim direction that the shot can deviate. This takes the possibility of being po�soned into account.<br/>
	/// Also updates the size and such of the aimcone.
	/// </summary>
	protected void UpdateAimCone()
	{
		float width = 2 * Mathf.Tan(Inaccuracy * Mathf.Deg2Rad);//the 1,1,1 scale of the cone has length one and width one.
		AimCone.transform.localScale = new Vector3(width, 1, 1);
	}

	public override void StartAttack()
	{
		specialController.Animator.SetTrigger("isStartSpecialStapler");
	}
	public override void Attack()
	{
		specialController.Animator.SetTrigger("isSpecialStapler");
	}

	public override void StartTurnEffect()
	{
		base.AddCharge();
	}

	public override void DoSpecialAction()
	{
		throw new System.NotImplementedException();
	}
}
