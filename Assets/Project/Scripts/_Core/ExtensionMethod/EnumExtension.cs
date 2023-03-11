using System;
using System.Linq;
// ReSharper disable UnusedMember.Global

public static class EnumExtension
{
    public static T Next<T>(this T src) where T : struct
    {
        if (!typeof(T).IsEnum) throw new ArgumentException($"Argument {typeof(T).FullName} is not an Enum");

        var arr = (T[])Enum.GetValues(src.GetType());
        var j   = Array.IndexOf(arr, src) + 1;
        return (arr.Length ==j) ? arr[0] : arr[j];
    }
        
    public static Enum GetRandomEnumValue(this Type t)
    {
        return Enum.GetValues(t)            
                   .OfType<Enum>()                 
                   .OrderBy(_ => Guid.NewGuid())   
                   .FirstOrDefault();              
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