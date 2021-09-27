using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uzi : BurstShotWeapon
{
	[SerializeField]
	private GameObject uziHandle;
	[SerializeField]
	private int uziDamage = 4;
	[SerializeField]
	private int uziThrowDamage = 2;

	private void Start()
	{
		Handle = uziHandle;
		Damage = uziDamage;
		ThrowDamage = uziThrowDamage;
	}
}
