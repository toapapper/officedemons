using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// [DEPRECATED] i suppose.. we decided not to use discreet heart containers to indicate hp
/// 
/// <para>
/// Container and handler for all the hearts shown in the player-ui
/// </para>
///   
///  <para>
///  Author: Ossian
///  
/// </para>
///  
/// </summary>

// Last Edited: 20-10-21

public class UIHeartContainer : MonoBehaviour
{
    /// <summary>
    /// Only used for creating the rest of the hearts.
    /// </summary>
    [SerializeField] private GameObject heartContainer;
    private GameObject player;
    private Attributes playerAttributes;

    [SerializeField] private float spacingRatio = 1.2f;
    [SerializeField] private int heartsPerStartHealth = 3;
    [SerializeField] private int startHealth = 6;

    private List<GameObject> hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite halfHeart;
    
    /// <summary>
    /// Initializes this heartcontainer
    /// </summary>
    /// <param name="player"> player this belongs to </param>
    public void SetPlayer(GameObject player)
    {
        this.player = player;
        playerAttributes = player.GetComponent<Attributes>();
        startHealth = playerAttributes.StartHealth;

        hearts = new List<GameObject>();
        UpdateHearts(playerAttributes.StartHealth);
    }
    
    void Update()
    {
        if(player != null && playerAttributes != null)
        {
            UpdateHearts(playerAttributes.Health);
        }
    }

    /// <summary>
    /// Updates the hearts in appearance and amount depending on <paramref name="health"/>
    /// </summary>
    /// <param name="health"></param>
    public void UpdateHearts(float health)
    {
        for(int i = 0; i < hearts.Count; i++)
        {
            hearts[i].SetActive(false);
        }

        float ratio = health / startHealth;
        int halfHearts = Mathf.RoundToInt(ratio * heartsPerStartHealth * 2);
        bool hasHalfHeart = halfHearts % 2 == 1;

        int fullHearts = Mathf.FloorToInt(halfHearts / 2);
        if(hearts.Count < fullHearts + 1)
        {
            int newHearts = fullHearts - hearts.Count;
            for(int i = 0; i < newHearts; i++)
                CreateNewHeart();
        }

        for(int i = 0; i < fullHearts; i++)
        {
            hearts[i].SetActive(true);
            hearts[i].GetComponent<Image>().sprite = fullHeart;
        }

        if (hasHalfHeart)
        {
            hearts[fullHearts].SetActive(true);
            hearts[fullHearts].GetComponent<Image>().sprite = halfHeart;
        }
    }

    /// <summary>
    /// Creates a new heart. Usefull in case you suddenly have more healt than you started with.
    /// </summary>
    private void CreateNewHeart()
    {
        int index = hearts.Count;
        Vector2 position = heartContainer.transform.position;
        RectTransform heartTransform = heartContainer.transform as RectTransform;
        //position = new Vector2(position.x + index * heartTransform.rect.size.x * spacingRatio, position.y);
        position = new Vector2(heartContainer.transform.localPosition.x + index * heartTransform.rect.size.x * spacingRatio, heartContainer.transform.localPosition.y);
        //Vector2 nPos = new Vector2(index * heartTransform.rect.size.x * spacingRatio, 0);
        //nPos *= transform.right;
        //position += nPos;

        GameObject heart = Instantiate(heartContainer, transform, false);
        heart.transform.localPosition = position;
        heart.transform.localRotation = Quaternion.identity;

        heart.SetActive(false);
        hearts.Add(heart);
    }
}
