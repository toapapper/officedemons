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

	[Tooltip("The Status effects this weapon has at spawn. Starts with standard amount of uses as well")]
	[SerializeField] protected List<StatusEffectType> EffectsAtSpawn;
	/// <summary> key = type, value = particleeffect and uses left. </summary>
    protected Dictionary<StatusEffectType, WeaponEffectInfo> effects = new Dictionary<StatusEffectType, WeaponEffectInfo>();
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
	public Dictionary<StatusEffectType, WeaponEffectInfo> EffectList
	{
		get { return effects; }
		set { effects = value; }
	}

    private void Start()
    {
        textObjectName = gameObject.transform.parent.GetComponentInChildren<TextMeshPro>();
        textObjectName.text = gameObject.transform.parent.name;
        textObjectName.faceColor = Color.white;


    	if(particleEffectsPosition == null)//if no custom particleeffectsposition has been defined use the weapon-gameobject
        {
			particleEffectsPosition = gameObject;
        }

		AddStatusEffects(EffectsAtSpawn);
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
                        textObjectName.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
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
	/// Called each attack to reduce the amount of charges left on the status effects and remove the ones with none.
	/// </summary>
	protected virtual void StatusEffectUpdate()
    {
		List<StatusEffectType> toRemove = new List<StatusEffectType>(2);

		foreach(KeyValuePair<StatusEffectType, WeaponEffectInfo> pair in effects)
        {
			effects[pair.Key].uses -= 1;

			if(pair.Value.uses <= 0) //charges == 0 therefore destroy particle effect and remove from list
            {
				toRemove.Add(pair.Key);
            }
        }

		foreach(StatusEffectType type in toRemove)
        {
			Destroy(effects[type].particleEffect);
			effects.Remove(type);
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


	/// <summary>
	/// Hell effects only get one use. all other get the same amount of uses that they have duration in turns when normally applied
	/// </summary>
	/// <param name="type"></param>
	public virtual void AddStatusEffect(StatusEffectType type)
    {
		GameObject particleEffect = null;
		int uses = 0;

		switch (type)
		{
			case StatusEffectType.fire:
				particleEffect = ParticleEffectContainer.fireEffect;
				uses = FireStatus.StdDuration;
				break;
			case StatusEffectType.poison:
				particleEffect = ParticleEffectContainer.poisonEffect;
				uses = PoisonStatus.StdDuration;
				break;
			case StatusEffectType.ice:
				particleEffect = ParticleEffectContainer.iceEffect;
				uses = IceStatus.StdDuration;
				break;
			case StatusEffectType.vulnerable:
				particleEffect = ParticleEffectContainer.vulnerableEffect;
				uses = VulnerableStatus.StdDuration;
				break;
			case StatusEffectType.damage_boost:
				particleEffect = ParticleEffectContainer.damageBoostEffect;
				uses = DamageBoostStatus.StdDuration;
				break;
			case StatusEffectType.glass_cannon:
				particleEffect = ParticleEffectContainer.glassCannonEffect;
				uses = GlassCannonStatus.StdDuration;
				break;
			case StatusEffectType.hell_fire:
				particleEffect = ParticleEffectContainer.hellFireEffect;
				uses = 1;
				break;
			case StatusEffectType.hell_poison:
				particleEffect = ParticleEffectContainer.hellPoisonEffect;
				uses = 1;
				break;
			case StatusEffectType.hell_ice:
				particleEffect = ParticleEffectContainer.hellIceEffect;
				uses = 1;
				break;
			case StatusEffectType.paralysis:
				particleEffect = ParticleEffectContainer.paralysisEffect;
				uses = ParalysisStatus.StdDuration;
				break;
			case StatusEffectType.mega_paralysis:
				particleEffect = ParticleEffectContainer.megaParalysisEffect;
				uses = MegaParalysisStatus.StdDuration;
				break;
		}

		if (effects.ContainsKey(type))//if already contains the statuseffect. simply reset its amount of uses.
		{
			effects[type].uses = uses;
		}
		else
		{
			effects.Add(type, new WeaponEffectInfo(particleEffect, uses));
			//instantiate particle effect as child of particleEffectsPosition and set weaponEffectsInfo to point to it.
			
			if(particleEffectsPosition == null)
            {
				particleEffectsPosition = gameObject;
            }

			GameObject instEffect = Instantiate(particleEffect, particleEffectsPosition.transform, false);
			instEffect.transform.localPosition = Vector3.zero;
			instEffect.transform.localScale *= 0.5f;
			effects[type].particleEffect = instEffect;
        }
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

		StatusEffectUpdate();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (isProjectile)
		{
			if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
			{
				Effects.WeaponDamage(collision.gameObject, throwDamage, holderAgent);
				//Effects.Damage(collision.gameObject, throwDamage);
				Effects.ApplyWeaponEffects(collision.gameObject, Utilities.ListDictionaryKeys(effects));
			}
			isProjectile = false;
		}
		holderAgent = null;
	}
}
