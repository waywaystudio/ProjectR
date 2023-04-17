using System;
using UnityEngine;

// ReSharper disable UnusedMember.Global

public static class BooleanExtension
{
    /// <summary>
    /// Invoke Callback When value return false
    /// </summary>
    public static void OnFalse(this bool value, Action callback)
    {
        if (!value) callback?.Invoke();
    }

    public static bool OnFalseLog(this bool value, string errorMessage)
    {
#if UNITY_EDITOR
        if (!value) Debug.LogError(errorMessage);
#endif
        return value;
    }

    /// <summary>
    /// Invoke Callback When value return true
    /// </summary>
    public static void OnTrue(this bool value, Action callback)
    {
        if (value) callback?.Invoke();
    }
    
    public static void OnTrue(this bool value, Func<bool> callback)
    {
        if (value) callback?.Invoke();
    }
    
    public static void OnTrue(this bool value, Func<int> callback)
    {
        if (value) callback?.Invoke();
    }

    public static void OnTrue(this bool value, Action onTrueAction, Action onFalseAction)
    {
        if (value) onTrueAction?.Invoke();
        else
        {
            onFalseAction?.Invoke();
        }
    }
}

