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
		if(damage < 0)
        {
			Heal(target, -damage);
			return;
        }

		int dmg = (int)(damage * (1 + target.GetComponent<StatusEffectHandler>().Vulnerability));
		target.GetComponent<Attributes>().Health -= dmg;

		UIManager.Instance.NewFloatingText(target, "" + dmg, Color.red);
	}

	public static void Heal(GameObject target, float amount)
    {
		if(amount < 0)
        {
			Damage(target, amount);
			return;
        }

		target.GetComponent<Attributes>().Health += (int)amount;

		UIManager.Instance.NewFloatingText(target, "+" + amount + " HP", Color.green);
    }

	public static void DrainStamina(GameObject target, float drain)
    {
		target.GetComponent<Attributes>().Stamina -= drain;
		UIManager.Instance.NewFloatingText(target, "-" + drain + " Stamina", Color.yellow);
    }

	public static void ApplyStatusEffect(GameObject target, StatusEffectType type, int duration = 1, int stacks = 1)
    {
		target.GetComponent<StatusEffectHandler>().ApplyEffect(type, duration, stacks);

		UIManager.Instance.NewFloatingText(target, "Status applied: " + type, Color.cyan);
    }

	public static void Disarm(GameObject target)
    {
		if(target == null)//could happen that the target is null,it should be a low chance but that's even more reason to guard against it here I think.
        {
			return;
        }

		target.GetComponent<WeaponHand>().DropWeapon();

		UIManager.Instance.NewFloatingText(target, "WEAPON DROPPED!", Color.red);
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
						Effects.Disarm(target);
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
