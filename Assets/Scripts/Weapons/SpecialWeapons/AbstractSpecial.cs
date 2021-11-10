using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Abstract class controlling all special abillitys
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15/10-27
public abstract class AbstractSpecial : MonoBehaviour
{
	[SerializeField] protected List<WeaponEffects> effects;

	protected GameObject holderAgent;
	protected SpecialHand specialController;

	[SerializeField]
	private float damage;
	[SerializeField]
	private float hitForce;

	[SerializeField]
	private int maxCharges;
	private int charges;


	protected float Damage { get { return damage; } }
	protected float HitForce { get { return hitForce; } }
	protected int MaxCharges { get { return maxCharges; } }
	protected int Charges { get { return charges; } set { charges = value; } }
	


	public void PickUpIn(GameObject agent)
	{
		holderAgent = agent;
		specialController = agent.GetComponent<SpecialHand>();
	}
	public virtual void SetAimColor(Gradient gradient) { }
	public virtual void SetFOVSize() { }

	public abstract void ToggleAim(bool isActive);
	public abstract void StartAttack();
	public abstract void Attack();
	public abstract void DoSpecialAction();

	public virtual void TakeDamageEffect() { }
	public virtual void AddCharge()
	{
		if(charges < maxCharges)
		{
			charges++;
		}
	}
}
