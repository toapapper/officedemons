using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static StatusEffectType;

class IceStatus : StatusEffect
{
    public const int StdDuration = 3;
    public const float SlowWeight = 40f;

    public IceStatus(GameObject agent, GameObject applier) : base(StatusEffectType.frost, StdDuration, agent, applier){}

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
    public const int StdDuration = 4;
    public const float SlowWeight = 50f;
    public const int Dmg = 15;

    public HellIceStatus(GameObject agent, GameObject applier, StatusEffectHandler handler) : base(StatusEffectType.hell_frost, StdDuration, agent, applier, handler){}

    public override void Update()
    {
        base.Update();
        DealDamage(Dmg);
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