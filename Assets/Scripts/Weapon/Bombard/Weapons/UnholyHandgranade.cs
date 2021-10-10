using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnholyHandgranade : BombardWeapon
{
	[SerializeField]
	private GameObject unholyHandgarnadeHandle;
	//[SerializeField]
	//private GameObject unholyHandgarnadeThrowAim;
	[SerializeField]
	private int unholyHandgarnadeDamage = 4;
	[SerializeField]
	private int unholyHandgarnadeThrowForce = 10;
	[SerializeField]
	private int unholyHandgarnadeExplodeForce = 40;
	[SerializeField]
	private int unholyHandgarnadeThrowDamage = 2;
	[SerializeField]
	private float unholyHandgarnadeViewDistance = 20f;
	[SerializeField]
	private float unholyHandgarnadeViewAngle = 10f;
	

	void Start()
    {
		Handle = unholyHandgarnadeHandle;
		//ThrowAim = unholyHandgarnadeThrowAim;
		Damage = unholyHandgarnadeDamage;
		GranadeThrowForce = unholyHandgarnadeThrowForce;
		HitForce = unholyHandgarnadeExplodeForce;
		ThrowDamage = unholyHandgarnadeThrowDamage;
		ViewDistance = unholyHandgarnadeViewDistance;
		ViewAngle = unholyHandgarnadeViewAngle;
	}
}
