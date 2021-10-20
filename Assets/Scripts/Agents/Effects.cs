using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Static effects that can be applied to characters
/// </para>
///   
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 14/10-21
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

	public static void Revive(GameObject target)
	{
		target.GetComponent<PlayerStateController>().Revive();
	}
}
