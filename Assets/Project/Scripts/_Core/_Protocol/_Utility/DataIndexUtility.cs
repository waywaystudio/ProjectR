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
}

