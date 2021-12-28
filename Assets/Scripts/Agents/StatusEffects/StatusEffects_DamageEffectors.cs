using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static StatusEffectType;

/// <summary>
/// This effect doesn't really do anything other than exist and count down
/// </summary>
class DamageBoostStatus : StatusEffect
{
    public const float Effect = .50f;//Percent extra damage done
    public const int StdDuration = 3;

    public DamageBoostStatus(GameObject agent, GameObject applier) : base(StatusEffectType.damage_boost, StdDuration, agent, applier)
    {
        comboWith = new Dictionary<StatusEffectType, StatusEffectType>();

        comboWith.Add(fire, comboAction);
        comboWith.Add(hell_fire, comboAction);
        comboWith.Add(ice, comboAction);
        comboWith.Add(hell_ice, comboAction);
        comboWith.Add(poison, comboAction);
        comboWith.Add(hell_poison, comboAction);
        comboWith.Add(vulnerable, glass_cannon);
    }
}


class VulnerableStatus : StatusEffect
{
    public const float Effect = .35f;//Percent extra damage done
    public const int StdDuration = 3;

    public VulnerableStatus(GameObject agent, GameObject applier) : base(StatusEffectType.vulnerable, StdDuration, agent, applier)
    {
        comboWith = new Dictionary<StatusEffectType, StatusEffectType>();
        comboWith.Add(fire, hell_fire);
        comboWith.Add(ice, hell_ice);
        comboWith.Add(poison, hell_poison);
        comboWith.Add(damage_boost, glass_cannon);
    }

}

/// <summary>
/// Doesn't combo with anything
/// </summary>
class GlassCannonStatus : StatusEffect
{
    public const float DmgBoostEffect = 1;//percent extra damage done (1 = 100% extra damage done that is)
    public const float VulnerableEffect = .7f;
    public const int StdDuration = 4;

    public GlassCannonStatus(GameObject agent, GameObject applier) : base(StatusEffectType.glass_cannon, StdDuration, agent, applier) {}
}