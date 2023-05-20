using UnityEngine;

public static class Verify
{
    /// <summary>
    /// Null Checking Static Method
    /// </summary>
    /// <param>Target which want to check
    ///     <name>value</name>
    /// </param>
    /// <param>Debug.LogError message when target is null
    ///     <name>messageWhenNull</name>
    /// </param>
    /// <typeparam>Nullable Type
    ///     <name>T</name>
    /// </typeparam>
    /// <returns></returns>
    public static bool IsNotNull<T>(T value, string messageWhenNull = "", bool showLog = true) where T : class
    {
        if (value != null) return true;
        if (!showLog) return false;
        
        Debug.LogError(messageWhenNull == "" 
            ? "Value is Null." 
            : messageWhenNull);
        
        return false;
    }

    public static bool IsNull<T>(T value, string message = "", bool showLog = true) where T : class
    {
        if (value != null) return false;
        if (!showLog) return true;
        
        Debug.LogError(message == "" 
            ? "Value is Null." 
            : message);

        return true;
    }
    

    /// <summary>
    /// Value Type Default Checking Static Method
    /// </summary>
    /// <param>Target which want to check
    ///     <name>value</name>
    /// </param>
    /// <param>Debug.LogError message when target is default(T)
    ///     <name>messageWhenDefault</name>
    /// </param>
    /// <typeparam>Value Type
    ///     <name>T</name>
    /// </typeparam>
    /// <returns></returns>
    public static bool IsNotDefault<T>(T value, string messageWhenDefault = "", bool showLog = true) where T : struct
    {
        if (!value.Equals(default(T))) return true;
        
        if (messageWhenDefault != "")
        {
            Debug.LogError(messageWhenDefault);
            return false;
        }

        if (showLog)
        {
            Debug.LogError($"inserted {value} is typeof{typeof(T).Name}'s default value.");
        }

        return false;
    }

    public static bool IsDefault<T>(T value, string message = "", bool showLog = true) where T : struct
    {
        if (!value.Equals(default(T))) return false;
        if (!showLog) return true;

        Debug.LogError(message == "" 
            ? $"inserted {value} is typeof{typeof(T).Name}'s default value." 
            : message);

        return true;
    }
}
