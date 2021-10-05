using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnEnter : MonoBehaviour
{

    public int damage = 100;
    public bool dissapearAfter = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


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
