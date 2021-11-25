using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple enum of possible special effects that a weapon can have
/// </summary>
public enum WeaponEffects
{
	Nothing,
	Fire,			//applies a stack of fire to the target				duration:	Medium (short = 1, medium = 3, long = 5(defined in StatusEffectHandler.cs))
	Bleed,			//applies a stack of bleed to the target			duration:	Medium
	Poison,			//applies a stack of posion to the target			duration:	Long
	StaminaDrain,	//applies a stack of stamina drain on the target	duration:	Long
	Vulnerable,		//applies a stack of vulnerability to the target	duration:	Short
	Paralyze,		//paralyzes target on hit							duration:	Short
	Slow,			//slows the target on hit							duration:	Medium
	Disarm,			//chance of disarming target on hit
	Slippery,		//risk of dropping the weapon on attack
	Recoil			//risk of hitting youself on attack
}

/// <summary>
/// <para>
/// Abstract class controlling all weapons
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15/10-29
public abstract class AbstractWeapon : MonoBehaviour
{
	public const float RecoilChance = .3f;
	public const float SlipperyDropChance = .3f;
	public const float DisarmChance = .3f;

    [SerializeField]
    private Sprite weaponTexture;

    public Sprite WeaponTexture
    {
        get { return weaponTexture; }
    }


    [SerializeField] protected List<WeaponEffects> effects;

	[SerializeField]
	protected GameObject holderAgent;
	[SerializeField]
	private GameObject handle;
	[SerializeField]
	private float damage = 10;
	[SerializeField]
	private float hitForce = 2;
	[SerializeField]
	private float throwDamage = 2;
	[SerializeField]
	private float viewDistance = 20f;
	[SerializeField]
	private float viewAngle = 10f;
	[SerializeField]
	private int durability = 3;
	[SerializeField]
	private float weight = 5;

	[SerializeField]
	private bool isHeld;
	private bool isProjectile;

	protected GameObject Handle
	{
		get { return handle; }
		set { handle = value; }
	}
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
	protected float ThrowDamage
	{
		get { return throwDamage; }
		set { throwDamage = value; }
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
	public bool IsHeld
	{
		get { return isHeld; }
		set { isHeld = value; }
	}
	public int Durability
	{
		get { return durability; }
		set { durability = value; }
	}

	public virtual void PickUpIn(GameObject hand)
	{
		holderAgent = hand.transform.parent.parent.gameObject;
		isHeld = true;
		handle.GetComponent<Rigidbody>().isKinematic = true;
		GetComponent<Rigidbody>().isKinematic = true;

		handle.transform.parent = hand.transform;
		handle.transform.position = hand.transform.position;
		handle.transform.rotation = hand.transform.rotation;
		Effects.ChangeWeight(hand.transform.parent.gameObject, weight);
		foreach (Collider collider in GetComponentsInChildren<Collider>())
		{
			collider.enabled = false;
		}
	}
	public void ReleaseThrow(float force)
	{
		Drop();
		GetComponentInChildren<Rigidbody>().AddForce(transform.up * force, ForceMode.VelocityChange);
		isProjectile = true;
	}
	public void Drop()
	{
		Effects.ChangeWeight(handle.transform.parent.parent.gameObject, -weight);
		handle.transform.parent = null;
		handle.GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<Rigidbody>().isKinematic = false;
		isHeld = false;
		foreach (Collider collider in GetComponentsInChildren<Collider>())
		{
			collider.enabled = true;
		}
		holderAgent = null;
	}

	public virtual void SetAimGradient(Gradient gradient) { }
	public virtual void ToggleAim(bool isActive, GameObject FOVView, GameObject throwAim) { }
	public virtual void StartAttack(Animator animator) { }
	public virtual void Attack(Animator animator) 
	{
		if(GameManager.Instance.CurrentCombatState == CombatState.playerActions)
		{
			durability -= 1;
		}
	}

	/// <summary>
	/// Method triggered by animation, Shoots projectile for ranged or deals damage for melee
	/// </summary>
	/// <param name="fov"></param>
	public virtual void DoAction(FieldOfView fov)
	{
		if (durability <= 0)
		{
			handle.GetComponentInParent<PlayerInputHandler>().RemoveObjectFromWeaponList(this.gameObject);
			handle.GetComponentInParent<WeaponHand>().DropWeapon();
			Destroy(this.gameObject);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (isProjectile)
		{
			if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
			{
				Effects.RegularDamage(collision.gameObject, throwDamage, holderAgent);
				//Effects.Damage(collision.gameObject, throwDamage);
				Effects.ApplyWeaponEffects(collision.gameObject, effects);
			}
			isProjectile = false;
		}
	}


}
