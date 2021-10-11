using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Effects
{
    public static void Damage(GameObject target, int damage)
	{
		target.GetComponent<Attributes>().Health -= damage;
		
	}

	public static void ApplyForce(GameObject target, Vector3 force)
	{
		target.GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);
	}
}
