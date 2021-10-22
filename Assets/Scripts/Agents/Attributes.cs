using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Contains all stats and information about the agent
/// 
/// </para>
///   
///  <para>
///  Author: Tinea Larsson, Johan Melkersson, Jonas Lundin, Kristian Svensson, Ossian Sarén Gran
///  
/// </para>
///  
/// </summary>

// Last Edited: 18-10-21

public enum Characters { Tim, Susan, Vicky, Devin }

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
    private int startHealth;

    public int StartHealth
    {
        get { return startHealth; }
    }

    [SerializeField]
    [Header("Current Health Points")]
    private int health;

    public int Health
    {
        get { return health; }
        set { 
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
