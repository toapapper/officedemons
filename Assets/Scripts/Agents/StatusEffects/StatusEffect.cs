using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusEffectType
{
    none,
    fire,
    poison,
    ice,
    vulnerable,
    damage_boost,
    hell_fire,
    hell_poison,
    hell_ice,
    glass_cannon,
    paralysis,
    mega_paralysis,
    comboAction
}

public class WeaponEffectInfo
{
    public GameObject particleEffect;
    public int uses;

    public WeaponEffectInfo(GameObject particleEffect, int charges)
    {
        this.particleEffect = particleEffect;
        this.uses = charges;
    }
}


public abstract class StatusEffect
{
    public StatusEffectType type;
    public int duration;
    public int startDuration;

    protected GameObject agent;
    protected GameObject applier;
    protected Dictionary<StatusEffectType, StatusEffectType> comboWith = new Dictionary<StatusEffectType, StatusEffectType>();

    /// <summary>
    /// Base class for status effect
    /// </summary>
    /// <param name="type"></param>
    /// <param name="duration"></param>
    /// <param name="agent"> agent effect is applied to </param>
    /// <param name="applier"> agent that applied the effect </param>
    public StatusEffect(StatusEffectType type, int duration, GameObject agent, GameObject applier)
    {
        this.type = type;
        this.duration = duration;
        this.startDuration = duration;
        this.agent = agent;
        this.applier = applier;
    }

    public virtual void Update()
    {
        this.duration--;
    }

    /// <summary>
    /// Deal damage to attached agent.
    /// </summary>
    /// <param name="amount"></param>
    protected void DealDamage(float amount)
    {
        Effects.Damage(agent, amount, applier);
    }

    public virtual void ResetDuration()
    {
        this.duration = startDuration;
    }

    /// <summary>
    /// returns none by default. should be overridden in child classes
    /// </summary>
    /// <param name="effect"></param>
    /// <returns></returns>
    public virtual StatusEffectType ComboWith(StatusEffectType effect)
    {
        if (comboWith != null && comboWith.ContainsKey(effect))
        {
            return comboWith[effect];
        }
        else
        {
            return StatusEffectType.none;
        }
    }

    /// <summary> Called when the agent dies. </summary>
    public virtual void OnDeath() { }

    /// <summary> Called when this statuseffect is applied </summary>
    public virtual void OnApply() { }

    /// <summary> Called when this statuseffect is removed </summary>
    public virtual void OnRemove() { }
}


public class HellStatusEffect : StatusEffect
{
    StatusEffectHandler handler;

    public HellStatusEffect(StatusEffectType type, int duration, GameObject agent, GameObject applier, StatusEffectHandler handler) : base(type, duration, agent, applier)
    {
        this.handler = handler;
    }

    public override void OnDeath()
    {
        base.OnDeath();
        handler.Explode(type);
    }
}