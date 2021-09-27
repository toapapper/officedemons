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
	[SerializeField]
	private float poleViewDistance = 4f;
	[SerializeField]
	private float poleViewAngle = 100f;


	private void Start()
	{
		Handle = poleHandle;
		Damage = poleDamage;
		ThrowDamage = poleThrowDamage;
		ViewDistance = poleViewDistance;
		ViewAngle = poleViewAngle;
	}
}
