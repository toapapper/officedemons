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

    public override void ToggleAim(bool isActive/*, GameObject FOVView*/)
	{
        if (!WeaponController.FOVVisualization.activeSelf && isActive)
        {
            WeaponController.FOVVisualization.SetActive(isActive);
        }
        else if (WeaponController.FOVVisualization.activeSelf && !isActive)
        {
            WeaponController.FOVVisualization.SetActive(isActive);
        }



        //FOVView.SetActive(isActive);
	}

	public override void Attack(Animator animator)
    {
		base.Attack(animator);
        AkSoundEngine.PostEvent("Play_MeleeSwingsPack_96khz_Stereo_NormalSwings39", gameObject);
    }

    public override void DoAction(/*FieldOfView fov*/)
    {
        GameObject wielder = gameObject.GetComponentInParent<Attributes>().gameObject;

        if(wielder == null)//if no ones holding this weapon do nothing
        {
            return;
        }

        if (WeaponController.FOV.VisibleTargets.Count > 0)
        {
            foreach (GameObject target in WeaponController.FOV.VisibleTargets)
            {
                if (target.layer != LayerMask.NameToLayer("Destructible"))
                {
                    Effects.RegularWeaponDamage(target, Damage , HolderAgent);
                    Effects.ApplyForce(target, (target.transform.position - WeaponController.FOV.transform.position).normalized * HitForce, HolderAgent);
                    Effects.ApplyWeaponEffects(target, wielder, Utilities.ListDictionaryKeys(effects));
                }
				else
				{
                    Effects.Damage(target, Damage , HolderAgent);
                }

                if (particleEffect)
                {
                    AkSoundEngine.PostEvent("Play_Blunt_thud", gameObject);
                    Instantiate(particleEffect, target.transform.position, target.transform.rotation * Quaternion.Euler(0, 180, 0));
                }
            }
        }

        base.DoAction();
    }
}
