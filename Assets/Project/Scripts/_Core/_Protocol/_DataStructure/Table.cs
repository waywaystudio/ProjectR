using System;
using System.Collections.Generic;
using UnityEngine;
// ReSharper disable InconsistentNaming

[Serializable]
public class Table<TKey, TValue>
{
    [SerializeField] protected List<TKey> keyList = new();
    [SerializeField] protected List<TValue> valueList = new();
    
    public List<TKey> KeyList => keyList;
    public List<TValue> ValueList => valueList;
    
    private Dictionary<TKey, TValue> map = new();
    protected Dictionary<TKey, TValue> Map
    {
        get
        {
            if (map.IsNullOrEmpty() || map.Count != keyList.Count)
            {
                map.Clear();
                valueList.ForEach((value, index) => map.Add(keyList[index], value));
            }

            return map;
        }
    }
    
    public TValue this[TKey key]
    {
        get => Map.ContainsKey(key) 
            ? Map[key] 
            : default;
        set
        {
            if (Map.ContainsKey(key))
            {
                var index = keyList.IndexOf(key);
                valueList[index] = value;
            }
            else
            {
                keyList.Add(key);
                valueList.Add(value);
            }

            Map[key] = value;
        }
    }

    public int Count => valueList.Count;

    public virtual void Add(TKey key, TValue value)
    {
        if (Map.ContainsKey(key))
        {
            return;
        }
        
        Map.Add(key, value);
        keyList.Add(key);
        valueList.Add(value);
    }

    public virtual bool Remove(TKey key)
    {
        if (!Map.ContainsKey(key)) return false;
        Map.Remove(key);
        
        var index = keyList.IndexOf(key);
        if(index < 0) return false;
        
        keyList.RemoveAt(index);
        valueList.RemoveAt(index);
        
        return true;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return Map.TryGetValue(key, out value);
    }

    public void Iterate(Action action) { foreach (var _ in valueList) action?.Invoke(); }
    public void Iterate(Action<TValue> action)  { foreach (var item in valueList) action?.Invoke(item); }
    public void KeyIterate(Action<TKey> action) { foreach (var item in keyList) action?.Invoke(item); }
    

    public void Clear()
    {
        keyList.Clear();
        valueList.Clear();
        Map.Clear();
    }
    
    public void CreateTable(List<TValue> dataList, Func<TValue, TKey> keySelector)
    {
        Clear();

        foreach (var data in dataList)
        {
            var key = keySelector(data);
            Add(key, data);
        }
    }
    public void CreateTable(IEnumerable<TValue> dataList, Func<TValue, TKey> keySelector)
    {
        Clear();

        foreach (var data in dataList)
        {
            var key = keySelector(data);
            Add(key, data);
        }
    }

    public bool ContainsKey(TKey key) => Map.ContainsKey(key);
    
    public void SwapOrder(TKey key1, TKey key2)
    {
        var index1 = keyList.IndexOf(key1);
        var index2 = keyList.IndexOf(key2);

        // Swap keys
        (keyList[index1], keyList[index2]) = (keyList[index2], keyList[index1]);

        // Swap values
        (valueList[index1], valueList[index2]) = (valueList[index2], valueList[index1]);
    }
    
    
#if UNITY_EDITOR
    [SerializeField] private bool hideKey;
#endif
}