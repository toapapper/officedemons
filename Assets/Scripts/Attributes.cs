using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attributes : MonoBehaviour
{
    [SerializeField]
    [Header("Start Health Points")]
    int startHealth;

    public int StartHealth
    {
        get { return startHealth; }
    }

    [SerializeField]
    [Header("Current Health Points")]
    int health;

    public int Health
    {
        get { return health; }
        set { //kanske bör flyttas någon annan stans.
            health = value;
            if (health <= 0)
            {
                health = 0;
                GetComponent<Actions>().Die();
            }
        } 
    }

   
    [SerializeField]
    [Header("Stamina")]
    [Tooltip("Stamina drains at a rate of one per second")]
    float startStamina = 3.5f;

    public float StartStamina
    {
        get { return startStamina; }
    }


    [SerializeField]
    float stamina;

    public float Stamina
    {
        get { return stamina; }
        set { stamina = value; }
    }

    private void Awake()
    {
        Health = StartHealth;
        Stamina = StartStamina;
    }
}
