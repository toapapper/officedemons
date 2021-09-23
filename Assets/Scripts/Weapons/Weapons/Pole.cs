using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole: SwingWeapon
{
    [SerializeField]
    private GameObject poleHandle;
    [SerializeField]
    private float poleDamage = 10f;
    [SerializeField]
    private float poleThrowDamage = 15f;

	private void Start()
	{
		Handle = poleHandle;
		Damage = poleDamage;
		ThrowDamage = poleThrowDamage;
	}
}
