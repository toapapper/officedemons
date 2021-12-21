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


    [SerializeField] private GameObject particleEffect;

    [SerializeField] public StatusEffectHandler statusEffectHandler;

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

    public int SaveLoadHealth
    {
		get { return health; }
        set { health = value; }
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

    private void Start()
    {
        if(statusEffectHandler == null && (this.CompareTag("Player") || this.CompareTag("Enemy"))) //Initialize the statuseffecthandler prefab that should be a child of this agent.
        {
            statusEffectHandler = gameObject.GetComponentInChildren<StatusEffectHandler>();
            if(statusEffectHandler != null)
            {
                statusEffectHandler.myAgent = gameObject;
            }
        }
    }

    //TODO Vicky takes dmg when rushing fix
    private void OnCollisionEnter(Collision collision)
    {
        Vector3 knockedbackVelocity = collision.relativeVelocity;
        if (characterName != Characters.Vicious_Vicky && !collision.gameObject.CompareTag("Projectile") && !collision.gameObject.CompareTag("Ground"))
        {
            float force = Mathf.Abs(knockedbackVelocity.x) + Mathf.Abs(knockedbackVelocity.z);
            if ( force >= 100)
            {

                Effects.Damage(gameObject, 50);
                gameObject.GetComponent<Rigidbody>().velocity.Set(0, 0, 0);
                if (collision.gameObject.layer == LayerMask.NameToLayer("Destructible"))
                {
                    if (particleEffect != null)
                    {
                        Instantiate(particleEffect, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation);
                    }
                    Effects.Damage(collision.gameObject, 50);
                    CameraShake.Shake(1f, 1f);

                }
            }
            else if (force >= 20)
            {
                Effects.Damage(gameObject, 20);
                if (collision.gameObject.layer == LayerMask.NameToLayer("Destructible"))
                {
                    if (particleEffect != null)
                    {
                        Instantiate(particleEffect, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation);
                    }
                    Effects.Damage(collision.gameObject, 20);
                    CameraShake.Shake(0.5f, 0.5f);
                }
                if (gameObject.GetComponent<Rigidbody>() != null)
                {
                    gameObject.GetComponent<Rigidbody>().velocity.Set(0, 0, 0);
                }
                
            }
            else
            {
                //Instantiate(particleEffect, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation);
            }
        }
    }
}
