using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Deals damage to entities with an Attributes component on trigger enter. Can optionally dissapear after it has dealt damage or just not dissapear.
/// </summary>
public class DamageOnEnter : MonoBehaviour
{
    [SerializeField] private int damage = 100;
    [SerializeField] private bool dissapearAfter = true;

    private void OnTriggerEnter(Collider other)
    {
        Attributes attr = other.gameObject.GetComponent<Attributes>();

        if(attr != null)
        {
            attr.Health -= damage;
            if (dissapearAfter)
                Destroy(gameObject);
        }
        
    }
}
