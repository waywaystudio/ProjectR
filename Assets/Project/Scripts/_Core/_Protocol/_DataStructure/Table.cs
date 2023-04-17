using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Table<TKey, TValue>
{
    [SerializeField] private List<TValue> list;
    
    private Dictionary<TKey, TValue> map;
    
    public Table()
    {
        list = new List<TValue>();
        map  = new Dictionary<TKey, TValue>();
    }

    public int Count => list.Count;

    public void Add(TKey key, TValue value)
    {
        if (!map.ContainsKey(key))
        {
            map.Add(key, value);
            list.Add(value);
        }
        else
        {
            throw new ArgumentException("Key already exists.");
        }
    }

    public bool Remove(TKey key)
    {
        if (!map.ContainsKey(key)) return false;
        map.Remove(key, out var value);
        
        return list.Remove(value);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return map.TryGetValue(key, out value);
    }

    public void Iterate(Action<TValue> action)
    {
        foreach (var item in list) action?.Invoke(item);
    }

    public void Clear()
    {
        list.Clear();
        map.Clear();
    }
}
