using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Summary of what the component does 
/// Miscelaneous static utilities
/// </para>
///   
///  <para>
///  Author: Ossian
///  
/// </para>
///  
/// </summary>

/*
 * Last Edited:
 * 15-10-2021
 */

public static class Utilities
{
    /// <summary>
    /// Returns a list containing all the keys in <paramref name="dictionary"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="dictionary"></param>
    /// <returns></returns>
    public static List<T> ListDictionaryKeys<T, U>(Dictionary<T, U> dictionary)
    {
        List<T> list = new List<T>();

        foreach(KeyValuePair<T, U> pair in dictionary)
        {
            list.Add(pair.Key);
        }

        return list;
    }

    /// <summary>
    /// Cleans a list of destroyed gameObjects or otherwise null objects, repacking it
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ts"></param>
    public static void CleanList<T>(List<T> ts)
    {
        List<int> rmvAt = new List<int>();

        for (int i = 0; i < ts.Count; i++)
            if (ts[i].Equals(null))
                rmvAt.Add(i);

        int removed = 0;
        foreach (int i in rmvAt)
        {
            ts.RemoveAt(i - removed);
            removed++;
        }
    }

    /// <summary>
    /// Lerp between multiple colors
    /// </summary>
    /// <param name="colors">Colors to lerp between</param>
    /// <param name="lerpNr">current lerpnr</param>
    /// <returns></returns>
    public static Color MultiColorLerp(Color[] colors, float lerpNr)
    {
        Color color = Color.black;

        int colorIndex0 = Mathf.FloorToInt(lerpNr * (colors.Length - 1));
        int colorIndex1 = colorIndex0 + 1;

        if (colorIndex1 >= colors.Length)
            return colors[colors.Length - 1];

        float lerpFloor = (float)colorIndex0 / (float)(colors.Length - 1);
        float lerpRoof = (float)colorIndex1 / (float)(colors.Length - 1);
        float currentLerp = lerpNr - lerpFloor;
        currentLerp /= lerpRoof - lerpFloor;

        color = Color.Lerp(colors[colorIndex0], colors[colorIndex1], currentLerp);

        return color;
    }
    
}
