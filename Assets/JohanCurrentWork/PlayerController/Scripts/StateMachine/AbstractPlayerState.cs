using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public abstract class AbstractPlayerState : MonoBehaviour, IPlayerState
{
    protected PlayerStateController playerStateCtrl;
	protected PlayerMovementController playerMovement;

	//World transform variables
	private Vector3 forward;
	private Vector3 right;

	private Vector2 moveInput;

	////Character movers
	//private CharacterController character;
	//private WeaponHand weaponHand;
	//private SpecialHand specialHand;

	////Movement variables
	private Vector3 moveDirection = Vector3.zero;
	//private Vector3 moveAmount = Vector3.zero;
	//private Vector3 smoothMoveVelocity;
	//private Quaternion rotationDirection;

	////Movement speeds
	//[SerializeField]
	//private float moveSpeed = 10f;
	//[SerializeField]
	//private float rotationSpeed = 500f;

	//Throwing variables
	[SerializeField]
    private float throwForceMultiplier = 25f;
    [SerializeField]
    private float maxThrowForce = 30f;
    private float addedThrowForce;

    //Helper variables
    private List<GameObject> nearbyObjects = new List<GameObject>();
    private bool isWeaponEquipped;
    private bool isActionStarted;
    private bool isAttack, isSpecialAttack, isThrowing, isAddingThrowForce;

    //protected WeaponHand WeaponHand
    //{
    //    get { return weaponHand; }
    //    set { weaponHand = value; }
    //}
    //protected SpecialHand SpecialHand
    //{
    //    get { return specialHand; }
    //    set { specialHand = value; }
    //}
    protected Vector2 MoveInput
	{
		get { return moveInput; }
		set { moveInput = value; }
	}
	protected Vector3 Forward
	{
		get { return forward; }
		set { forward = value; }
	}
	protected Vector3 Right
	{
		get { return right; }
		set { right = value; }
	}
	protected Vector3 MoveDirection
	{
		get { return moveDirection; }
		set { moveDirection = value; }
	}

	protected bool IsWeaponEquipped
	{
        get { return isWeaponEquipped; }
        set { isWeaponEquipped = value; }
    }
    protected bool IsThrowing
    {
        get { return isThrowing; }
        set { isThrowing = value; }
    }
    protected float AddedThrowForce
    {
        get { return addedThrowForce; }
        set { addedThrowForce = value; }
    }


    protected List<GameObject> NearbyObjects
    {
        get { return nearbyObjects; }
        set { nearbyObjects = value; }
    }


    private void Awake()
    {
        playerStateCtrl = (PlayerStateController)FindObjectOfType(typeof(PlayerStateController));
		playerMovement = (PlayerMovementController)FindObjectOfType(typeof(PlayerMovementController));

		forward = Camera.main.transform.forward;
		forward.y = 0;
		forward.Normalize();
		right = new Vector3(forward.z, 0, -forward.x);

		//      character = GetComponent<CharacterController>();
		//      weaponHand = GetComponent<WeaponHand>();
		//specialHand = GetComponent<SpecialHand>();
	}

    internal abstract void OnMove(CallbackContext context);
    internal abstract void OnAttack(CallbackContext context);
    internal abstract void OnSpecial(CallbackContext context);
    internal abstract void OnThrow(CallbackContext context);
    internal abstract void OnPickup(CallbackContext context);

    protected virtual void FixedUpdate()
    {
        //Rotation
        if (moveDirection != Vector3.zero)
        {
            CalculateRotation();
        }
        if (transform.rotation != rotationDirection)
        {
            PerformRotation();
        }
        if (isActionStarted)
        {
            //Throwing
            if (isAddingThrowForce && addedThrowForce <= maxThrowForce)
            {
                addedThrowForce += throwForceMultiplier * Time.fixedDeltaTime;
            }
        }
        //Movement
        else
        {
            CalculateMovement();
            if (moveAmount != Vector3.zero)
            {
                PerformMovement();
            }
        }
        if (transform.position.y > 0)
        {
            playerMovement.Move(Vector3.down);
        }
    }

    protected void CalculateRotation()
    {
        rotationDirection = Quaternion.LookRotation(moveDirection, Vector3.up);
    }
    protected void PerformRotation()
    {
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationDirection, rotationSpeed * Time.fixedDeltaTime);
        playerMovement.Rotate(Quaternion.RotateTowards(transform.rotation, rotationDirection, rotationSpeed * Time.fixedDeltaTime));
    }
    protected void CalculateMovement()
    {
        moveDirection = (moveInput.x * right + moveInput.y * forward).normalized;
        Vector3 targetMoveAmount = moveDirection * moveSpeed;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);
    }
    protected void PerformMovement()
    {
        //Vector3 localMove = moveAmount * Time.fixedDeltaTime;
        playerMovement.Move(moveAmount * Time.fixedDeltaTime);
    }



    //public abstract void DoAction();
    public abstract void OnStateExit();
    public abstract void OnStateEnter();




    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "WeaponObject")
        {
            nearbyObjects.Remove(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "WeaponObject")
        {
            Physics.IgnoreCollision(character, collision.collider);
        }
    }
}
