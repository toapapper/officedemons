using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// <para>
/// Static effects that can be applied to characters
/// </para>
///   
///  <para>
///  Author: Johan Melkersson & Jonas Lundin
/// </para>
/// </summary>

// Last Edited: 14/10-28
public static class Effects
{
    public static void Damage(GameObject target, float damage)
	{
		target.GetComponent<Attributes>().Health -= (int)damage;
		
	}
	public static void ApplyForce(GameObject target, Vector3 force)
	{
		target.GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);
	}

	public static void Die(GameObject target)
	{
		if (target.tag == "Enemy")
		{
			// Tillfällig
			Debug.Log("Enemy died");
			target.GetComponent<MeshRenderer>().material.color = Color.black;
			target.GetComponent<AIController>().CurrentState = AIStates.States.Dead;
		}
		else if (target.tag == "Player")
		{
			//Disable Movement
			//Play death animation
			// bool targetIsDead so it's not targetet and attacked again while dead
			target.GetComponent<PlayerStateController>().Die();
		}
	}
	/// <summary>
	/// Add or remove weight
	/// </summary>
	/// <param name="target"></param>
	/// <param name="weight"> value between -100 - +100 </param>
	public static void ChangeWeight(GameObject target, float weight)
	{
		float speedEffect = -weight / 100;
		ModifySpeed(target, speedEffect);
	}
	/// <summary>
	/// add or remove speed effect
	/// </summary>
	/// <param name="target"></param>
	/// <param name="speedEffect"> value between -1 - +1 (positive value speeds up, negative value slows down)</param>
	public static void ModifySpeed(GameObject target, float speedEffect)
	{
		if(target.tag == "Player")
		{
			target.GetComponent<PlayerMovementController>().SlowEffect += speedEffect;
		}
		else if(target.tag == "Enemy")
		{
			//TODO
			//target.GetComponent<NavMeshAgent>().speed += speedEffect;
		}
	}

	public static void Revive(GameObject target)
	{
		target.GetComponent<PlayerStateController>().Revive();
	}
}
