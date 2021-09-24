using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHand : MonoBehaviour
{
	[SerializeField]
	private GameObject handObject;
	private AbstractWeapon objectInHand;

	[SerializeField]
	private int damage = 5;

	private Actions actions;

    //Throwing variables
    [SerializeField]
    private float throwForce = 25f;
    [SerializeField]
    private float maxThrowForce = 30f;
    public float addedForce;
	private void Start()
	{
		actions = GetComponent<Actions>();
	}
	public void Equip(GameObject newObject)
	{
		newObject.GetComponent<AbstractWeapon>().PickUpIn(handObject);
		objectInHand = newObject.GetComponent<AbstractWeapon>();
	}

	public void DropObject(Vector3 direction)
	{
		objectInHand.Drop(direction);
		Debug.Log("Drop");
		objectInHand = null;
	}

	public void Hit(Animator animator)
	{
		//Actions Attack(objectinHand)
		Debug.Log(objectInHand);
		if (objectInHand != null)
		{
			objectInHand.Hit(animator);
			actions.Attack(objectInHand);
		}
		else
		{
			Debug.Log("HandHit" + damage);
		}
	}
	public void AddThrowForce()
	{
		if (addedForce <= maxThrowForce)
		{
			addedForce += throwForce * Time.fixedDeltaTime;
		}
	}

	public void AimThrow(Animator animator)
	{
		if (objectInHand != null)
		{
			animator.SetTrigger("isAimThrow");
		}
	}
	public void Throw(Animator animator, Vector3 direction)
	{
		if (objectInHand != null)
		{
			animator.SetTrigger("isThrow");
		}
	}
	public void ReleaseThrow()
	{
		if (objectInHand != null)
		{
			objectInHand.ReleaseThrow(addedForce);

			addedForce = 0;
			objectInHand = null;
		}
	}
}
