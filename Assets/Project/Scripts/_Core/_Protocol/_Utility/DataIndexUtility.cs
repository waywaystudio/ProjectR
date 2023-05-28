using System;
using UnityEngine;

public static class DataIndexUtility
{
    public static DataIndex GetCategory(this DataIndex indexer)
    {
        return (DataIndex)Mathf.RoundToInt((int)indexer * 0.000001f);
    }

    public static bool TryFindDataIndex(this DataIndex indexer, string text, out DataIndex result)
    {
        return Enum.TryParse(text, out result);
    }
    
    public static DataIndex TryFindDataIndex(this string original)
    {
        return Enum.TryParse<DataIndex>(original, out var result) ? result : DataIndex.None;
    }

    public static string ConvertDataIndexStyle(this string original)
    {
        return original.Replace(" ", "")
                       .Replace("'", "")
                       .Replace("&", "")
                       .ToPascalCase();
    }

    /// <summary>
    /// part of 000000"00"
    /// </summary>
    public static int GetIndexBar(this DataIndex indexer) => GetNumberFromDigitsToDestDigits(indexer, 1, 2);
    
    /// <summary>
    /// part of 0000"00"00
    /// </summary>
    public static int GetGroupBar(this DataIndex indexer) => GetNumberFromDigitsToDestDigits(indexer, 3, 4);
    
    /// <summary>
    /// part of 00"00"0000
    /// </summary>
    public static int GetSubCategoryBar(this DataIndex indexer) => GetNumberFromDigitsToDestDigits(indexer, 5, 6);
    
    /// <summary>
    /// part of "00"000000
    /// </summary>
    public static int GetCategoryBar(this DataIndex indexer) => GetNumberFromDigitsToDestDigits(indexer, 7, 8);

    public static int GetBar(this DataIndex indexer, int order)
    {
        if (order is >= 5 or <= 0)
        {
            Debug.LogError($"DataIndex's Bar must larger than 0, smaller than 5. Input:{order} return 0");
            return 0;
        }

        if ((int)indexer <= 9999999)
        {
            Debug.LogError($"DataIndex's Bar must larger 10000000. Input:{order} return 0");
            return 0;
        }

        var lastDigit = order * 2;
        var startDigit = lastDigit - 1;

        return GetNumberFromDigitsToDestDigits(indexer, startDigit, lastDigit);
    } 

    public static int GetNumberOfDataIndex(this DataIndex indexer, int digit)
    {
        return ((int)indexer).GetNumberOfDigits(digit);
    }
    
    public static int GetNumberFromDigitsToDestDigits(this DataIndex indexer, int startDigit, int endDigit)
    {
        return ((int)indexer).GetNumberOfFromToDestDigits(startDigit, endDigit);
    }
}

