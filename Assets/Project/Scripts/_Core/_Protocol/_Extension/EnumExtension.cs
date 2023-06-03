using System;
using System.Linq;
using UnityEngine;

// ReSharper disable UnusedMember.Global

public static class EnumExtension
{
    public static T Next<T>(this T source) where T : struct
    {
        if (!EnumValidate<T>()) return default;

        var arr = (T[])Enum.GetValues(source.GetType());

        var nextIndex = Array.IndexOf(arr, source) + 1;
        return nextIndex >= arr.Length ? arr[0] : arr[nextIndex];
    }
    
    public static T NextExceptNone<T>(this T source) where T : struct
    {
        if (!EnumValidate<T>()) return default;

        var arr = (T[])Enum.GetValues(source.GetType());

        if (arr.Length < 2) 
        {
            return arr[0];
        }
    
        var nextIndex = Array.IndexOf(arr, source) + 1;
        return nextIndex >= arr.Length ? arr[1] : arr[nextIndex];
    }

    public static T PrevExceptNone<T>(this T source) where T : struct
    {
        if (!EnumValidate<T>()) return default;

        var arr = (T[])Enum.GetValues(source.GetType());

        if (arr.Length < 2) 
        {
            return arr[0];
        }
    
        var prevIndex = Array.IndexOf(arr, source) - 1;
        return prevIndex == 0 ? arr[^1] : arr[prevIndex];
    }

    public static T RandomEnum<T>(this T source) where T : Enum
    {
        if (!EnumValidate<T>()) return default;

        var arr = (T[])Enum.GetValues(source.GetType());

        var randomIndex = UnityEngine.Random.Range(0, arr.Length);
        
        return arr[randomIndex];
    }
    
    public static T RandomEnumExceptNone<T>(this T source) where T : Enum
    {
        if (!EnumValidate<T>()) return default;

        var arr = (T[])Enum.GetValues(source.GetType());

        if (arr.Length < 2)
        {
            return arr[0];
        }
        
        var randomIndex = UnityEngine.Random.Range(1, arr.Length);
        
        return arr[randomIndex];
    }

    public static T FindIndex<T>(this int digits) where T : Enum
    {
        foreach (T enumValue in Enum.GetValues(typeof(T)))
        {
            if (Convert.ToInt32(enumValue) % 100 == digits)
                return enumValue;
        }

        UnityEngine.Debug.LogError($"No {typeof(T).Name} found with end digits {digits}");
        return default;
    }

    public static void Iterator<T>(this T source, Action<T> action)
    {
        if (!EnumValidate<T>()) return;

        var arr = (T[])Enum.GetValues(source.GetType());
        arr.ForEach(action.Invoke);
    }

    /// <summary>
    /// Includes an enumerated type and returns the new value
    /// </summary>
    public static T Include<T>(this Enum value, T append)
    {
        var type = value.GetType();
 
        // determine the values
        object result = value;
        var    parsed = new Value(append, type);
        if (parsed.Signed != null)
        {
            result = Convert.ToInt64(value) | (long)parsed.Signed;
        }
 
        // return the final value
        return (T)Enum.Parse(type, result.ToString());
    }
 
    public static T Remove<T>(this Enum value, T remove)
    {
        var type = value.GetType();
 
        // determine the values
        object result = value;
        var    parsed = new Value(remove, type);
        if (parsed.Signed != null)
        {
            result = Convert.ToInt64(value) & ~(long)parsed.Signed;
        }
 
        // return the final value
        return (T)Enum.Parse(type, result.ToString());
    }


    private static bool EnumValidate<T>()
    {
        if (typeof(T).IsEnum) return true;
        Debug.LogError($"Argument {typeof(T).FullName} is not an Enum");
        return false;
    }
        
    private class Value
    {
        public readonly long? Signed;
 
        // cached comparisons for tye to use
        private static readonly Type UInt64 = typeof(ulong);
        private static readonly Type UInt32 = typeof(long);
 
        public Value(object value, Type type)
        {
            //make sure it is even an enum to work with
            if (!type.IsEnum)
            {
                throw new
                    ArgumentException("Value provided is not an enumerated type!");
            }
 
            //then check for the enumerated value
            var compare = Enum.GetUnderlyingType(type);
 
            //if this is an unsigned long then the only
            //value that can hold it would be a ulong
            if (compare == UInt32 || compare == UInt64)
            {
                Convert.ToUInt64(value);
            }
            //otherwise, a long should cover anything else
            else
            {
                Signed = Convert.ToInt64(value);
            }
        }
    }
}