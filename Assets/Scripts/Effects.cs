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

	public static void Revive(GameObject target)
	{
		target.GetComponent<Attributes>().Health = target.GetComponent<Attributes>().StartHealth / 2;
		target.GetComponent<PlayerStateController>().Revive();
	}
}
