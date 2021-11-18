//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PositiveCoffeeGrenade : GroundEffectGrenade
//{
//	[SerializeField]
//	PositiveGroundObject groundObject;

//	protected override void CreateGroundObject(Vector3 groundObjectPos)
//	{
//		groundObject.CreateGroundObject(groundObjectPos, FOV.ViewRadius, healthModifyAmount, statusEffects);
//	}

//	protected override void ImpactAgents()
//	{
//		List<GameObject> targetList = GetComponent<FieldOfView>().VisibleTargets;
//		foreach (GameObject target in targetList)
//		{
//			if (target.tag == "Player" || target.tag == "Enemy")
//			{
//				Effects.Heal(target, healthModifyAmount);
//				foreach (StatusEffectType effect in statusEffects)
//				{
//					Effects.ApplyStatusEffect(target, effect);
//				}
//			}
//		}
//		AddToEffectList(groundObject);
//	}

//	private void OnCollisionEnter(Collision collision)
//	{
//		if (collision.gameObject.transform.tag == "Ground")
//		{
//			CreateGroundObject(collision.contacts[0].point);
//		}
//		else
//		{
//			RaycastHit hit;
//			if (Physics.Raycast(transform.position, Vector3.down, out hit, maxDistance, LayerMask.GetMask("Ground")))
//			{
//				CreateGroundObject(hit.point);
//			}
//		}
//		Explode();
//	}
//}
