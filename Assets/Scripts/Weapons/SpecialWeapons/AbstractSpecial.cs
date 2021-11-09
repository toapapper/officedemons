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

	
	protected float Damage { get { return damage; } }
	protected float HitForce { get { return hitForce; } }


	public void PickUpIn(GameObject holderAgent)
	{
		this.holderAgent = holderAgent;
		specialController = holderAgent.GetComponent<SpecialHand>();
	}
	public virtual void SetAimColor(Gradient gradient) { }
	public virtual void SetFOVSize() { }

	public abstract void ToggleAim(bool isActive);
	public abstract void StartAttack();
	public abstract void Attack();
	public abstract void DoSpecialAction();
}
