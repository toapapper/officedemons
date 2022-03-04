using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



/// <summary>
/// <para>
/// Abstract class controlling all weapons
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 28/11-21
public abstract class AbstractWeapon : MonoBehaviour
{
    [SerializeField]
    private Sprite weaponTexture;

    public Sprite WeaponTexture
    {
        get { return weaponTexture; }
    }

	[Tooltip("The Status effects this weapon has at spawn. Can be used when the game is running in the inspector as well.")]
	[SerializeField] protected List<StatusEffectType> Effects_Editor;

	/// <summary> key = type, value = particleeffect </summary>
    protected Dictionary<StatusEffectType, GameObject> effects = new Dictionary<StatusEffectType, GameObject>();
	[SerializeField] protected GameObject particleEffectsPosition;

	[SerializeField]
	private GameObject holderAgent;
	private WeaponHand weaponController;

	[SerializeField]
	private GameObject handle;
	[SerializeField]
	private float damage = 10;
	[SerializeField]
	private float hitForce = 2;
	[SerializeField]
	private float throwDamage = 2;
	[SerializeField]
	private float viewDistance = 20f;
	[SerializeField]
	private float viewAngle = 10f;
	[SerializeField]
	private int durability = 3;
	[SerializeField]
	private float weight = 5;

    private TextMeshPro textObjectName;


    [SerializeField]
	private bool isHeld;
	private bool isProjectile;

	protected GameObject HolderAgent
	{
		get { return holderAgent; }
		set { holderAgent = value; }
	}
	protected WeaponHand WeaponController
	{
		get { return weaponController; }
		set { weaponController = value; }
	}
	protected GameObject Handle
	{
		get { return handle; }
		set { handle = value; }
	}
	public float Damage
	{
		get { return damage; }
		set { damage = value; }
	}
	public float HitForce
	{
		get { return hitForce; }
		set { hitForce = value; }
	}
	public float ThrowDamage
	{
		get { return throwDamage; }
		set { throwDamage = value; }
	}
	public float ViewAngle
	{
		get { return viewAngle; }
		set { viewAngle = value; }
	}
	public float ViewDistance
	{
		get { return viewDistance; }
		set { viewDistance = value; }
	}
	public bool IsHeld
	{
		get { return isHeld; }
		set { isHeld = value; }
	}
	public int Durability
	{
		get { return durability; }
		set { durability = value; }
	}
	public float Weight
	{
		get { return weight; }
		set { weight = value; }
	}
	public Dictionary<StatusEffectType, GameObject> EffectList
	{
		get { return effects; }
		set { effects = value; }
	}

    private void Start()
    {

        textObjectName = gameObject.transform.parent.GetComponentInChildren<TextMeshPro>();
		if(textObjectName != null)
        {
			textObjectName.text = gameObject.transform.parent.name;
			textObjectName.faceColor = Color.white;
        }


    	if(particleEffectsPosition == null)//if no custom particleeffectsposition has been defined use the weapon-gameobject
        {
			particleEffectsPosition = gameObject;
        }

		AddStatusEffects(Effects_Editor);
    }

    protected virtual void Update()
    {
        bool showName = false;
        if (IsHeld && textObjectName != null)
        {
            showName = false;
            textObjectName.gameObject.SetActive(false);
        }
        else
        {
            for (int i = 0; i < PlayerManager.players.Count; i++)
            {
                if (Vector3.Distance(PlayerManager.players[i].transform.position,gameObject.transform.position) < 5)
                {
                    if (textObjectName != null)
                    {
                        textObjectName.gameObject.SetActive(true);
                        showName = true;
                        textObjectName.transform.rotation = new Quaternion(Quaternion.identity.x + 0.2f, Quaternion.identity.y, Quaternion.identity.z, Quaternion.identity.w);
                    }

                }
            }
        }
        if (!showName && textObjectName != null)
        {
            textObjectName.gameObject.SetActive(false);
        }
    }



	/// <summary>
	/// Used on startup and when loading a checkpoint.
	/// </summary>
	/// <param name="effects"></param>
	public virtual void AddStatusEffects(List<StatusEffectType> effects)
    {
		foreach (StatusEffectType type in effects)
		{
			AddStatusEffect(type);
		}
	}


	bool OnValidateDoneOnce = false;
	/// <summary>
	/// Called when value is changed. Used to add or remove status-effects from the weapon through the inspector while the game is running
	/// </summary>
    private void OnValidate()
    {
		if (!Application.isPlaying)
			return;
        if (!OnValidateDoneOnce) // just to avoid one nullrefeference exception that happens when this method is executed when this script is loaded.
        {
			OnValidateDoneOnce = true;
			return;
        }


		//Add effect if in Effects_Editor but not on the weapon yet
        foreach(StatusEffectType type in Effects_Editor)
        {
            if (!effects.ContainsKey(type))
            {
				AddStatusEffect(type);
            }
        }

		//Remove effect if on the weapon but not in Effects_Editor
		List<StatusEffectType> toRemove = new List<StatusEffectType>();
		foreach(KeyValuePair<StatusEffectType, GameObject> pair in effects)
        {
			if (!Effects_Editor.Contains(pair.Key))
            {
				toRemove.Add(pair.Key);
            }
        }

		foreach(StatusEffectType type in toRemove)
        {
			RemoveStatusEffect(type);
        }
    }

    /// <summary>
    /// Add effect to this weapon
    /// </summary>
    /// <param name="type"></param>
    public virtual void AddStatusEffect(StatusEffectType type)
    {
		if (effects.ContainsKey(type))
			return;


		GameObject particleEffect = null;

		switch (type)
		{
			case StatusEffectType.fire:
				particleEffect = GameManager.Instance.fireEffect;
				break;
			case StatusEffectType.poison:
				particleEffect = GameManager.Instance.poisonEffect;
				break;
			case StatusEffectType.frost:
				particleEffect = GameManager.Instance.frostEffect;
				break;
			case StatusEffectType.vulnerable:
				particleEffect = GameManager.Instance.vulnerableEffect;
				break;
			case StatusEffectType.hell_fire:
				particleEffect = GameManager.Instance.hellFireEffect;
				break;
			case StatusEffectType.hell_poison:
				particleEffect = GameManager.Instance.hellPoisonEffect;
				break;
			case StatusEffectType.hell_frost:
				particleEffect = GameManager.Instance.hellFrostEffect;
				break;
			default:
				return;
		}

		
		if(particleEffectsPosition == null)
        {
			particleEffectsPosition = gameObject;
        }

		GameObject instEffect = Instantiate(particleEffect, particleEffectsPosition.transform, false);
		instEffect.transform.localPosition = Vector3.zero;
		instEffect.transform.localScale *= 0.5f;
		
        
		effects.Add(type, instEffect);


#if UNITY_EDITOR
        if (!Effects_Editor.Contains(type))
        {
			Effects_Editor.Add(type);
        }
#endif
	}

	/// <summary>
	/// Remove status effect and particle-effect
	/// At the moment only used to remove effects when running in editor
	/// </summary>
	/// <param name="type"></param>
	public virtual void RemoveStatusEffect(StatusEffectType type)
    {
        if (effects.ContainsKey(type))
        {
			Destroy(effects[type]);
			effects.Remove(type);
        }

#if UNITY_EDITOR
		if (Effects_Editor.Contains(type))
        {
			Effects_Editor.Remove(type);
        }
#endif
	}


	public virtual void PickUpIn(GameObject hand)
	{
		holderAgent = hand.transform.parent.parent.gameObject;

		weaponController = holderAgent.GetComponent<WeaponHand>();

		isHeld = true;
		handle.GetComponent<Rigidbody>().isKinematic = true;
		GetComponent<Rigidbody>().isKinematic = true;

		handle.transform.parent = hand.transform;
		handle.transform.position = hand.transform.position;
		//handle.transform.rotation = hand.transform.rotation;
		handle.transform.localRotation = Quaternion.Euler(0, 0, 0); // Maybe fixes the wrong rotation of weapon at pick up
		Effects.ChangeWeight(hand.transform.parent.gameObject, weight);
		foreach (Collider collider in GetComponentsInChildren<Collider>())
		{
			collider.enabled = false;
		}
	}
	public void ReleaseThrow(float force)
	{
		Vector3 direction = holderAgent.transform.forward;
		Drop();
		if(force > 1)
		{
			GetComponentInChildren<Rigidbody>().AddForce(/*transform.up*/ direction * force, ForceMode.VelocityChange);
			isProjectile = true;
		}
	}

	public void Drop()
	{
		Effects.ChangeWeight(holderAgent, -weight);
		handle.transform.parent = GameObject.Find("Weapons").transform;
		handle.GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<Rigidbody>().isKinematic = false;
		isHeld = false;
		foreach (Collider collider in GetComponentsInChildren<Collider>())
		{
			collider.enabled = true;
		}
		holderAgent = null;

		Debug.Log("Weapon dropped");
	}

	public void Disarm(Vector3 velocity)
	{
		Drop();
		GetComponentInChildren<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
		isProjectile = true;
	}

	public virtual void SetAimGradient(Gradient gradient) { }
	public virtual void ToggleAim(bool isActive/*, GameObject FOVView*/) { }
	public virtual void StartAttack(Animator animator) { }
	public virtual void Attack(Animator animator)
	{
		if(GameManager.Instance.CurrentCombatState == CombatState.playerActions)
		{
			durability -= 1;
		}
        if (HolderAgent.GetComponent<AIController>())
        {
			durability -= 1;
		}

	}

	/// <summary>
	/// Method triggered by animation, Shoots projectile for ranged or deals damage for melee
	/// </summary>
	/// <param name="fov"></param>
	public virtual void DoAction(/*FieldOfView fov*/)
	{
		if (durability <= 0)
		{
            if (!HolderAgent.GetComponent<AIController>())
            {
				handle.GetComponentInParent<PlayerInputHandler>().RemoveObjectFromWeaponList(this.gameObject);
            }
			handle.GetComponentInParent<WeaponHand>().DropWeapon();

			Destroy(this.gameObject);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (isProjectile)
		{
			if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "NPC")
			{
				Effects.WeaponDamage(collision.gameObject, throwDamage, holderAgent);
				//Effects.Damage(collision.gameObject, throwDamage);
				Effects.ApplyWeaponEffects(collision.gameObject, holderAgent, Utilities.ListDictionaryKeys(effects));
			}
			isProjectile = false;
		}
		holderAgent = null;
	}
}
