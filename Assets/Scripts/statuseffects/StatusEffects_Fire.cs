using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static StatusEffectType;

class FireStatus : StatusEffect
{
    public const int StdDuration = 2;
    public const int Dmg = 20;

    public FireStatus(GameObject agent, GameObject applier):base(StatusEffectType.fire, StdDuration, agent, applier){}

    public override void Update()
    {
        base.Update();
        DealDamage(Dmg);
    }
}


class HellFireStatus : HellStatusEffect
{
    public const int StdDuration = 2;
    public const int Dmg = 35;

    public HellFireStatus(GameObject agent, GameObject applier, StatusEffectHandler handler) : base(StatusEffectType.hell_fire, StdDuration, agent, applier, handler){}

    public override void Update()
    {
        base.Update();
        DealDamage(Dmg);
    }

    public override void OnDeath()
    {
        base.OnDeath();

        //Explode
        Debug.Log("Hellfire explosion -BOOOOM-");
    }
}
