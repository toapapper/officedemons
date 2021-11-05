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

	//[SerializeField]
	protected GameObject holderAgent;
	[SerializeField]
	private float damage;
	[SerializeField]
	private float hitForce;
	[SerializeField]
	private float viewDistance = 20f;
	[SerializeField]
	private float viewAngle = 10f;

	public float Damage
	{
		get { return damage; }
		set { damage = value; }
	}
	public float HitForce
	{
		get { return hitForce; }
		set { hitForce = value; }
	}
	public float ViewAngle
	{
		get { return viewAngle; }
		set { viewAngle = value; }
	}
	public float ViewDistance
	{
		get { return viewDistance; }
		set { viewDistance = value; }
	}

	public void PickUpIn(GameObject holderAgent)
	{
		this.holderAgent = holderAgent;
	}
	public virtual void SetAimGradient(Gradient gradient) { }
	public virtual void ToggleAim(bool isActive, GameObject FOVView, GameObject throwAim) { }
	public virtual void StartAttack(Animator animator) { }
	public abstract void Attack(Animator animator);
	public virtual void DoSpecialAction(FieldOfView fov) { }
	public virtual void DoPassiveSpecial() { }

	//public virtual void OnCollisionEnter(Collision collision) { }
}
