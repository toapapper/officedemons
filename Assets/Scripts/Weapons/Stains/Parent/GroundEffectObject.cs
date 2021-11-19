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
    public List<GameObject> agentsOnGroundEffect;
    [SerializeField]
    protected int totalDurabilityTurns = 3;
    [SerializeField]
    protected float totalDurabilityTime = 30f;

    protected int durabilityTurns;
    protected float durabilityTime;

    public void ApplyEffectsOnEnemys()
    {
        foreach (GameObject agent in agentsOnGroundEffect)
        {
            if (agent.tag == "Enemy")
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
    protected abstract void ApplyEffectsOn(GameObject agent);

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

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!agentsOnGroundEffect.Contains(other.gameObject) && other.gameObject.GetComponent<Attributes>().Health > 0)
        {
            agentsOnGroundEffect.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (agentsOnGroundEffect.Contains(other.gameObject))
        {
            agentsOnGroundEffect.Remove(other.gameObject);
        }
    }
}
