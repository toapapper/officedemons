using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static StatusEffectType;

class IceStatus : StatusEffect
{
    public const int StdDuration = 2;
    public const float SlowWeight = 20f;

    public IceStatus(GameObject agent) : base(StatusEffectType.ice, StdDuration, agent)
    {
        comboWith = new Dictionary<StatusEffectType, StatusEffectType>();
        comboWith.Add(fire, comboAction);
        comboWith.Add(hell_fire, comboAction);
        comboWith.Add(poison, paralysis);
        comboWith.Add(hell_poison, mega_paralysis);
        comboWith.Add(damage_boost, comboAction);
        comboWith.Add(vulnerable, hell_ice);
    }

    public override void OnApply()
    {
        base.OnApply();
        Effects.ChangeWeight(agent, SlowWeight);
    }

    public override void OnRemove()
    {
        base.OnRemove();
        Effects.ChangeWeight(agent, -SlowWeight);
    }
}

class HellIceStatus : HellStatusEffect
{
    public const int StdDuration = 2;
    public const float SlowWeight = 35f;
    public const int Dmg = 20;

    public HellIceStatus(GameObject agent, StatusEffectHandler handler) : base(StatusEffectType.hell_ice, StdDuration, agent, handler)
    {
        comboWith = new Dictionary<StatusEffectType, StatusEffectType>();
        comboWith.Add(fire, comboAction);
        comboWith.Add(hell_fire, comboAction);
        comboWith.Add(poison, mega_paralysis);
        comboWith.Add(hell_poison, comboAction);
        comboWith.Add(damage_boost, comboAction);
    }

    public override void Update()
    {
        base.Update();
        Effects.Damage(agent, Dmg);
    }

    public override void OnApply()
    {
        base.OnApply();
        Effects.ChangeWeight(agent, SlowWeight);
    }

    public override void OnRemove()
    {
        base.OnRemove();
        Effects.ChangeWeight(agent, -SlowWeight);
    }
}