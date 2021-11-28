using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Tims Terrible breath (specialWeapon)
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15-11-16
public class TerribleBreath : AbstractSpecial
{
    [SerializeField]
    private float viewDistance = 10f;
    [SerializeField]
    private float viewAngle = 100f;
    [SerializeField]
    private float damageMultiplier = 3f;
    [SerializeField]
    private float hitForceMultiplier = 10f;
    [SerializeField]
    GameObject mouth;
    [SerializeField]
    private GameObject particleEffect;

    public override void SetFOVSize()
    {
        SpecialController.FOV.ViewAngle = viewAngle;
        SpecialController.FOV.ViewRadius = viewDistance;
    }

    public override void ToggleAim(bool isActive)
    {
        SpecialController.FOVVisualization.SetActive(isActive);
    }

    public override void StartAttack()
    {
        SpecialController.Animator.SetTrigger("isStartSpecialBreath");
    }
    public override void Attack()
    {
        if (!particleEffect.activeSelf)
        {
            AkSoundEngine.PostEvent("Play_Fire_woosh", gameObject);
            particleEffect.SetActive(true);
            StartCoroutine(CountdownTime(1.5f));

        }
        SpecialController.Animator.SetTrigger("isSpecialBreath");
    }

    public override void StartTurnEffect()
    {
        base.AddCharge();
    }
    public override void RevivedEffect()
    {
        Charges = MaxCharges;
    }

    public override void DoSpecialAction()
    {
        if (SpecialController.FOV.VisibleTargets.Count > 0)
        {
            foreach (GameObject target in SpecialController.FOV.VisibleTargets)
            {
                if(target.tag != "CoverObject")
				{
                    Effects.WeaponDamage(target, (Damage + (damageMultiplier * Charges)) * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), HolderAgent);
                    Effects.ApplyForce(target, (target.transform.position - SpecialController.FOV.transform.position).normalized * (HitForce + (hitForceMultiplier * Charges)));
                    Effects.ApplyWeaponEffects(target, effects);
                }
				else
				{
                    Effects.Damage(target, (Damage + (damageMultiplier * Charges)) * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost));
                }
                
            }
        }
        Charges = 0;
    }
    private IEnumerator CountdownTime(float time)
    {
        yield return new WaitForSeconds(time);
        if (particleEffect.activeSelf)
        {
            particleEffect.SetActive(false);
        }
    }
}
