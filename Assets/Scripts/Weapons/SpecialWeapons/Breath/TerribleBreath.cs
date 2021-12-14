using System.Collections;
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
    [SerializeField]private bool superCharged;
    private bool changedFOV = false;

    public override void SetFOVSize()
    {
        SpecialController.FOV.ViewAngle = viewAngle;
        SpecialController.FOV.ViewRadius = viewDistance;
    }

    public override void ToggleAim(bool isActive)
    {
        if (!changedFOV)
        {
            switch (Charges)
            {
                case 1:
                    ActionPower = 1;
                    break;
                case 2:
                    ActionPower = 2.5f;
                    break;
                case 3:
                    ActionPower = 4;
                    break;
                default:
                    break;
            }
            if (superCharged)
            {
                ActionPower = 5;
                specialController.FOV.ViewAngle += 60;
                Charges = MaxCharges;
                Debug.Log("SUPERCHARGED");
            }
            specialController.FOV.ViewRadius *= ActionPower;
        }       
        SpecialController.FOVVisualization.SetActive(isActive);
        changedFOV = true;
    }

    public override void StartAttack()
    {
        SpecialController.Animator.SetTrigger("isStartSpecialBreath");
    }
    public override void Attack()
    {
        Charges = 0;
        changedFOV = false;
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
        changedFOV = false;
        SpecialController.FOV.ViewRadius = viewDistance;
        SpecialController.FOV.ViewAngle = viewAngle;
        Charges = MaxCharges;
        superCharged = true;
    }
    public override void RevivedEffect()
    {
        Charges = MaxCharges;
        superCharged = true;
        changedFOV = false;
    }

    public override void DoSpecialAction()
    {
        if (SpecialController.FOV.VisibleTargets.Count > 0)
        {
            foreach (GameObject target in SpecialController.FOV.VisibleTargets)
            {
                if (target.layer != LayerMask.NameToLayer("Destructible"))
                {
                    if (superCharged)
                    {
                        Effects.WeaponDamage(target, (Damage + (damageMultiplier * ActionPower)) * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost), HolderAgent);
                        Effects.ApplyForce(target, (target.transform.position - SpecialController.FOV.transform.position).normalized * (HitForce + (hitForceMultiplier * ActionPower)));
                        //Effects.ApplyWeaponEffects(target, effects);
                        Effects.ApplyWeaponEffects(target, ultiEffects);
                    }
                    else
                    {
                        Effects.WeaponDamage(target, (Damage + (damageMultiplier * ActionPower)) * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost), HolderAgent);
                        Effects.ApplyForce(target, (target.transform.position - SpecialController.FOV.transform.position).normalized * (HitForce + (hitForceMultiplier * ActionPower)));
                        Effects.ApplyWeaponEffects(target, effects);
                    }
                }
				else
				{
                    Effects.Damage(target, (Damage + (damageMultiplier * ActionPower)) * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost));
                }
                
            }
        }
        ActionPower = 0;
        superCharged = false;
        SpecialController.FOV.ViewRadius = viewDistance;
        SpecialController.FOV.ViewAngle = viewAngle;
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
