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
	public static void RegularWeaponDamage(GameObject target, float damage, GameObject wielder)
	{
		if(wielder.tag == "Player")
		{
			wielder.GetComponent<SpecialHand>().GiveRegularDamageEffect();
		}
        //Damage(target, damage, wielder);
        if (target.tag == "Enemy")
        {
            if (target.name != "tank" && target.GetComponent<AIController>().InActiveCombat)
            {
                WeaponDamage(target, damage, wielder);
            }
            else if(target.name == "tank" && target.GetComponent<TankController>().InActiveCombat)
            {
                WeaponDamage(target, damage, wielder);
            }
        }
		else if (target.tag == "Player")
		{
			WeaponDamage(target, damage, wielder);
		}
	}
	public static void WeaponDamage(GameObject target, float damage, GameObject wielder = null)
	{
		Damage(target, damage, wielder);

		if(target.tag == "Player")
		{
			target.GetComponent<SpecialHand>().TakeDamageEffect();
		}
	}

	public static void Damage(GameObject target, float damage, GameObject wielder = null)
	{
		//Debug.Log("Damage done, wielder: " + wielder + " + target: " + target);

		if (damage < 0)
		{
			Heal(target, -damage);
			return;
		}
		else if (damage == 0)
		{
			return;
		}

		int dmg = (int)damage;
		if (target.tag == "Player" || target.tag == "Enemy")
		{
			dmg = (int)(damage * (1 + target.GetComponent<Attributes>().statusEffectHandler.Vulnerability));
		}

		if(target.GetComponent<Attributes>().Health > 0)
        {
			target.GetComponent<Attributes>().Health -= dmg;

			UIManager.Instance.NewFloatingText(target, "" + dmg, Color.red);

			if (wielder != null && target.GetComponent<Attributes>().Health <= 0)
			{
				if (wielder.tag == "Player")
				{
					wielder.GetComponent<Attributes>().KillCount++;
					wielder.GetComponent<SpecialHand>().KillEffect();
				}
			}
		}
	}
	public static void Heal(GameObject target, float amount)
    {
		if(amount < 0)
        {
			WeaponDamage(target, amount);
			return;
        }
		else if(amount == 0)
        {
			return;
        }

		target.GetComponent<Attributes>().Health += (int)amount;

		UIManager.Instance.NewFloatingText(target, "+" + amount + " HP", Color.green);
    }

	public static void DrainStamina(GameObject target, float drain)
    {
		if(drain == 0)
        {
			return;
        }

		target.GetComponent<Attributes>().Stamina -= drain;
		UIManager.Instance.NewFloatingText(target, "-" + drain + " Stamina", Color.yellow);
    }

	public static void ApplyStatusEffect(GameObject target, StatusEffectType type)
    {
        if (target.tag == "Enemy")
        {
            if (target.name != "tank" && target.GetComponent<AIController>().InActiveCombat)
            {
                target.GetComponent<Attributes>().statusEffectHandler.ApplyEffect(type);
                UIManager.Instance.NewFloatingText(target, "Status applied: " + type, Color.cyan);
            }
        }

    }

	public static void Disarm(GameObject target)
    {
		if(target == null)//could happen that the target is null,it should be a low chance but that's even more reason to guard against it here I think.
        {
			return;
        }

		target.GetComponent<WeaponHand>().Disarm();

		UIManager.Instance.NewFloatingText(target, "WEAPON DROPPED!", Color.red);
    }

    /// <summary>
    /// Applies the offensive effects to the hit target
    /// </summary>
    public static void ApplyWeaponEffects(GameObject target, List<StatusEffectType> weaponEffects)
    {
        if (target.tag == "Enemy")
        {
            if (weaponEffects == null)
            {
                return;
            }
            foreach (StatusEffectType type in weaponEffects)
            {
								ApplyStatusEffect(target, type);
            }
        }
    }

	public static void ApplyForce(GameObject target, Vector3 force)
	{
		if(target.tag != "CoverObject" && target.GetComponent<Rigidbody>() != null)
        {
			target.GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);
        }
	}

	public static void Die(GameObject target)
	{
		if (target.tag == "Enemy")
		{
            if (target.name == "tank")
            {
                target.GetComponent<TankController>().CurrentState = TankController.TankStates.Dead;
                target.GetComponent<TankController>().Die();
            }
            else
            {
                Disarm(target);

                target.GetComponent<Attributes>().statusEffectHandler.OnDeath();

                target.GetComponent<AIController>().CurrentState = AIStates.States.Dead;
                target.GetComponent<AIController>().Die();
            }

		}
		else if (target.tag == "Player")
		{
			target.GetComponent<WeaponHand>().ToggleAimView(false);
			target.GetComponent<SpecialHand>().ToggleAimView(false);
			target.GetComponent<Animator>().SetTrigger("isCancelAction");

			Debug.Log("pre player death disarm:");
			Disarm(target);

			Debug.Log("pre player death cleareffects:");
			target.GetComponent<Attributes>().statusEffectHandler.OnDeath();

			Debug.Log("pre player death Die:");
			target.GetComponent<PlayerStateController>().Die();
		}
		else if(target.layer == LayerMask.NameToLayer("Destructible"))
        {
			target.GetComponent<DestructibleObjects>().Explode();
        }
	}
	/// <summary>
	/// Add weight to player. if negative remove weight
	/// </summary>
	/// <param name="target"></param>
	/// <param name="weight"> value between -100 - +100 </param>
	public static void ChangeWeight(GameObject target, float weight)
	{
		if(weight > 0)
        {
			UIManager.Instance.NewFloatingText(target, "SLOW", Color.red);
        }
		else if(weight < 0)
        {
			UIManager.Instance.NewFloatingText(target, "DE-SLOW", Color.green);
		}

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
		//Debug.Log("ModifySpeed target: " + target + " amount: " + speedEffect);

		if(target.tag == "Player")
		{
			float playerSpeed = target.GetComponent<PlayerMovementController>().SlowEffect + speedEffect;

			if(playerSpeed >= 1)
            {
				playerSpeed = 1;
            }
			else if(playerSpeed <= 0)
            {
				playerSpeed = 0;
            }

			//Debug.Log("playerSpeeed on speedmodify: " + playerSpeed);

			target.GetComponent<PlayerMovementController>().SlowEffect = playerSpeed;
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
