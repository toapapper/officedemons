using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnUI : MonoBehaviour
{
    [SerializeField] private List<Image> turnImages;
    [SerializeField] private float defaultTime;
    private float time;
    private bool somethingShown;
    public bool showVictory;
    private float timer;

    public static TurnUI Instance { get; private set; }
    // Start is called before the first frame update


    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    private void Update()
    {
        if (somethingShown)
        {
            timer += Time.deltaTime;
            if (timer >= defaultTime)
            {
                for (int i = 0; i < turnImages.Count; i++)
                {
                    if (turnImages[i].gameObject.activeSelf == true)
                    {
                        turnImages[i].gameObject.SetActive(false);
                    }
                }
                timer = 0;
                somethingShown = false;
                showVictory = false;
            }
        }
        if (showVictory)
        {
            ShowNewImage(3,2);
        }
    }
    /// <summary>
    /// 0 == CombatStart, 1 == DemonTurn <br/>
    /// 2 == PolieTurn, 3 == Victory
    /// </summary>
    /// <param name="image"></param>
    /// <param name="duration">0 == standard</param>

    public void ShowNewImage(int imageNr, float duration)
    {
        Image image = turnImages[imageNr];
        if (duration == 0)
        {
            time = defaultTime;
        }
        else
        {
            time = duration;
        }
        for (int i = 0; i < turnImages.Count; i++)
        {
            if (turnImages[i].gameObject.activeSelf == true)
            {
                turnImages[i].gameObject.SetActive(false);
            }
        }
        image.gameObject.SetActive(true);
        somethingShown = true;
    }


    /// <summary>
    /// Hard change the alpha to make them transperant or opaque
    /// </summary>
    /// <param name="image"></param>
    /// <param name="alphaValue"></param>
    /// <returns></returns>
    private Image ChangeAlpha(Image image, float alphaValue)
    {
        Color color = image.color;
        color.a = alphaValue;
        image.color = color;
        return image;
    }


    /// <summary>
    /// make the alpha grow 
    /// </summary>
    /// <param name="image"></param>
    /// <param name="EndAlpha"></param>
    private void AddAlpha(Image image, float EndAlpha)
    {
        if (image.color.a >= 1f ||image.color.a >= EndAlpha)
        {
            return;
        }
        float alpha = image.color.a;
        alpha += Time.deltaTime * 0.001f;
        if (alpha > 1)
        {
            alpha = 1;
            return;
        }
        image = ChangeAlpha(image, alpha);
        AddAlpha(image, EndAlpha);
    }


    /// <summary>
    /// make the alpha shrink
    /// </summary>
    /// <param name="image"></param>
    /// <param name="EndAlpha"></param>
    private void ReduceAlpha(Image image, float EndAlpha)
    {
        if (image.color.a <= 0f || image.color.a <= EndAlpha)
        {
            image.gameObject.SetActive(false);
            return;
        }
        float alpha = image.color.a;
        alpha -= Time.deltaTime * 0.1f;
        if (alpha < 0)
        {
            alpha = 0;
        }
        image = ChangeAlpha(image, alpha);

        ReduceAlpha(image, EndAlpha);
    }
}
