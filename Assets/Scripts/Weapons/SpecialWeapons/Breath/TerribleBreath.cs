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
    private List<GameObject> particleEffects;

    [SerializeField] private bool superCharged = false;


    public bool SuperCharged
    {
        get { return superCharged; }
    }

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
        changedFOV = false;
        if (superCharged)
        {
            if (!particleEffects[3].activeSelf)
            {
                AkSoundEngine.PostEvent("Dragon_Fire_short", gameObject);
                particleEffects[3].SetActive(true);
                StartCoroutine(CountdownTime(1.5f, particleEffects[3]));
            }
        }
        else
        {
            switch (Charges)
            {
                case 1:
                    if (!particleEffects[0].activeSelf)
                    {
                        AkSoundEngine.PostEvent("Play_Fire_woosh", gameObject);
                        particleEffects[0].SetActive(true);
                        StartCoroutine(CountdownTime(1.5f, particleEffects[0]));
                    }
                    break;
                case 2:
                    if (!particleEffects[1].activeSelf)
                    {
                        AkSoundEngine.PostEvent("Play_Fire_woosh", gameObject);
                        particleEffects[1].SetActive(true);
                        StartCoroutine(CountdownTime(1.5f, particleEffects[1]));
                    }
                    break;
                case 3:
                    if (!particleEffects[2].activeSelf)
                    {
                        AkSoundEngine.PostEvent("Play_Fire_woosh", gameObject);
                        particleEffects[2].SetActive(true);
                        StartCoroutine(CountdownTime(1.5f, particleEffects[2]));
                    }
                    break;
            }
        }
        SpecialController.Animator.SetTrigger("isSpecialBreath");
        Charges = 0;
    }

    public override void StartTurnEffect()
    {
        base.AddCharge();
        changedFOV = false;
        SpecialController.FOV.ViewRadius = viewDistance;
        SpecialController.FOV.ViewAngle = viewAngle;
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
                        Effects.WeaponDamage(target, (Damage + (damageMultiplier * ActionPower) + 20) , HolderAgent);
                        Effects.ApplyForce(target, (target.transform.position - SpecialController.FOV.transform.position).normalized * (HitForce + (hitForceMultiplier * ActionPower)));
                        Effects.ApplyWeaponEffects(target, HolderAgent, ultiEffects);
                    }
                    else
                    {
                        Effects.WeaponDamage(target, (Damage + (damageMultiplier * ActionPower)) , HolderAgent);
                        Effects.ApplyForce(target, (target.transform.position - SpecialController.FOV.transform.position).normalized * (HitForce + (hitForceMultiplier * ActionPower)));
                        Effects.ApplyWeaponEffects(target, HolderAgent, effects);
                    }
                }
				else
				{
                    Effects.Damage(target, (Damage + (damageMultiplier * ActionPower)) );
                }

            }
        }
        ActionPower = 0;
        superCharged = false;
        SpecialController.FOV.ViewRadius = viewDistance;
        SpecialController.FOV.ViewAngle = viewAngle;
    }
    private IEnumerator CountdownTime(float time, GameObject particleEffect)
    {
        yield return new WaitForSeconds(time);
        if (particleEffect.activeSelf)
        {
            particleEffect.SetActive(false);
        }
    }
}
