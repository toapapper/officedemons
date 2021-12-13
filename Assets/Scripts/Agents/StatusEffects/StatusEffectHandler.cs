using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using static StatusEffectType;

/// <summary>
/// <para>
/// A container and handler for all statuseffects applied to this Entity<br/>
/// Each status effect can stack up to 3 times, increasing the effect with each stack. For example 3 stacks of fire deals more damage than 2.<br/>
/// this file also contains the StatusEffectStruct, used to know which effects are active and such
/// </para>
///   
///  <para>
///  Author: Ossian
///  
/// </para>
///  
/// </summary>

// Last Edited: 09-12-21
public class StatusEffectHandler : MonoBehaviour
{
    public const float outOfCombatUpdateFrequency = 2f;
    private float oocUpdateTimer = outOfCombatUpdateFrequency;
    private Dictionary<StatusEffectType, StatusEffect> activeEffects;
    private Dictionary<StatusEffectType, GameObject> activeParticleEffects;

    public GameObject myAgent;

    [SerializeField] private Transform explosionPosition;

    [Header("Explosions")]
    [SerializeField] private GameObject hellFireExplosion;
    [SerializeField] private GameObject hellPoisonExplosion;
    [SerializeField] private GameObject hellIceExplosion;

    [SerializeField] private GameObject firePoisonExplosion;
    [SerializeField] private GameObject hellFirePoisonExplosion;
    [SerializeField] private GameObject firePoisonNuke;
    [SerializeField] private GameObject steamExplosion;
    [SerializeField] private GameObject biggerSteamExplosion;
    [SerializeField] private GameObject steamNuke;
    [SerializeField] private GameObject paralysisNuke;


    #region properties
    /// <summary> returns a dictionary where </summary>
    public Dictionary<StatusEffectType, StatusEffect> ActiveEffects { get { return activeEffects; }}

    ///<summary>true if paralyzed</summary>
    public bool Paralyzed { get { return activeEffects.ContainsKey(StatusEffectType.paralysis); }}

    /// <summary> returns the modifier to the damage this entity should take </summary>
    public float Vulnerability{ 
        get 
        {
            float value = 0;

            if (activeEffects.ContainsKey(StatusEffectType.vulnerable)){
                value += VulnerableStatus.Effect;
            }
            
            if (activeEffects.ContainsKey(StatusEffectType.glass_cannon))
            {
                value += GlassCannonStatus.VulnerableEffect;
            }
            return value;
        }
    }

    /// <summary> returns the modifier to the damage this entity should make </summary>
    public float DmgBoost { 
        get 
        {
            float value = 0;

            if (activeEffects.ContainsKey(StatusEffectType.damage_boost))
            {
                value += DamageBoostStatus.Effect;
            }

            if (activeEffects.ContainsKey(StatusEffectType.glass_cannon))
            {
                value += GlassCannonStatus.DmgBoostEffect;
            }

            return value;
        }
    }
    #endregion

    void Awake()
    {
        activeEffects = new Dictionary<StatusEffectType, StatusEffect>();
        activeParticleEffects = new Dictionary<StatusEffectType, GameObject>();
    }

    private void Update()
    {
        if(GameManager.Instance.CurrentCombatState == CombatState.none)
        {
            oocUpdateTimer -= Time.deltaTime;
            if(oocUpdateTimer <= 0)
            {
                UpdateEffects();
                oocUpdateTimer = outOfCombatUpdateFrequency;
            }
        }
    }

    /// <summary>
    /// Applies the selected amount of stacks of the selected effect.
    /// </summary>
    /// <param name="effectType"></param>
    /// <param name="duration">This duration overrides the existing duration if it is greater than it. otherwise the old one is used, the stacks are however still applied</param>
    /// <param name="stacks"></param>
    public void ApplyEffect(StatusEffectType effectType)
    {
        if (activeEffects.Count == 2 && !activeEffects.ContainsKey(effectType))//too many. remove one before doing anything else.
        {
            RemoveEffect(activeEffects.ElementAt(0).Key);
        }

        if (activeEffects.ContainsKey(effectType))//already exists -> Reset duration
        {
            StatusEffect aEffect = activeEffects[effectType];
            aEffect.ResetDuration();
        }
        else if(activeEffects.Count == 1)//already exists one other -> check if combos
        {
            StatusEffect currentEffect = activeEffects.ElementAt(0).Value;
            StatusEffectType comboType = currentEffect.ComboWith(effectType);

            if(comboType == StatusEffectType.none) //no combo. both can be active at the same time
            {
                AddEffect(effectType);
            }
            else if(comboType == StatusEffectType.comboAction) //do comboaction (mostly explosions and moving to weapon)
            {
                RemoveEffect(currentEffect.type);
                PerformComboAction(currentEffect.type, effectType);
            }
            else//there is a combo. Therefore remove the existing one and add the combined one
            {
                RemoveEffect(currentEffect.type);
                AddEffect(comboType);
            }
        }
        else //otherwise just add the new effect
        {
            AddEffect(effectType);
        }
    }

    /// <summary>
    /// no questions asked. just applies the effect. only to be used internally
    /// </summary>
    /// <param name="effectType"></param>
    private void AddEffect(StatusEffectType effectType)
    {
        StatusEffect effect = null;
        GameObject particleEffect = null;
        switch (effectType)
        {
            case fire:
                effect = new FireStatus(myAgent);
                particleEffect = ParticleEffectContainer.fireEffect;
                break;
            case poison:
                effect = new PoisonStatus(myAgent);
                particleEffect = ParticleEffectContainer.poisonEffect;
                break;
            case ice:
                effect = new IceStatus(myAgent);
                particleEffect = ParticleEffectContainer.iceEffect;
                break;
            case damage_boost:
                effect = new DamageBoostStatus(myAgent);
                particleEffect = ParticleEffectContainer.damageBoostEffect;
                break;
            case vulnerable:
                effect = new VulnerableStatus(myAgent);
                particleEffect = ParticleEffectContainer.vulnerableEffect;
                break;
            case hell_fire:
                effect = new HellFireStatus(myAgent, this);
                particleEffect = ParticleEffectContainer.hellFireEffect;
                break;
            case hell_poison:
                effect = new HellPoisonStatus(myAgent, this);
                particleEffect = ParticleEffectContainer.hellPoisonEffect;
                break;
            case hell_ice:
                effect = new HellIceStatus(myAgent, this);
                particleEffect = ParticleEffectContainer.hellIceEffect;
                break;
            case paralysis:
                effect = new ParalysisStatus(myAgent);
                particleEffect = ParticleEffectContainer.paralysisEffect;
                break;
            case mega_paralysis:
                effect = new MegaParalysisStatus(myAgent);
                particleEffect = ParticleEffectContainer.megaParalysisEffect;
                break;
            case glass_cannon:
                effect = new GlassCannonStatus(myAgent);
                particleEffect = ParticleEffectContainer.glassCannonEffect;
                break;
        }

        Debug.Log("Adding particleeffect: " + effectType + " + instance: " + effect);

        effect.OnApply();
        activeEffects.Add(effectType, effect);

        particleEffect = Instantiate(particleEffect, transform);
        particleEffect.transform.localPosition = Vector3.zero;
        activeParticleEffects.Add(effectType, particleEffect);
    }

    public void RemoveEffect(StatusEffectType type)
    {
        if (activeEffects.ContainsKey(type))
        {
            activeEffects[type].OnRemove();
            activeEffects.Remove(type);
        }

        if (activeParticleEffects.ContainsKey(type))
        {
            Destroy(activeParticleEffects[type]);
            activeParticleEffects.Remove(type);
        }
    }

    public void ClearEffects()
    {
        for (int i = 0; i < activeEffects.Count; i++)
        {
            RemoveEffect(activeEffects.ElementAt(i).Key);
            i--;
        }
    }

    /// <summary>
    /// Workaround to allow the non-monobehaviour statuseffects to instantiate an explosion. Used only by the hell-effects as of yet
    /// </summary>
    /// <param name="type"></param>
    public void Explode(StatusEffectType type)
    {
        switch (type)
        {
            case hell_fire:
                Explode(hellFireExplosion);
                break;
            case hell_poison:
                Explode(hellPoisonExplosion);
                break;
            case hell_ice:
                Explode(hellIceExplosion);
                break;
        }
    }

    /// <summary>
    /// basically just instantiates <paramref name="explosion"/> at explosionPosition. Should not pass anything that isn't a self terminating explosionthing
    /// </summary>
    /// <param name="explosion"></param>
    public void Explode(GameObject explosion)
    {
        Instantiate(explosion, explosionPosition.position, Quaternion.identity);
    }

    /// <summary>
    /// do the explosions here. The status effects know by themselves if they should explode or become something else.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    private void PerformComboAction(StatusEffectType left, StatusEffectType right)
    {
        Debug.Log("Perform combo");

        if((int)left > (int)right)//Sort by size. means i dont have to take different permutations into account in the else if statements.
        {
            StatusEffectType temp = left;
            left = right;
            right = temp;
        }
        //Must however make sure i order the items correctly

        #region fire combos
        if(left == fire && right == poison)
        {
            Explode(firePoisonExplosion);
        }
        else if (left == fire && right == ice)
        {
            Explode(steamExplosion);
        }
        else if(left == fire && right == hell_poison)
        {
            Explode(hellFirePoisonExplosion);
        }
        else if(left == fire && right == hell_ice)
        {
            Explode(biggerSteamExplosion);
        }
        #endregion

        #region ice combos
        else if(left == ice && right == hell_fire)
        {
            Explode(biggerSteamExplosion);
        }
        #endregion

        #region poison combos
        else if(left == poison && right == hell_fire)
        {
            Explode(hellFirePoisonExplosion);
        }
        #endregion

        #region hell-combos
        else if(left == hell_fire && right == hell_ice)
        {
            Explode(steamNuke);
        }
        else if(left == hell_fire && right == hell_poison)
        {
            Explode(firePoisonNuke);
        }
        else if(left == hell_poison && right == hell_ice)
        {
            Explode(paralysisNuke);
        }
        #endregion

        #region damage boosts(move to weapon-combos)
        else if(left == fire && right == damage_boost)
        {
            MoveEffectToWeapon(left);
        }
        else if(left == ice && right == damage_boost)
        {
            MoveEffectToWeapon(left);
        }
        else if(left == poison && right == damage_boost)
        {
            MoveEffectToWeapon(left);
        }
        else if(left == damage_boost && right == hell_fire)
        {
            MoveEffectToWeapon(right);
        }
        else if(left == damage_boost && right == hell_ice)
        {
            MoveEffectToWeapon(right);
        }
        else if(left == damage_boost && right == hell_poison)
        {
            MoveEffectToWeapon(right);
        }

        #endregion
    }

    protected void MoveEffectToWeapon(StatusEffectType type)
    {
        myAgent.GetComponent<WeaponHand>().objectInHand.AddStatusEffect(type);
    }

    /// <summary>
    /// Is called on death
    /// </summary>
    public void OnDeath()
    {
        for(int i = 0; i < activeEffects.Count; i++)
        {
            KeyValuePair<StatusEffectType, StatusEffect> entry = activeEffects.ElementAt(i);

            entry.Value.OnDeath();
            RemoveEffect(entry.Key);
            i--;
        }
    }

    /// <summary>
    /// Calls effect.Update() and afterwards if its duration is 0 removes it immediately.
    /// </summary>
    public void UpdateEffects()
    {
        for(int i = 0; i < activeEffects.Count; i++)
        {
            KeyValuePair<StatusEffectType, StatusEffect> entry = activeEffects.ElementAt(i);

            entry.Value.Update();
            if(entry.Value.duration <= 0)
            {
                RemoveEffect(entry.Key);
                i--;
            }
        }
    }
}

