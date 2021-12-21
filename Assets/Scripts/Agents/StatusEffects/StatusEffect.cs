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
    protected Dictionary<StatusEffectType, StatusEffectType> comboWith;

    public StatusEffect(StatusEffectType type, int duration, GameObject agent)
    {
        this.type = type;
        this.duration = duration;
        this.startDuration = duration;
        this.agent = agent;
    }

    public virtual void Update()
    {
        this.duration--;
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

    public HellStatusEffect(StatusEffectType type, int duration, GameObject agent, StatusEffectHandler handler) : base(type, duration, agent)
    {
        this.handler = handler;
    }

    public override void OnDeath()
    {
        base.OnDeath();
        handler.Explode(type);
    }
}