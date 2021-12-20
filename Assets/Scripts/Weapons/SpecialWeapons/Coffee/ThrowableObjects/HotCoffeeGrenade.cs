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
	[Header("SplitFuncionality")]
	[SerializeField] protected bool split = false;
	[SerializeField] protected GameObject splitObject;
	[SerializeField] protected float splitAngle = 75;//Degrees. Both left and right on collision
	[SerializeField] protected float splitHorizontalVelocity = 10;
	[SerializeField] protected float splitVerticalVelocity = 5;
 
	public void CreateGrenade(GameObject thrower, Vector3 position, Vector3 velocity,
		float explodeRadius, float grenadeExplodeForce, float grenadeDamage, List<StatusEffectType> effects)
	{
		HotCoffeeGrenade grenade;

		//Set rotation to only take y axis into account.
		Quaternion rotation = Quaternion.LookRotation(velocity.normalized);
		Vector3 euler = rotation.eulerAngles;
		euler.x = 0;
		euler.z = 0;
		rotation = Quaternion.Euler(euler);

		grenade = Instantiate(this, position, rotation);
		grenade.thrower = thrower;
		grenade.FOV.ViewRadius = explodeRadius;
		grenade.healthModifyAmount = grenadeDamage;
		grenade.explosionForce = grenadeExplodeForce;
		grenade.weaponEffects = effects;
		grenade.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.Impulse);

		GameManager.Instance.StillCheckList.Add(grenade.gameObject);
	}

	//Alternative implementation of CreateGrenade used when spawning splitGrenades
	public void CreateGrenadeVelocity(GameObject thrower, Vector3 position, Vector3 velocity,
		float explodeRadius, float grenadeExplodeForce, float grenadeDamage, List<StatusEffectType> effects)
	{
		HotCoffeeGrenade grenade;

		//Set rotation to only take y axis into account.
		Quaternion rotation = Quaternion.LookRotation(velocity.normalized);
		Vector3 euler = rotation.eulerAngles;
		euler.x = 0;
		euler.z = 0;
		rotation = Quaternion.Euler(euler);

		float offset = .3f;//distance it spawns away from position at in the direction of its velocity
		Vector3 spawnPos = velocity.normalized;
		spawnPos *= offset;
		spawnPos += position;

		grenade = Instantiate(this, spawnPos, rotation);
		grenade.thrower = thrower;
		grenade.FOV.ViewRadius = explodeRadius;
		grenade.healthModifyAmount = grenadeDamage;
		grenade.explosionForce = grenadeExplodeForce;
		grenade.weaponEffects = effects;
		grenade.GetComponent<Rigidbody>().velocity = velocity;

		GameManager.Instance.StillCheckList.Add(grenade.gameObject);
	}

	protected override void CreateGroundObject(Vector3 groundObjectPos)
	{
		Debug.Log("Create stain at: " + groundObjectPos);
		Vector3 rot = transform.rotation.eulerAngles;
		rot.x = 0;
		rot.z = 0;
		Instantiate(stain, groundObjectPos, Quaternion.Euler(rot));
	}

	protected override void ImpactAgents() {/*This should do nothing. its a useless method.*/}

    protected override void Explode()
    {
		GameObject expl = Instantiate(explosion, transform.position, transform.rotation);
		Explosion explos = expl.GetComponent<Explosion>();
		if(explos != null)
        {
			explos.SpawnerAgent = thrower;
        }

		DestroyGrenade();
    }

	protected void Split()
    {
		if(split == false || splitObject == null)
        {
			return;
        }

		SpawnSplitGrenade(splitAngle);
		SpawnSplitGrenade(-splitAngle);
    }

	protected void SpawnSplitGrenade(float angle)
    {
		Vector3 directionVector = transform.forward;
		float x = directionVector.x * Mathf.Cos(angle * Mathf.Deg2Rad) - directionVector.z * Mathf.Sin(angle * Mathf.Deg2Rad);
		float z = directionVector.x * Mathf.Sin(angle * Mathf.Deg2Rad) + directionVector.z * Mathf.Cos(angle * Mathf.Deg2Rad);
		directionVector.x = x;
		directionVector.z = z;
		directionVector *= splitHorizontalVelocity;
		directionVector.y = splitVerticalVelocity;

		splitObject.GetComponent<HotCoffeeGrenade>().CreateGrenadeVelocity(thrower, transform.position, directionVector, FOV.ViewRadius, healthModifyAmount, explosionForce, weaponEffects);
	}

	protected override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);
		AkSoundEngine.PostEvent("Play_Splash", gameObject);


		Split();
		Explode();
	}
}
