using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHeartContainer : MonoBehaviour
{
    /// <summary>
    /// Only used for creating the rest of the hearts.
    /// </summary>
    public GameObject heartContainer;
    public GameObject player;
    private Attributes playerAttributes;

    public float spacingRatio = 1.2f;
    public int heartsPerStartHealth = 3;
    public int startHealth = 6;

    private List<GameObject> hearts;
    private Sprite fullHeart;
    private Sprite halfHeart;

    private void Start()
    {
        heartContainer.SetActive(false);
        fullHeart = Resources.Load<Sprite>("Textures/UI/helhjärta");
        halfHeart = Resources.Load<Sprite>("Textures/UI/halvhjärta");
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
        playerAttributes = player.GetComponent<Attributes>();
        startHealth = playerAttributes.StartHealth;

        UpdateHearts(playerAttributes.Health);
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null && playerAttributes != null)
        {
            UpdateHearts(playerAttributes.Health);
        }
    }

    public void UpdateHearts(float health)
    {
        float ratio = health / startHealth;
        int halfHearts = Mathf.RoundToInt(ratio * heartsPerStartHealth);
        bool halfHeart = halfHearts % 2 != 0;

        int fullHearts = Mathf.FloorToInt(halfHearts / 2);
    }

    private void CreateNewHeart()
    {
        int index = hearts.Count - 1;
        Vector2 position = heartContainer.transform.position;
        RectTransform heartTransform = heartContainer.transform as RectTransform;
        position = new Vector2(position.x + index * heartTransform.rect.size.x * spacingRatio, position.y);

        GameObject heart = Instantiate(heartContainer);
        heart.transform.position = position;

        heart.SetActive(false);
        hearts.Add(heart);
    }
}
