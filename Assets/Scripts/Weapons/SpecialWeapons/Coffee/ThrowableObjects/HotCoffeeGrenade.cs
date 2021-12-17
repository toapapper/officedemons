using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// The harmful grenade version thrown by Devins special weapon,
/// doing damage and creating debuffing coffestain on impact
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15-11-18
public class HotCoffeeGrenade : GroundEffectGrenade
{
	[Header("Hot coffee specifics")]
	[SerializeField] private GameObject stain;
	[SerializeField] protected GameObject explosion;
	 
	public void CreateGrenade(GameObject thrower, Vector3 position, Vector3 velocity,
		float explodeRadius, float grenadeExplodeForce, float grenadeDamage, List<StatusEffectType> effects)
	{
		HotCoffeeGrenade grenade;
		
		grenade = Instantiate(this, position, Quaternion.LookRotation(velocity.normalized));
		grenade.thrower = thrower;
		grenade.FOV.ViewRadius = explodeRadius;
		grenade.healthModifyAmount = grenadeDamage;
		grenade.explosionForce = grenadeExplodeForce;
		grenade.weaponEffects = effects;
		grenade.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.Impulse);

		GameManager.Instance.StillCheckList.Add(grenade.gameObject);
	}

	protected override void CreateGroundObject(Vector3 groundObjectPos)
	{
		Debug.Log("Create stain at: " + groundObjectPos);
		Instantiate(stain, groundObjectPos, transform.rotation);
	}

	protected override void ImpactAgents() {/*This should do nothing. its a useless method.*/}

    protected override void Explode()
    {
		Instantiate(explosion, transform.position, transform.rotation);
		DestroyGrenade();
    }

    protected override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);
		AkSoundEngine.PostEvent("Play_Splash", gameObject);
		Explode();
	}
}
