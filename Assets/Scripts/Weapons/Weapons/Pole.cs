using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole: SwingWeapon
{
    [SerializeField]
    private GameObject poleHandle;
    [SerializeField]
    private int poleDamage = 10;
    [SerializeField]
    private int poleThrowDamage = 15;


	private void Start()
	{
		Handle = poleHandle;
		Damage = poleDamage;
		ThrowDamage = poleThrowDamage;
	}
}
