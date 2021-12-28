using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Abstract class for all ground effect objects
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15-11-19
public abstract class GroundEffectObject : MonoBehaviour
{
    public List<GameObject> agentsOnGroundEffect = new List<GameObject>();
    [SerializeField]
    protected int totalDurabilityTurns = 3;
    [SerializeField]
    protected float totalDurabilityTime = 30f;

    [SerializeField] protected List<StatusEffectType> effects;

    protected int durabilityTurns;
    protected float durabilityTime;

    public GameObject creatorAgent;

    private void Start()
    {
        this.durabilityTime = totalDurabilityTime;
        this.durabilityTurns = totalDurabilityTurns;
    }

    public void ApplyEffectsOnEnemys()
    {
        foreach (GameObject agent in agentsOnGroundEffect)
        {
            if (agent.tag == "Enemy" && agent.name != "tank")
            {
                ApplyEffectsOn(agent);
            }
        }
    }
    public void ApplyEffectsOnPlayers()
    {
        foreach (GameObject agent in agentsOnGroundEffect)
        {
            if (agent.tag == "Player")
            {
                ApplyEffectsOn(agent);
            }
        }

        durabilityTurns--;
        if (durabilityTurns <= 0)
        {
            RemoveGroundEffectObject();
        }
    }
    protected virtual void ApplyEffectsOn(GameObject agent)
    {
        Effects.ApplyWeaponEffects(agent, creatorAgent, effects);
    }

    public void UpdateTime()
    {
        durabilityTime -= Time.deltaTime;
        if (durabilityTime <= 0)
        {
            RemoveGroundEffectObject();
        }
    }

    private void RemoveGroundEffectObject()
    {
        GameManager.Instance.GroundEffectObjects.Remove(gameObject);
        Destroy(gameObject);
    }

    //Just apply effects on everyone when any of these are called. why not?
    protected virtual void OnTriggerEnter(Collider other)
    {
        //Only do things to enemies and players and non-triggers.
        if(other.tag != "Enemy" || other.tag != "Player" || other.isTrigger)
        {
            return;
        }

        if (!agentsOnGroundEffect.Contains(other.gameObject) && other.gameObject.GetComponent<Attributes>().Health > 0)
        {
            AkSoundEngine.PostEvent("Play_Sizzle", gameObject);
            agentsOnGroundEffect.Add(other.gameObject);
        }

        foreach(GameObject agent in agentsOnGroundEffect)
        {
            ApplyEffectsOn(agent);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (agentsOnGroundEffect.Contains(other.gameObject))
        {
            agentsOnGroundEffect.Remove(other.gameObject);
        }

        foreach (GameObject agent in agentsOnGroundEffect)
        {
            ApplyEffectsOn(agent);
        }
    }
}
