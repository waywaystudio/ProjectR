using System;
using System.Collections.Generic;
using System.Reflection;
using UnityGoogleSheet.Core.Exception;
using UnityGoogleSheet.Core.Type.Attribute;

namespace UnityGoogleSheet.Core.Type
{ 
    public static class TypeMap
    {
        public static bool init;

        public static Dictionary<System.Type, IType> Map { get; } = new();
        public static Dictionary<string, System.Type> StrMap { get; } = new();
        public static Dictionary<string, EnumType> EnumMap { get; } = new();

        public static void EnumTypeInjection(System.Type enumType)
        {
            if (enumType.IsEnum)
            {
                var key = enumType.Name;  
                if (!EnumMap.ContainsKey(key))
                {
                    EnumMap.Add(key, new EnumType()
                    {
                        EnumName = enumType.Name,
                        Assembly = enumType.Assembly,
                        NameSpace = (string.IsNullOrEmpty(enumType.Namespace)) ? null : enumType.Namespace,
                        Type = enumType
                    });
                }
            }
            else
            {
                UnityEngine.Debug.LogError(enumType +" is not enum!");
            }
        } 

        public static void Init()
        {
            if (init) 
                return;
            
            var subClassesEnum = UgsUtility.GetAllSubclassOf(typeof(Enum));
#if UGS_DEBUG
                Stopwatch sw = new Stopwatch();
                sw.Start();
#endif
            foreach (var value in subClassesEnum)
            {
                var att = value.GetCustomAttribute(typeof(UGSAttribute));
                if (att != null)
                {
                    EnumTypeInjection(value);
                }
            }
#if UGS_DEBUG
                sw.Stop();
                sw.Reset();
                sw.Start();
#endif
            var subClasses = UgsUtility.GetAllSubclassOf(typeof(IType));
            foreach (var data in subClasses)
            {
                if (data.IsInterface)
                    continue;
                var instance = Activator.CreateInstance(data);
                var att = instance.GetType().GetCustomAttribute<TypeAttribute>();
                if (att != null)
                {
#if !UNITY_EDITOR
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("[TypeMap] Added " + att.type.ToString() + "  " + instance.ToString());
#endif

#if UNITY_EDITOR && UGS_DEBUG
                        UnityEngine.Debug.Log("[TypeMap] Added " + att.type.ToString() + "  " + instance.ToString());
#endif
                    if (!Map.ContainsKey(att.type))
                    {
                        Map.Add(att.type, (IType)instance);
                    }
                    foreach (var separator in att.separactors)
                    {
                        if (StrMap.ContainsKey(separator) == false)
                            StrMap.Add(separator, att.type);
#if !UNITY_EDITOR
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(" ㄴ[TypeMap] Added Sepractors " + separator);
#endif
                    }
                }
                else
                {
#if !UNITY_EDITOR
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[TypeMap] Require Set Type Attribute => " + instance.ToString());
#endif
                    throw new RequireTypeAttributeException();
                }
            }
#if UGS_DEBUG
                sw.Stop();
#endif
            Console.ForegroundColor = ConsoleColor.White;
            init = true;
        }
    }
}
