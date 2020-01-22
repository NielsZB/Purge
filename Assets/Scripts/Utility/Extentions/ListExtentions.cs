using System.Collections;
using System;
using System.Collections.Generic;

public static class ListExtentions
{

    /// <summary>
    /// Shuffle the list using Fisher-Yates method.
    /// </summary>
    public static void Shuffle<T>(this IList<T> list)
    {
        if (list.Count == 0)
            return;

        Random random = new Random();

        int n = list.Count;

        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);

            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /// <summary>
    /// Returns a random item from the list.
    /// Sampling with replacement by default.
    /// </summary>
    /// <param name="remove">Should the item be removed from the list?</param>
    /// <returns></returns>
    public static T GetRandom<T>(this IList<T> list, bool remove = false)
    {
        if (list.Count == 0)
            throw new IndexOutOfRangeException("Can't select a random item from an empty list");

        int index = UnityEngine.Random.Range(0, list.Count);
        T item = list[index];


        if (remove)
        {
            list.RemoveAt(index);
        }

        return item;

    }

    public static int GetRandom<T>(this IList<T> list, out T randomItem, bool remove = false)
    {
        if (list.Count == 0)
            throw new IndexOutOfRangeException("Can't select a random item from an empty list");

        int index = UnityEngine.Random.Range(0, list.Count);
        T item = list[index];


        if (remove)
        {
            list.RemoveAt(index);
        }

        randomItem = item;
        return index;
    }
}
