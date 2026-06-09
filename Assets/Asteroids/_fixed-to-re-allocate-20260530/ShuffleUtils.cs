using System;
using System.Collections.Generic;

public class ShuffleUtils
{
    private static Random rng = new Random();

    public static void FisherYatesShuffle<T>(List<T> list)
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    public static List<T> InterleavedShuffle<T>(List<T> list)
    {
        List<T> result = new List<T>();
        int half = list.Count / 2;

        for (int i = 0; i < half; i++)
        {
            result.Add(list[half + i]); // from second half
            result.Add(list[i]);        // from first half
        }

        // If odd number of elements, add the last remaining one
        if (list.Count % 2 != 0)
        {
            result.Add(list[list.Count - 1]);
        }

        return result;
    }

}
