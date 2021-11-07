using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusEffectType
{
    Fire,           //simple damage over time
    Bleed,          //damage over time with stamina drain
    Poison,         //damage over time with accuracy loss(?)
    StaminaDrain,   //lose stamina att start of turn.
    Vulnerable,     //Take more damage for a duration
    HealOverTime,   //heal at start of turn
    StaminaFill,    //get extra stamina at start of turn.
    Paralyze,       //Unable to do anything at all.         ----------------NOT CERTAIN IT IS IMPLEMENTED ALRIGHT--------------------------
    DamageBoost,    //deal more damage for a duration
    Slow,
    NumberOfTypes
}

public enum EffectDurations
{
    Short = 1,
    Medium = 3,
    Long = 5
}

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

// Last Edited: 28-10-21



/*
 * TODO:
 * Fixa SLOW så att den gör en effects.slow  när den applieas och tar bort det när den försvinner
 */

public class StatusEffectHandler : MonoBehaviour
{
    #region const stats
    //these are the stats used for the different effects' damage or drains and such.
    private const int fireDamage = 15;
    private const int bleedDamage = 10;
    private const float bleedDrain = .5f;//stam drain, per stack
    private const int poisonDamage = 10;
    public const float poisonAccuracyMod = 5;//Degrees additional spread by the weapon
    private const float staminaDrain = .8f;
    private const float vulnerableMod = .3f;//percent increased damage per stack
    private const int healOverTime = 20;
    private const float staminaFill = .8f;
    private const float damageBoost = .3f; //percent increased damage per stack
    private const float slowWeight = 20f; // percent slow per slow ----------- NOT IMPLEMENTED ------------ IMPLEMENT ON APPLY AND REMOVE, not doing anything otherwise
    #endregion


    private Dictionary<StatusEffectType, StatusEffect> activeEffects;

    /// <summary> returns a dictionary where </summary>
    public Dictionary<StatusEffectType, StatusEffect> ActiveEffects { get { return activeEffects; }}

    ///<summary>true if paralyzed</summary>
    public bool Paralyzed { get { return activeEffects.ContainsKey(StatusEffectType.Paralyze); }}

    /// <summary> returns the modifier to the damage this entity should take </summary>
    public float Vulnerability{ get { return activeEffects.ContainsKey(StatusEffectType.Vulnerable) ? activeEffects[StatusEffectType.Vulnerable].stacks * vulnerableMod : 0; }}

    /// <summary> returns the modifier to the damage this entity should make </summary>
    public float DmgBoost { get { return activeEffects.ContainsKey(StatusEffectType.DamageBoost) ? activeEffects[StatusEffectType.DamageBoost].stacks * damageBoost : 0; } }
    
    public float InAccuracyMod
    {
        get
        {
            return activeEffects.ContainsKey(StatusEffectType.Poison) ? activeEffects[StatusEffectType.Poison].stacks * poisonAccuracyMod : 0;
        }
    }

    void Start()
    {
        activeEffects = new Dictionary<StatusEffectType, StatusEffect>();
    }

    /// <summary>
    /// Applies the selected amount of stacks of the selected effect.
    /// </summary>
    /// <param name="effect"></param>
    /// <param name="duration">This duration overrides the existing duration if it is greater than it. otherwise the old one is used, the stacks are however still applied</param>
    /// <param name="stacks"></param>
    public void ApplyEffect(StatusEffectType effect, int duration, int stacks = 1)
    {
        if (activeEffects.ContainsKey(effect))//add stack to existing
        {
            StatusEffect sEffect = activeEffects[effect];
            sEffect.stacks += stacks;
            sEffect.stacks = Mathf.Clamp(sEffect.stacks, 0, 3);
            sEffect.duration = Mathf.Max(duration, sEffect.duration);

            if(effect == StatusEffectType.Slow)
            {
                Effects.ChangeWeight(gameObject, slowWeight);
            }
        }
        else //create a new effect
        {
            Debug.Log("New effect added" + effect);

            int dmg = 0;
            float drain = 0;

            switch (effect)
            {
                case StatusEffectType.Fire:
                    dmg = fireDamage;
                    break;
                case StatusEffectType.Bleed:
                    dmg = bleedDamage;
                    drain = bleedDrain;
                    break;
                case StatusEffectType.Poison:
                    dmg = poisonDamage;
                    break;
                case StatusEffectType.StaminaDrain:
                    drain = staminaDrain;
                    break;
                case StatusEffectType.HealOverTime:
                    dmg = -healOverTime;
                    break;
                case StatusEffectType.StaminaFill:
                    drain = -staminaFill;
                    break;
                case StatusEffectType.Slow:
                    Effects.ChangeWeight(gameObject, slowWeight);
                    break;
            }

            StatusEffect sEffect = new StatusEffect(effect, duration, dmg, drain, stacks);
            activeEffects.Add(effect, sEffect);
        }
    }

    /// <summary>
    /// Applies the effects of the active effects. For example deals damage if on fire. Also decreases their duration by one and removes them if duration is zero
    /// </summary>
    public void UpdateEffects()
    {
        for(int i = 0; i < (int)StatusEffectType.NumberOfTypes; i++)
        {
            StatusEffectType si = (StatusEffectType)i;

            if (activeEffects.ContainsKey(si))
            {
                if(activeEffects[si].duration <= 0)
                {
                    if(si == StatusEffectType.Slow)
                    {
                        Effects.ChangeWeight(gameObject, activeEffects[si].stacks * -slowWeight);
                    }

                    activeEffects.Remove(si);
                }
                else
                {
                    activeEffects[(StatusEffectType)i].Update(this.gameObject);
                }
            }

        }
    }
}


/// <summary>
/// A generic struct containing the information a status effect would need. You pretty much construct your own status effect on constructing it.
/// </summary>
public class StatusEffect
{
    public StatusEffectType type;
    public int stacks;
    public int duration;
    public int damage;
    public float stamDrain;

    public StatusEffect(StatusEffectType type, int duration, int damage, float stamDrain, int stacks)
    {
        this.type = type;
        this.stacks = stacks;
        this.duration = duration;
        this.damage = damage;
        this.stamDrain = stamDrain;
    }

    public void Update(GameObject gameObject)
    {
        Effects.Damage(gameObject, damage * stacks);
        Effects.DrainStamina(gameObject, stamDrain * stacks);

        this.duration--;
    }
}