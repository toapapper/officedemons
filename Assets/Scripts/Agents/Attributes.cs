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

public enum Characters { Terrible_Tim, Susan_The_Destroyer, Vicious_Vicky, Devin }

public class Attributes : MonoBehaviour
{
    [SerializeField]
    private Color playerColor = Color.grey;
    public Color PlayerColor { get { return playerColor; } }

    [SerializeField]
    private Characters characterName = Characters.Devin;
    public Characters Name { get { return characterName; } }

    [SerializeField]
    public Sprite portrait;

    [SerializeField]
    private string jobTitle = "arbetsl�s";
    public string JobTitle { get { return jobTitle; } }

    [SerializeField]
    [Header("Start Health Points")]
    private int startHealth;
    [SerializeField]
    private int killCount = 0;

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
            else if (health > startHealth)
            {
                health = startHealth;
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

    public int KillCount
	{
        get { return killCount; }
        set { killCount = value; }
    }

    private void Awake()
    {
        Health = StartHealth;
        Stamina = StartStamina;
    }



    //TODO Vicky takes dmg when rushing fix
    private void OnCollisionEnter(Collision collision)
    {
        Vector3 velocity = gameObject.GetComponent<Rigidbody>().velocity;
        if (characterName != Characters.Vicious_Vicky)
        {
            float force = Mathf.Abs(velocity.x) + Mathf.Abs(velocity.z);
            if ( force >= 100)
            {
                Debug.Log("VELOCITY.X : " + velocity.x + "| VELOCITY.Z : " + velocity.z);
                Debug.Log("BOOM BIG DAMAGE");
                health -= 70;
                velocity = Vector3.zero;
            }
            else if (force >= 20)
            {
                Debug.Log("VELOCITY.X : " + velocity.x + "| VELOCITY.Z : " + velocity.z);
                Debug.Log("Abs"+Mathf.Abs(velocity.x + velocity.z));
                Debug.Log("BOOM MEDIUM DAMAGE");
                health -= 20;
                velocity = Vector3.zero;
            }
            else
            {
                //Debug.Log("VELOCITY.X : " + velocity.x + "| VELOCITY.Z : " + velocity.z);
            }
        }
    }
}
