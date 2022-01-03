using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Manages all the floating text elements.
/// </para>
///   
///  <para>
///  Author: Ossian
/// </para>
/// </summary>

// Last Edited: 29-10-2021

public class FloatingTextManager : MonoBehaviour
{
    [SerializeField] private GameObject textElement;
    
    [Tooltip("The max amount of textElements visible at once")]
    [SerializeField] private int elementCount = 20;

    [Tooltip("length of random offset in pixels distance")]
    [SerializeField] private float offsetLength = 10f;

    private FloatingTextElement[] elements;
    private Canvas myCanvas;

    //what index of elements is next
    private int currentIndex = 0;

    // Start is called before the first frame update
    void Awake()
    {
        elements = new FloatingTextElement[elementCount];
        elements[0] = textElement.GetComponent<FloatingTextElement>();

        myCanvas = GetComponentInParent<Canvas>();

        for(int i = 1; i < elementCount; i++)
        {
            elements[i] = Instantiate(textElement, transform, false).GetComponent<FloatingTextElement>();
        }
    }

    /// <summary>
    /// Makes a new floating text at the gameobject with a random offset
    /// </summary>
    /// <param name="atObject"></param>
    /// <param name="text"></param>
    /// <param name="color"></param>
    public void newText(GameObject atObject, string text, Color color, float size = 1f)
    {
        Vector2 position = Camera.main.WorldToViewportPoint(atObject.transform.position);
        position.x *= myCanvas.pixelRect.width;
        position.y *= myCanvas.pixelRect.height;

        //random values between -.5 and .5
        Vector2 offset = new Vector2(Random.value - .5f, Random.value - .5f);
        offset.Normalize();

        position += offset * offsetLength;

        newText(position, text, color, size);
    }

    /// <summary>
    /// Makes a new floating text at the pixelposition, no random offset
    /// </summary>
    /// <param name="position"></param>
    /// <param name="text"></param>
    /// <param name="color"></param>
    public void newText(Vector2 position, string text, Color color, float size = 1f)
    {
        elements[currentIndex].Activate(text, position, color, sizeMultiplier: size);
        
        currentIndex++;
        if(currentIndex >= elements.Length)
        {
            currentIndex = 0;
        }
    }

}
