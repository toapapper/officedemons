using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using static StatusEffectType;

/// <summary>
/// <para>
/// A container and handler for all statuseffects applied to this agent<br/>
/// </para>
///   
///  <para>
///  Author: Ossian
///  
/// </para>
///  
/// </summary>

// Last Edited: 05-03-22
public class StatusEffectHandler : MonoBehaviour
{
    public const float outOfCombatUpdateFrequency = 2f;
    private float oocUpdateTimer = outOfCombatUpdateFrequency;
    private Dictionary<StatusEffectType, StatusEffect> activeEffects;
    private Dictionary<StatusEffectType, GameObject> activeParticleEffects;

    public GameObject myAgent;

    [SerializeField] private Transform explosionPosition;

    [Header("Explosions")]
    [SerializeField] private GameObject hellFireExplosion;
    [SerializeField] private GameObject hellPoisonExplosion;
    [SerializeField] private GameObject hellIceExplosion;
    

    #region properties
    /// <summary> returns a dictionary where </summary>
    public Dictionary<StatusEffectType, StatusEffect> ActiveEffects { get { return activeEffects; }}

    /// <summary> returns the modifier to the damage this entity should take </summary>
    public float Vulnerability{ 
        get 
        {
            return VulnerableStatus.Effect;
        }
    }

    #endregion

    void Awake()
    {
        activeEffects = new Dictionary<StatusEffectType, StatusEffect>();
        activeParticleEffects = new Dictionary<StatusEffectType, GameObject>();
    }

    private void Update()
    {
        if(GameManager.Instance.CurrentCombatState == CombatState.none)
        {
            oocUpdateTimer -= Time.deltaTime;
            if(oocUpdateTimer <= 0)
            {
                UpdateEffects();
                oocUpdateTimer = outOfCombatUpdateFrequency;
            }
        }
    }

    /// <summary>
    /// Applies the selected amount of stacks of the selected effect.
    /// </summary>
    /// <param name="effectType"></param>
    /// <param name="duration">This duration overrides the existing duration if it is greater than it. otherwise the old one is used, the stacks are however still applied</param>
    /// <param name="stacks"></param>
    public void ApplyEffect(StatusEffectType effectType, GameObject applier)
    {
        if (activeEffects.ContainsKey(effectType))//already exists -> Reset duration
        {
            activeEffects[effectType].ResetDuration();
        }
        else //otherwise check if there is some interaction
        {
            //Sorry for monster switch-case. Checks for interactions. ice removes fire. Only Hell ice can remove hell fire etc.. Defined in Game design document. section 5.4.1
            switch (effectType)
            {
                case fire:
                    if (activeEffects.ContainsKey(hell_fire))
                    {
                        activeEffects[hell_fire].ResetDuration();
                        break;
                    }

                    if (activeEffects.ContainsKey(hell_frost))
                        break;
                    
                    RemoveEffect(frost);
                    AddEffect(effectType, applier);
                    break;

                case frost:
                    if (activeEffects.ContainsKey(hell_frost))
                    {
                        activeEffects[hell_frost].ResetDuration();
                        break;
                    }

                    if (activeEffects.ContainsKey(hell_fire))
                        break;
                    
                    RemoveEffect(fire);
                    AddEffect(effectType, applier);
                    break;

                case poison:
                    if (activeEffects.ContainsKey(hell_poison))
                    {
                        activeEffects[hell_poison].ResetDuration();
                        break;
                    }

                    AddEffect(effectType, applier);
                    break;

                case hell_fire:
                    RemoveEffect(fire);
                    RemoveEffect(frost);
                    RemoveEffect(hell_frost);

                    AddEffect(effectType, applier);
                    break;

                case hell_frost:
                    RemoveEffect(frost);
                    RemoveEffect(fire);
                    RemoveEffect(hell_fire);

                    AddEffect(effectType, applier);
                    break;

                case hell_poison:
                    RemoveEffect(poison);
                    AddEffect(effectType, applier);
                    break;

                case vulnerable:
                    AddEffect(effectType, applier);
                    break;
            }
        }
    }




    /// <summary>
    /// no questions asked. just applies the effect. only to be used internally
    /// </summary>
    /// <param name="effectType"></param>
    private void AddEffect(StatusEffectType effectType, GameObject applier)
    {
        StatusEffect effect = null;
        GameObject particleEffect = null;
        switch (effectType)
        {
            case fire:
                effect = new FireStatus(myAgent, applier);
                particleEffect = GameManager.Instance.fireEffect;
                break;
            case frost:
                effect = new IceStatus(myAgent, applier);
                particleEffect = GameManager.Instance.frostEffect;
                break;
            case poison:
                effect = new PoisonStatus(myAgent, applier);
                particleEffect = GameManager.Instance.poisonEffect;
                break;
            case hell_fire:
                effect = new HellFireStatus(myAgent, applier, this);
                particleEffect = GameManager.Instance.hellFireEffect;
                break;
            case hell_frost:
                effect = new HellIceStatus(myAgent, applier, this);
                particleEffect = GameManager.Instance.hellFrostEffect;
                break;
            case hell_poison:
                effect = new HellPoisonStatus(myAgent, applier, this);
                particleEffect = GameManager.Instance.hellPoisonEffect;
                break;
            case vulnerable:
                effect = new VulnerableStatus(myAgent, applier);
                particleEffect = GameManager.Instance.vulnerableEffect;
                break;
            default:
                return;
        }

        effect.OnApply();
        activeEffects.Add(effectType, effect);

        particleEffect = Instantiate(particleEffect, transform);
        particleEffect.transform.localPosition = Vector3.zero;
        activeParticleEffects.Add(effectType, particleEffect);
    }

    /// <summary>
    /// Removes the status-effect
    /// </summary>
    /// <param name="type"></param>
    public void RemoveEffect(StatusEffectType type)
    {
        if (activeEffects.ContainsKey(type))
        {
            activeEffects[type].OnRemove();
            activeEffects.Remove(type);
        }

        if (activeParticleEffects.ContainsKey(type))
        {
            Destroy(activeParticleEffects[type]);
            activeParticleEffects.Remove(type);
        }
    }

    public void ClearEffects()
    {
        for (int i = 0; i < activeEffects.Count; i++)
        {
            RemoveEffect(activeEffects.ElementAt(i).Key);
            i--;
        }
    }

    /// <summary>
    /// Workaround to allow the non-monobehaviour statuseffects to instantiate an explosion. Used only by the hell-effects as of yet
    /// </summary>
    /// <param name="type"></param>
    public void Explode(StatusEffectType type)
    {
        switch (type)
        {
            case hell_fire:
                Explode(hellFireExplosion);
                break;
            case hell_poison:
                Explode(hellPoisonExplosion);
                break;
            case hell_frost:
                Explode(hellIceExplosion);
                break;
        }
    }

    /// <summary>
    /// Instantiates the gameobject <paramref name="explosion"/> at explosionPosition
    /// </summary>
    /// <param name="explosion"></param>
    public void Explode(GameObject explosion)
    {
        Instantiate(explosion, explosionPosition.position, Quaternion.identity);
    }
    
    /// <summary>
    /// Is called on death
    /// </summary>
    public void OnDeath()
    {
        for(int i = 0; i < activeEffects.Count; i++)
        {
            KeyValuePair<StatusEffectType, StatusEffect> entry = activeEffects.ElementAt(i);

            entry.Value.OnDeath();
            RemoveEffect(entry.Key);
            i--;
        }

        ClearEffects();
    }

    /// <summary>
    /// Calls effect.Update() and afterwards if its duration is 0 removes.
    /// </summary>
    public void UpdateEffects()
    {
        for(int i = 0; i < activeEffects.Count; i++)
        {
            KeyValuePair<StatusEffectType, StatusEffect> entry = activeEffects.ElementAt(i);

            entry.Value.Update();
            if(entry.Value.duration <= 0)
            {
                RemoveEffect(entry.Key);
                i--;
            }
        }
    }
}

