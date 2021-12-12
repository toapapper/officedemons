using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnUI : MonoBehaviour
{
    [SerializeField] private List<Image> turnImages;

    // Start is called before the first frame update
    void Start()
    {
    }
    /// <summary>
    /// 0 == CombatStart, 1 == DemonTurn <br/>
    /// 2 == PolieTurn, 3 == Victory
    /// </summary>
    /// <param name="image"></param>
    public void ShowNewImage(int imageNr)
    {
        Image image = turnImages[imageNr];
        image.gameObject.SetActive(true);
        image = ChangeAlpha(image, 0f);
        AddAlpha(image, 1f);
        //Wait(10000);
        //ReduceAlpha(image, 0f);
    }

    private Image ChangeAlpha(Image image, float alphaValue)
    {
        Color color = image.color;
        color.a = alphaValue;
        image.color = color;
        return image;
    }

    private void AddAlpha(Image image, float EndAlpha)
    {
        if (image.color.a >= 1f ||image.color.a >= EndAlpha)
        {
            Debug.Log("ALPHA : " + image.color.a);
            return;
        }
        float alpha = image.color.a;
        alpha += Time.deltaTime * 0.01f;
        if (alpha > 1)
        {
            alpha = 1;
        }
        image = ChangeAlpha(image, alpha);
        AddAlpha(image, EndAlpha);
    }

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


    private void Wait(float time)
    {
        while (time >= 0)
        {
            time -= Time.deltaTime;
        }
    }
}
