using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// <para>
/// A single floating text element. Shows a string on screen while fading and floating upwards. <br/>
/// Inactivates itself when its lifetime is up.
/// </para>
///   
///  <para>
///  Author: Ossian
/// </para>
/// </summary>

// Last Edited: 29-10-2021

public class FloatingTextElement : MonoBehaviour
{
    private const float standardLifeTime = 1.6f;
    private const float standardVelocity = 10f; //pixels per second


    [SerializeField] private TMP_Text text;
    public float stdSize;

    private float velocity = standardVelocity;  //velocity in pixels per second
    private float lifeTime = standardLifeTime;   //lifetime in seconds
    private float deltaAlpha { get { return 1 / lifeTime; } }

    void Awake()
    {
        stdSize = text.fontSize;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPos = this.transform.position;
        newPos += Vector2.up * velocity * Time.deltaTime;

        transform.position = newPos;

        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - deltaAlpha * Time.deltaTime);
        if(text.color.a <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Activates the gameObject and sets screen position and text 
    /// </summary>
    /// <param name="text"> the text that will be displayed </param>
    /// <param name="position"> the position this will start from </param>
    /// <param name="color"> the color it will have</param>
    /// <param name="lifeTime"> how long it will take to fade </param>
    /// <param name="velocity"> the speed it rises with (screen space coordinates per second)</param>
    /// <param name="sizeMultiplier"> how much bigger or smaller it will be than the standard </param>
    public void Activate(string text, Vector2 position, Color color, float size, float lifeTime = standardLifeTime, float velocity = standardVelocity)
    {
        this.lifeTime = lifeTime;
        this.velocity = velocity;
        this.text.fontSize = size;
        color.a = 1;
        this.text.color = color;
        this.text.text = text;
        //this.transform.position = position;
        GetComponent<RectTransform>().position = position;
        this.gameObject.SetActive(true);
    }
}
