using System;
using System.Collections.Generic;
using System.Diagnostics;
using Core;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class ActionTable : Dictionary<int, Action>
{
    public void Register(Action action)
    {
        TryAdd(GetHashCode(), action);
    }

    public void UnRegister()
    {
        if (ContainsKey(GetHashCode()))
            Remove(GetHashCode());
    }
}

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
