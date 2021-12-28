using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// The supporting version of Devins specialWeapons [DEPRECATED AND NOT IN USE. both devins use hotcoffee-script with different prefab-settings]
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15-11-16
public class StaleCoffeeCupSpecial : CoffeeCupSpecial
{
	[SerializeField]
	private List<StatusEffectType> buffEffects;


	public override void DoSpecialAction()
	{
		Vector3 velocity = SpecialController.ThrowAim.InitialVelocity;

		
		ActionPower = 0;
	}
}
