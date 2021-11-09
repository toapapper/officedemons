//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class OfficeChairSpecial : AbstractSpecial
//{
//	//[SerializeField]
//	//private GameObject laserAim;
//	//[SerializeField]
//	//private float rushForce = 100f;

//	//public bool isProjectile;
//	//public override void SetAim(FieldOfView fov, Gradient gradient) { }
//	//public override void SetAimGradient(Gradient gradient) { }
//	//public override void ToggleAim(bool isActive, GameObject FOVView, GameObject throwAim) { }
//	//{
//	//	laserAim.SetActive(isActive);
//	//	//if (!isActive)
//	//	//{
//	//	//	throwAim.GetComponent<LineRenderer>().positionCount = 0;
//	//	//}
//	//	//throwAim.SetActive(isActive);
//	//}

//	//public override void StartAttack(Animator animator) { }
//	//{
//	//	animator.SetTrigger("isStartSpecialRush");
//	//}
//	//public override void Attack(Animator animator) { }
//	//{
//	//	animator.SetTrigger("isSpecialRush");
//	//}

//	//public override void DoSpecialAction(FieldOfView fov) { }
//	//{
//	//	holderAgent.GetComponent<Rigidbody>().AddForce(-holderAgent.transform.forward * rushForce, ForceMode.VelocityChange);
//	//	GameManager.Instance.StillCheckList.Add(holderAgent);
//	//	//Vector3 forward = transform.parent.parent.forward;
//	//	//forward.y = 0;
//	//	//forward.Normalize();
//	//	//Vector3 right = new Vector3(forward.z, 0, -forward.x);

//	//	//Vector3 direction = (Quaternion.AngleAxis(-GetComponentInParent<SpecialHand>().ThrowAim.initialAngle, right) * forward).normalized;
//	//	//float throwForce = GetComponentInParent<SpecialHand>().ThrowAim.initialVelocity;

//	//	//grenade.GetComponent<GrenadeObject>().CreateGrenade(transform.position, direction, throwForce, HitForce, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), effects);
//	//}

//	//private void OnCollisionEnter(Collision collision)
//	//{
//	//	if (isProjectile)
//	//	{
//	//		if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
//	//		{



//	//		}
//	//		isProjectile = false;
//	//		GameManager.Instance.StillCheckList.Remove(holderAgent);
//	//	}
//	//}
//	//public override void OnCollisionEnter(Collision collision) { }
//}
