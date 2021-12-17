using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// The negative version of ground effect objects
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15-11-19
public class NegativeGroundObject : GroundEffectObject
{
    private NegativeGroundObject groundObject;
    public float damage;

    //I refuse to use this. This is a symptom of a fucked up way of doing things. Just stick this script on a prefab and instantiate it instead. Screw this method.
    //Fuck fuck fuck fuckedi fuck duck
    public void CreateGroundObject(Vector3 position, float stainRadius, float damage, List<StatusEffectType> effects)
    {
        groundObject = Instantiate(this, position, Quaternion.Euler(0, 0, 0));
        groundObject.transform.localScale = new Vector3(stainRadius, groundObject.transform.localScale.y, stainRadius);
        groundObject.damage = damage;
        groundObject.effects = effects;
        groundObject.durabilityTurns = totalDurabilityTurns;
        groundObject.durabilityTime = totalDurabilityTime;
        groundObject.agentsOnGroundEffect = new List<GameObject>();
        GameManager.Instance.GroundEffectObjects.Add(groundObject.gameObject);
    }


    protected override void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            if (other.tag == "Player" || (other.tag == "Enemy" && other.gameObject.name != "tank"))
			{
                if (!agentsOnGroundEffect.Contains(other.gameObject) &&
                    other.GetComponent<Attributes>().Health > 0)
                {
                    ApplyEffectsOn(other.gameObject);
                    base.OnTriggerEnter(other);
                }
            }
        }
    }
}
