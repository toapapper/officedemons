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
        set { health = value; } 
    }

    [SerializeField]
    [Header("Stamina Points")]
    float stamina;

    public float Stamina
    {
        get { return stamina; }
        set { stamina = value; }
    }

    private void Start()
    {
        Health = StartHealth;
    }
}
