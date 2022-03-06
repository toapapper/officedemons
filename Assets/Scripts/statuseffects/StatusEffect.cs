using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusEffectType
{
    none,
    fire,
    frost,
    poison,
    hell_fire,
    hell_frost,
    hell_poison,
    vulnerable
}

public abstract class StatusEffect
{
    public StatusEffectType type;
    public int duration;
    public int startDuration;

    protected GameObject agent;
    protected GameObject applier;

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


    /// <summary> Called when the agent dies. </summary>
    public virtual void OnDeath() { }

    /// <summary> Called when this statuseffect is applied </summary>
    public virtual void OnApply() { }

    /// <summary> Called when this statuseffect is removed </summary>
    public virtual void OnRemove() { }
}

/// <summary>
/// Base class for HellEffect. explodes on death. Therefor needs a reference to the Statuseffecthandler
/// </summary>
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