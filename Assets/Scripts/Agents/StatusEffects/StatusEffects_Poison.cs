using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using static StatusEffectType;


//TODO: lägg till combowith-metoderna.



class PoisonStatus : StatusEffect
{
    public const int StdDuration = 4;
    public const int Dmg = 12;

    public PoisonStatus(GameObject agent, GameObject applier) : base(StatusEffectType.poison, StdDuration, agent, applier) 
    {
        comboWith = new Dictionary<StatusEffectType, StatusEffectType>();
        comboWith.Add(fire, comboAction);
        comboWith.Add(hell_ice, comboAction);
        comboWith.Add(damage_boost, comboAction);
        comboWith.Add(ice, paralysis);
        comboWith.Add(vulnerable, hell_poison);
    }

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

    public HellPoisonStatus(GameObject agent, GameObject applier, StatusEffectHandler handler) : base(StatusEffectType.hell_poison, StdDuration, agent, applier, handler)
    {
        comboWith = new Dictionary<StatusEffectType, StatusEffectType>();
        comboWith.Add(fire, comboAction);
        comboWith.Add(hell_fire, comboAction);
        comboWith.Add(hell_ice, comboAction);
        comboWith.Add(damage_boost, comboAction);
        comboWith.Add(ice, mega_paralysis);
    }

    public override void Update()
    {
        base.Update();

        DealDamage(Dmg);
    }

    public override void OnDeath()
    {
        base.OnDeath();

        //TODO: Implement explosion
        Debug.Log("HellPoisonExplosion -(poisony)SWOOSHBOOM-");
    }
}

class ParalysisStatus : StatusEffect
{
    public const int StdDuration = 1;

    public ParalysisStatus(GameObject agent, GameObject applier) : base(StatusEffectType.paralysis, StdDuration, agent, applier)
    {
        comboWith = new Dictionary<StatusEffectType, StatusEffectType>();
        comboWith.Add(damage_boost, comboAction);
    }
}

class MegaParalysisStatus : StatusEffect
{
    public const int StdDuration = 3;
    public const int Dmg = 25;

    public MegaParalysisStatus(GameObject agent, GameObject applier): base(StatusEffectType.mega_paralysis, StdDuration, agent, applier)
    {
        comboWith = new Dictionary<StatusEffectType, StatusEffectType>();
        comboWith.Add(damage_boost, comboAction);
    }

    public override void Update()
    {
        base.Update();

        DealDamage(Dmg);
    }
}