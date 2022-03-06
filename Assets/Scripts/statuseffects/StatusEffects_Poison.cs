using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using static StatusEffectType;


class PoisonStatus : StatusEffect
{
    public const int StdDuration = 4;
    public const int Dmg = 12;

    public PoisonStatus(GameObject agent, GameObject applier) : base(StatusEffectType.poison, StdDuration, agent, applier) {}

    public override void Update()
    {
        base.Update();

        DealDamage(Dmg);
    }
}


class HellPoisonStatus : HellStatusEffect
{
    public const int StdDuration = 4;
    public const int Dmg = 25;

    public HellPoisonStatus(GameObject agent, GameObject applier, StatusEffectHandler handler) : base(StatusEffectType.hell_poison, StdDuration, agent, applier, handler){}

    public override void Update()
    {
        base.Update();
        DealDamage(Dmg);
    }
}