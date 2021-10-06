//Initially     written by Ossian, feel free to add to it.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OssianUtils
{

    /// <summary>
    /// Cleans a list of null objects, repacking it
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
    
}
