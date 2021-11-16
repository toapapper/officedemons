using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CoffeStain : MonoBehaviour
{
    public List<GameObject> agentsOnStain;
    [SerializeField]
    protected int totalDurabilityTurns = 3;
    [SerializeField]
    protected float totalDurabilityTime = 60f;

    protected int durabilityTurns;
    protected float durabilityTime;



    public void ApplyEffectsOnEnemys()
    {
        foreach (GameObject agent in agentsOnStain)
        {
            if (agent.tag == "Enemy")
            {
                ApplyEffectsOn(agent);
            }
        }
    }
    public void ApplyEffectsOnPlayers()
    {
        foreach (GameObject agent in agentsOnStain)
        {
            if (agent.tag == "Player")
            {
                ApplyEffectsOn(agent);
            }
        }

        durabilityTurns--;
        if(durabilityTurns <= 0)
		{
            RemoveGroundEffectObject();
        }
    }
    protected abstract void ApplyEffectsOn(GameObject agent);

    protected void RemoveGroundEffectObject()
	{
        GameManager.Instance.GroundEffectObjects.Remove(gameObject);
        Destroy(this);
    }

	private void FixedUpdate()
	{
        durabilityTime -= Time.fixedDeltaTime;
        if(durabilityTime <= 0)
		{
            RemoveGroundEffectObject();
        }
	}


	protected virtual void OnTriggerEnter(Collider other)
	{
        agentsOnStain.Add(other.gameObject);
    }
	private void OnTriggerExit(Collider other)
    {
        if(agentsOnStain.Contains(other.gameObject))
		{
            agentsOnStain.Remove(other.gameObject);
        }
    }
}
