using System;
using System.Collections.Generic;
using UnityEngine;

public class ConditionTable : Dictionary<string, Func<bool>>
{
    public void Unregister(string key) => this.TryRemove(key);
    public void UnregisterAll() => Clear();

    protected void TryAdd(string key, Func<bool> value, bool overwrite)
    {
        if (!TryAdd(key, value) && overwrite) this[key] = value;
    }
        
    public void Register(string key, Func<bool> predicate)
    {
        if (ContainsKey(key)) Debug.LogWarning($"Key is already Exist. key:{key}");

        TryAdd(key, predicate);
    }
        
    public bool IsAllTrue
    { 
        get
        {
            foreach (var item in this) 
                if (!item.Value.Invoke()) return false;

            return true;
        }
    }
    public bool IsAllFalse
    {
        get
        {
            if (Count == 0)
            {
                Debug.LogWarning("Table is Null or Empty. return true");
                return true;
            }
                
            foreach (var item in this) 
                if (item.Value.Invoke()) return false;

            return true;
        }
    }
    public bool HasTrue
    {
        get
        {
            if (Count == 0)
            {
                Debug.LogWarning("Table is Null or Empty. return false");
                return false;
            }
                
            foreach (var item in this) 
                if (item.Value.Invoke()) return true;
                
            return false;
        }
    }
    public bool HasFalse
    {
        get
        {
            foreach (var item in this) 
                if (!item.Value.Invoke()) return true;

            return false;
        }
    }
    public int TrueCount
    {
        get
        {
            var result = 0;

            foreach (var item in this)
            {
                if (item.Value.Invoke()) result++;
            }

            return result;
        }
    }
    public int FalseCount
    {
        get
        {
            var result = 0;

            foreach (var item in this)
            {
                if (!item.Value.Invoke()) result++;
            }

            return result;
        }
    }
}