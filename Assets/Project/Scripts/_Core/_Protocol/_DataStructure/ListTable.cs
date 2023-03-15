using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ListTable<TKey, TValue>
{
    [SerializeField]
    private List<TValue> list;
    private Dictionary<TKey, TValue> table;
    
    public int Count => list.Count;
    
    public ListTable()
    {
        list  = new List<TValue>();
        table = new Dictionary<TKey, TValue>();
    }

    public ListTable(TKey key, List<TValue> list)
    {
        this.list = list;
        table     = new Dictionary<TKey, TValue>();

        this.list.ForEach(item => table.Add(key, item));
    }

    public void Add(TKey key, TValue value)
    {
        list.Add(value);
        table.Add(key, value);
    }

    public void Remove(TKey key)
    {
        table.Remove(key, out var value);
        list.Remove(value);
    }

    public bool TryAdd(TKey key, TValue value)
    {
        if (table.ContainsKey(key)) return false;

        Add(key, value);
        return true;
    }

    public bool TryRemove(TKey key)
    {
        if (!table.ContainsKey(key)) return false;

        Remove(key);
        return true;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return table.TryGetValue(key, out value);
    }

    public void ForEach(Action<TValue> action)
    {
        foreach (var item in list) action?.Invoke(item);
    }

    public void Clear()
    {
        list.Clear();
        table.Clear();
    }
}
