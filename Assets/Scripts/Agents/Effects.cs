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
		if(damage < 0)
        {
			Heal(target, -damage);
			return;
        }

		float dmgMod = 1 + target.GetComponent<StatusEffectHandler>().Vulnerability;
		target.GetComponent<Attributes>().Health -= (int)(damage * dmgMod);
	}

	public static void Heal(GameObject target, float amount)
    {
		if(amount < 0)
        {
			Damage(target, amount);
			return;
        }

		target.GetComponent<Attributes>().Health += (int)amount;
    }

	public static void DrainStamina(GameObject target, float drain)
    {
		target.GetComponent<Attributes>().Stamina -= drain;
    }

	public static void ApplyStatusEffect(GameObject target, StatusEffectType type, int duration = 1, int stacks = 1)
    {
		target.GetComponent<StatusEffectHandler>().ApplyEffect(type, duration, stacks);
    }

	public static void Disarm(GameObject target)
    {
		if(target == null)//could happen,it should be a very low chance, but that's even more reason to guard against it here I  think.
        {
			return;
        }

		//IMPlement!
    }

	/// <summary>
	/// Applies the offensive effects to the hit target
	/// </summary>
	public static void ApplyWeaponEffects(GameObject target, List<WeaponEffects> weaponEffects)
	{
		if(weaponEffects == null)
        {
			return;
        }
		foreach (WeaponEffects effect in weaponEffects)
		{
			switch (effect)
			{
				case WeaponEffects.Fire:
					Effects.ApplyStatusEffect(target, StatusEffectType.Fire, (int)EffectDurations.Medium, 1);
					break;
				case WeaponEffects.Bleed:
					Effects.ApplyStatusEffect(target, StatusEffectType.Bleed, (int)EffectDurations.Medium, 1);
					break;
				case WeaponEffects.Poison:
					Effects.ApplyStatusEffect(target, StatusEffectType.Poison, (int)EffectDurations.Long, 1);
					break;
				case WeaponEffects.StaminaDrain:
					Effects.ApplyStatusEffect(target, StatusEffectType.StaminaDrain, (int)EffectDurations.Long, 1);
					break;
				case WeaponEffects.Vulnerable:
					Effects.ApplyStatusEffect(target, StatusEffectType.Vulnerable, (int)EffectDurations.Short, 1);
					break;
				case WeaponEffects.Paralyze:
					Effects.ApplyStatusEffect(target, StatusEffectType.Paralyze, (int)EffectDurations.Short, 1);
					break;
				case WeaponEffects.Slow:
					Effects.ApplyStatusEffect(target, StatusEffectType.Slow, (int)EffectDurations.Medium, 1);
					break;
				case WeaponEffects.Disarm:
					float rand = Random.value;
					if (rand < AbstractWeapon.DisarmChance)
					{
						Disarm(target);
					}
					break;
			}
		}
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
