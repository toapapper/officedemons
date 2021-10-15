using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bara en enum med de olika karakt�rerna, kommer anv�ndas fr�mst i ui antar jag f�r att kunna avg�ra vilken karakt�r ett gameObject �r
/// </summary>
public enum Characters
{
    Tim,
    Susan,
    Vicky,
    Devin
}

public class Attributes : MonoBehaviour
{
    [SerializeField]
    private Color playerColor = Color.grey;
    public Color PlayerColor { get { return playerColor; } }

    [SerializeField]
    private Characters characterName = Characters.Devin;
    public Characters Name { get { return characterName; } }

    [SerializeField]
    private string jobTitle = "arbetsl�s";
    public string JobTitle { get { return jobTitle; } }

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
        set { //kanske b�r flyttas n�gon annan stans.
            health = value;
            if (health <= 0)
            {
                health = 0;
                Effects.Die(gameObject);
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
