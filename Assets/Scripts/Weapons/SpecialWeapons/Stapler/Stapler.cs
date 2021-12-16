using UnityEngine;
using System.Collections.Generic;
using System.Collections;
/// <summary>
/// <para>
/// Tims stapler (specialWeapon)
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 14-12-21
public class Stapler : AbstractSpecial
{
	[SerializeField]
	private GameObject weaponMuzzle;
	[SerializeField]
	private GameObject laserAim;
	[SerializeField]
	private float bulletFireForce = 20;
	[SerializeField]
	protected GameObject bullet;

	[SerializeField]
	protected GameObject specialBullet;

	[SerializeField]
	private int bulletsInBurst = 4;
	private int bulletCount;

	[Tooltip("The maximum amount of degrees away from the direction aimed that the projectile might fly")]
	[SerializeField]
	protected float inaccuracy = 3;


	[SerializeField] private bool superCharged;
	private bool changedFOV;

	[SerializeField]
	protected List<GameObject> aimCones;

	/// <summary>
	/// The degrees by which the shot might change direction to either side. The effect of poison is included here
	/// </summary>
	public float Inaccuracy
	{
		get { return Mathf.Clamp(inaccuracy, 0, 89); }
		set { inaccuracy = Mathf.Clamp(value, 0, 89); }
	}

	public override void SetAimColor(Gradient gradient)
	{
		//laserAim.SetActive(true);
		GetComponentInChildren<LineRenderer>().colorGradient = gradient;
		//laserAim.SetActive(false);
        for (int i = 0; i < aimCones.Count; i++)
        {
			aimCones[i].GetComponentInChildren<MeshRenderer>().material.color = gradient.colorKeys[0].color;
        }
	}

	public override void ToggleAim(bool isActive)
	{
		if (!changedFOV)
		{
			switch (Charges)
			{
				case 1:
					//one shot
					aimCones[0].SetActive(isActive);
					aimCones[1].SetActive(false);
					aimCones[2].SetActive(false);
					aimCones[3].SetActive(false);
					aimCones[4].SetActive(false);
					break;
				case 2:
					//Three shots
					aimCones[0].SetActive(isActive);
					aimCones[1].SetActive(isActive);
					aimCones[2].SetActive(isActive);
					aimCones[3].SetActive(false);
					aimCones[4].SetActive(false);
					break;
				case 3:
					//five shots
					aimCones[0].SetActive(isActive);
					aimCones[1].SetActive(isActive);
					aimCones[2].SetActive(isActive);
					aimCones[3].SetActive(isActive);
					aimCones[4].SetActive(isActive);
					break;
				default:
					break;
			}
			if (superCharged)
			{
				//five shots with clusterbombs at the end
				aimCones[0].SetActive(true);
				aimCones[1].SetActive(true);
				aimCones[2].SetActive(true);
				aimCones[3].SetActive(true);
				aimCones[4].SetActive(true);
				Charges = MaxCharges;
			}
		}
		SpecialController.FOVVisualization.SetActive(true);
		changedFOV = true;

		//laserAim.SetActive(isActive);
		//UpdateAimCone();
	}
	/// <summary>
	/// The maximum amount of degrees from the aim direction that the shot can deviate. This takes the possibility of being po�soned into account.<br/>
	/// Also updates the size and such of the aimcone.
	/// </summary>
	protected void UpdateAimCone()
	{
		//float width = 2 * Mathf.Tan(Inaccuracy * Mathf.Deg2Rad);//the 1,1,1 scale of the cone has length one and width one.
		//aimCones.transform.localScale = new Vector3(width, 1, 1);
	}

	public override void StartAttack()
	{
		SpecialController.Animator.SetTrigger("isStartSpecialStapler");
	}
	public override void Attack()
	{
		SpecialController.Animator.SetTrigger("isSpecialStapler");
	}

	public override void StartTurnEffect()
	{
		base.AddCharge();
		DisableAimCones();
		changedFOV = false;
	}
	public override void RevivedEffect()
	{
		Charges = MaxCharges;
		DisableAimCones();
		superCharged = true;
		changedFOV = false;
	}

	/// <summary>
	/// Returns a randomized direction within the weapons (in)accuracy.
	/// </summary>
	/// <param name="aim"></param>
	/// <returns></returns>
	//protected Vector3 GetBulletDirection()
	//{
	//	Vector3 bulletDir = transform.forward;//I rotate this forward vector by a random amount of degrees basically
	//	float deviation = ((Random.value * 2) - 1) * Inaccuracy * Mathf.Deg2Rad;

	//	float newX = bulletDir.x * Mathf.Cos(deviation) - bulletDir.z * Mathf.Sin(deviation);
	//	float newZ = bulletDir.x * Mathf.Sin(deviation) + bulletDir.z * Mathf.Cos(deviation);
	//	bulletDir = new Vector3(newX, 0, newZ);

	//	return bulletDir;
	//}

	public override void DoSpecialAction()
	{
		changedFOV = false;
		DisableAimCones();
		switch (Charges)
		{
			case 0:
				SpecialController.Animator.SetTrigger("isCancelAction");
				break;
			case 1:
				StartCoroutine(volleys(0f, 1));
				StartCoroutine(volleys(0.2f, 1));
				StartCoroutine(volleys(0.4f, 1));
				SpecialController.Animator.SetTrigger("isSpecialStaplerShot");

			break;
			case 2:
				StartCoroutine(volleys(0f,3));
				StartCoroutine(volleys(0.2f, 3));
				StartCoroutine(volleys(0.4f, 3));

				SpecialController.Animator.SetTrigger("isSpecialStaplerShot");
			break;
			case 3:
				StartCoroutine(volleys(0f, 5));
				StartCoroutine(volleys(0.2f, 5));
				StartCoroutine(volleys(0.4f, 5));

				SpecialController.Animator.SetTrigger("isSpecialStaplerShot");
				break;
			default:
				break;
		}
		Charges = 0;

		//if(bulletCount < bulletsInBurst)
		//{

		//          //if (bulletCount > 0)
		//          //{
		//          //	//if (particleEffect)
		//          //	//{
		//          //	//	Instantiate(particleEffect, weaponMuzzle.transform.position, weaponMuzzle.transform.rotation * Quaternion.Euler(0, 180, 0));
		//          //	//}
		//          //	//Vector3 direction = GetBulletDirection();
		//          //	bullet.GetComponent<Bullet>().CreateBullet(HolderAgent, weaponMuzzle.transform.position, direction, bulletFireForce, HitForce, Damage, this.effects);
		//          //	bulletCount--;
		//          //}
		//          //else
		//          //{
		//          //	SpecialController.Animator.SetTrigger("isCancelAction");
		//          //	ActionPower = 0;
		//          //}			
		//      }
		//else
		//{
		//	//if (particleEffect)
		//	//{
		//	//	Instantiate(particleEffect, weaponMuzzle.transform.position, weaponMuzzle.transform.rotation * Quaternion.Euler(0, 180, 0));
		//	//}
		//	//Vector3 direction = GetBulletDirection();
		//	bullet.GetComponent<Bullet>().CreateBullet(HolderAgent, weaponMuzzle.transform.position, direction, bulletFireForce, HitForce, Damage, this.effects);
		//	bulletCount--;

		//	SpecialController.Animator.SetTrigger("isSpecialStaplerShot");
		//}
	}



	private void DisableAimCones()
    {
		for (int i = 0; i < aimCones.Count; i++)
		{
			aimCones[i].SetActive(false);
		}
	}

	private void StapleShot(GameObject aimCone)
    {
		effects[0] = RandomEffect();

        bullet.GetComponent<Bullet>().CreateBullet(HolderAgent, aimCone.transform.position, aimCone.transform.forward, bulletFireForce, HitForce, Damage, this.effects);
	}


	private StatusEffectType RandomEffect()
    {
		int chance = Random.Range(0, 100);
		int rnd = 0;

		if (superCharged)
        {
			chance += 30;
			rnd = Random.Range(1, StatusEffectType.GetNames(typeof(StatusEffectType)).Length - 1);
		}
        else
        {
			//Only normal no hell versions nor para
			rnd = Random.Range(1, 5);
		}
		if (chance >= 60)
        {
			return (StatusEffectType)rnd;
        }
        else
        {
			return StatusEffectType.none;
        }
    }
	/// <summary>
	/// 
	/// </summary>
	/// <param name="time"></param>
	/// <param name="numberOfShots"></param>
	/// <returns></returns>
	private IEnumerator volleys(float time, int numberOfShots)
	{
		yield return new WaitForSeconds(time);
        for (int i = 0; i < numberOfShots; i++)
        {
			StapleShot(aimCones[i]);
        }
	}
}
