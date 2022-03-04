using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static StatusEffectType;



class VulnerableStatus : StatusEffect
{
    public const float Effect = .35f;//Percent extra damage done
    public const int StdDuration = 3;

    public VulnerableStatus(GameObject agent, GameObject applier) : base(StatusEffectType.vulnerable, StdDuration, agent, applier) {}

}
