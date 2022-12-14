using System;
using System.Diagnostics;
using Core;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[Serializable]
public class DebugController
{
    [OdinSerialize, ShowInInspector] 
    protected static bool Show;

    [Conditional("UNITY_EDITOR")]
    public static void Log(object message) => Show.OnTrue(() => UnityEngine.Debug.Log(message));
}

public class CombatDebug : DebugController {}
public class CharacterDebug : DebugController {}
