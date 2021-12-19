using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>
/// The harmful version of Susans specialWeapon
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 16-12-21
public class PaperShredder : AbstractSpecial
{
	[SerializeField]
	private float viewDistance = 0.75f;
	[SerializeField]
	private float viewAngle = 360f;
	[SerializeField]
	private GameObject particleEffect;
	[SerializeField]
	private GameObject explosionParticleEffect;

	[SerializeField] private ParticleSystem angryParticleEffect;

	[SerializeField] private Image countDownText;

	[SerializeField] private List<Sprite> numbers;

	private bool readyToExplode;
	private bool exploading;
	private bool changedFOV;
	public override void SetFOVSize()
	{
		SpecialController.FOV.ViewAngle = viewAngle;
		SpecialController.FOV.ViewRadius = viewDistance;
	}

	public override void ToggleAim(bool isActive)
	{
		if (!changedFOV)
		{
            if (Charges > MaxCharges)
            {
				Charges = MaxCharges;
            }
			switch (Charges)
			{
				case 0:
					ActionPower = 0;
					break;
				case 1:
					ActionPower = 1;
					break;
				case 2:
					ActionPower = 1.5f;
					break;
				case 3:
					ActionPower = 2.5f;
					break;
				case 4:
					ActionPower = 4;
					break;
				case 5:
					ActionPower = 5.5f;
					break;
				default:
					break;
			}
			specialController.FOV.ViewRadius *= ActionPower;
		}
		SpecialController.FOVVisualization.SetActive(isActive);
		changedFOV = true;
	}

	public override void StartAttack()
	{
		SpecialController.Animator.SetTrigger("isStartSpecialSelfExplode");
	}
	public override void Attack()
	{
		AkSoundEngine.PostEvent("SusanScream", gameObject);
		specialController.Animator.SetTrigger("isSpecialSelfExplode");
	}

	public override void StartTurnEffect()
	{
		AddCharge();
		changedFOV = false;
        if (Charges == MaxCharges)
        {
			readyToExplode = true;
        }
        else
        {
			readyToExplode = false;
        }
		SpecialController.FOV.ViewAngle = viewAngle;
		SpecialController.FOV.ViewRadius = viewDistance;
		exploading = false;
	}
	public override void TakeDamageEffect()
	{
		if (Charges < MaxCharges && !readyToExplode)
		{
			AddCharge();
		}
		if (readyToExplode && !exploading)
        {
			exploading = true;
			SpecialController.FOVVisualization.SetActive(true);
			specialController.FOV.ViewRadius *= 5.5f;
			StartCoroutine(CountDown(0.5f,3));
			StartCoroutine(CountDown(1f,2));
			StartCoroutine(CountDown(1.5f,1));
			StartCoroutine(CountDown(2f,0));
		}

	}
	protected override void AddCharge()
	{
		base.AddCharge();
	}

    public override void RevivedEffect()
    {
		readyToExplode = false;
		changedFOV = false;
		SpecialController.FOV.ViewAngle = viewAngle;
		SpecialController.FOV.ViewRadius = viewDistance;
		exploading = false;
	}
	private IEnumerator CountDown(float time, int number)
    {
		yield return new WaitForSeconds(time);
		countDownText.sprite = numbers[number];
        if (number == 0)
        {
			Attack();
			SpecialController.FOVVisualization.SetActive(false);
		}
	}


	public override void DoSpecialAction()
	{
		if (readyToExplode || Charges >= MaxCharges)
        {
			AkSoundEngine.PostEvent("Play_Explosion", gameObject);
			CameraShake.Shake(1f, 1f);
			Instantiate(explosionParticleEffect, transform.position, transform.rotation);
		}
		else
        {
			AkSoundEngine.PostEvent("SusanBurst", gameObject);
			Instantiate(particleEffect, transform.position, transform.rotation);
		}

		if (specialController.FOV.VisibleTargets.Count > 0)
		{
			foreach (GameObject target in SpecialController.FOV.VisibleTargets)
			{
				if (target.layer != LayerMask.NameToLayer("Destructible"))
				{
                    if (readyToExplode || Charges >= MaxCharges)
                    {
						Effects.WeaponDamage(target, Damage * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost) * ActionPower, HolderAgent);
						Effects.ApplyWeaponEffects(target, ultiEffects);
						Effects.ApplyForce(target, (target.transform.position - SpecialController.FOV.transform.position).normalized * HitForce);

					}
					else
					{
						Effects.ApplyWeaponEffects(target, effects);
						Effects.WeaponDamage(target, Damage * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost) * ActionPower, HolderAgent);
					}
				}
				else
				{
					Effects.Damage(target, Damage * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost));
				}
			}

		}
		Charges = 0;
		changedFOV = false;
		readyToExplode = false;
		SpecialController.FOV.ViewAngle = viewAngle;
		SpecialController.FOV.ViewRadius = viewDistance;
	}

    private void Update()
    {
        if (readyToExplode && !angryParticleEffect.isPlaying)
        {
			angryParticleEffect.gameObject.SetActive(true);
			angryParticleEffect.Play();
		}
		else if (!readyToExplode && angryParticleEffect.isPlaying)
        {
			angryParticleEffect.gameObject.SetActive(false);
			angryParticleEffect.Stop();
		}
	}
}
