using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Methods connected to all melee weapons
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 14/10-28
public abstract class MeleeWeapon : AbstractWeapon
{
    [SerializeField]
    protected GameObject particleEffect;

    public override void ToggleAim(bool isActive, GameObject FOVView)
	{
		FOVView.SetActive(isActive);
	}

	public override void Attack(Animator animator)
    {
		base.Attack(animator);
        AkSoundEngine.PostEvent("Play_MeleeSwingsPack_96khz_Stereo_NormalSwings39", gameObject);
    }

    public override void DoAction(FieldOfView fov)
    {
        GameObject wielder = gameObject.GetComponentInParent<Attributes>().gameObject;

        if(wielder == null)//if no ones holding this weapon do nothing
        {
            return;
        }

        if (fov.VisibleTargets.Count > 0)
        {
            foreach (GameObject target in fov.VisibleTargets)
            {
                if (target.layer != LayerMask.NameToLayer("Destructible"))
                {
                    Effects.RegularWeaponDamage(target, Damage * (1 + wielder.GetComponent<StatusEffectHandler>().DmgBoost), HolderAgent);
                    Effects.ApplyForce(target, (target.transform.position - fov.transform.position).normalized * HitForce);
                    Effects.ApplyWeaponEffects(target, effects);
                }
				else
				{
                    Effects.Damage(target, Damage * (1 + wielder.GetComponent<StatusEffectHandler>().DmgBoost));
                }

                if (particleEffect)
                {
                    AkSoundEngine.PostEvent("Play_Blunt_thud", gameObject);
                    Instantiate(particleEffect, target.transform.position, target.transform.rotation * Quaternion.Euler(0, 180, 0));
                }
            }
        }


        //Check for recoil recoil deals half the weapondamage and applies the effects
        if (effects.Contains(WeaponEffects.Recoil))
        {
            float rand = Random.value;
            if(rand < RecoilChance)
            {
                Effects.WeaponDamage(wielder, Damage/2);
                Effects.ApplyForce(wielder, (wielder.transform.forward * -1 * HitForce));
                Effects.ApplyWeaponEffects(wielder, effects);
            }
        }

        //disarms the wielder
        if (effects.Contains(WeaponEffects.Slippery))
        {
            float rand = Random.value;
            if(rand < SlipperyDropChance)
            {
                Effects.Disarm(wielder);
            }
        }

        base.DoAction(fov);
    }
}
