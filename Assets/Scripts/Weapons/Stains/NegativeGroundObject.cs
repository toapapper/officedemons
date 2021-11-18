using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegativeGroundObject : GroundEffectObject
{
    private NegativeGroundObject groundObject;
    private float damage;
    List<WeaponEffects> effects;

    public void CreateGroundObject(Vector3 position, float stainRadius, float damage, List<WeaponEffects> effects)
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

    protected override void ApplyEffectsOn(GameObject agent)
    {
        //Effects.Damage(agent, damage);
        Effects.ApplyWeaponEffects(agent, effects);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            if (!agentsOnGroundEffect.Contains(other.gameObject))
            {
                if (other.tag == "Player" || other.tag == "Enemy")
                {
                    ApplyEffectsOn(other.gameObject);
                    base.OnTriggerEnter(other);
                }
            }
        }
    }
}
