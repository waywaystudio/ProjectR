using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConditionTable
{
    [Sirenix.OdinInspector.ShowInInspector]
    private Dictionary<string, Func<bool>> table = new();

    public void Add(string key, Func<bool> predicate)
    {
        if (table.ContainsKey(key)) 
            Debug.LogWarning($"Key is already Exist. key:{key}");

        table.Add(key, predicate);
    }

    public void Register(string key, ConditionTable otherTable)
    {
        table.Add(key, () => otherTable.IsAllTrue);
    }
    
    public void Remove(string key) => table.TryRemove(key);
    public void Clear() => table.Clear();
        
    public bool IsAllTrue
    { 
        get
        {
            foreach (var item in table) 
                if (!item.Value.Invoke()) return false;

            return true;
        }
    }

    public bool HasFalse
    {
        get
        {
            foreach (var item in table) 
                if (!item.Value.Invoke()) return true;

            return false;
        }
    }
}