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
        if (!FOVView.activeSelf && isActive)
        {
            FOVView.SetActive(isActive);
        }
        else if (FOVView.activeSelf && !isActive)
        {
            FOVView.SetActive(isActive);
        }



        //FOVView.SetActive(isActive);
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
                    Effects.RegularWeaponDamage(target, Damage * (1 + wielder.GetComponent<Attributes>().statusEffectHandler.DmgBoost), HolderAgent);
                    Effects.ApplyForce(target, (target.transform.position - fov.transform.position).normalized * HitForce);
                    Effects.ApplyWeaponEffects(target, Utilities.ListDictionaryKeys(effects));
                }
				else
				{
                    Effects.Damage(target, Damage * (1 + wielder.GetComponent<Attributes>().statusEffectHandler.DmgBoost));
                }

                if (particleEffect)
                {
                    AkSoundEngine.PostEvent("Play_Blunt_thud", gameObject);
                    Instantiate(particleEffect, target.transform.position, target.transform.rotation * Quaternion.Euler(0, 180, 0));
                }
            }
        }

        base.DoAction(fov);
    }
}
