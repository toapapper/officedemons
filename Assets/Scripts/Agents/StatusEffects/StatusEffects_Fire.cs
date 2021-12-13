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

    public FireStatus(GameObject agent):base(StatusEffectType.fire, StdDuration, agent)
    {
        comboWith = new Dictionary<StatusEffectType, StatusEffectType>();
        comboWith.Add(ice, comboAction);
        comboWith.Add(hell_ice, comboAction);
        comboWith.Add(poison, comboAction);
        comboWith.Add(hell_poison, comboAction);
        comboWith.Add(damage_boost, comboAction);
        comboWith.Add(vulnerable, hell_fire);
    }

    public override void Update()
    {
        base.Update();
        Effects.Damage(agent, Dmg);
    }
}


class HellFireStatus : HellStatusEffect
{
    public const int StdDuration = 2;
    public const int Dmg = 35;

    public HellFireStatus(GameObject agent, StatusEffectHandler handler) : base(StatusEffectType.hell_fire, StdDuration, agent, handler)
    {
        comboWith = new Dictionary<StatusEffectType, StatusEffectType>();
        comboWith.Add(ice, comboAction);
        comboWith.Add(hell_ice, comboAction);
        comboWith.Add(poison, comboAction);
        comboWith.Add(hell_poison, comboAction);
        comboWith.Add(damage_boost, comboAction);
    }

    public override void Update()
    {
        base.Update();
        Effects.Damage(agent, Dmg);
    }

    public override void OnDeath()
    {
        base.OnDeath();

        //Explode
        Debug.Log("Hellfire explosion -BOOOOM-");
    }
}
