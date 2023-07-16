using System;
using System.Collections.Generic;

public abstract class ActionTableCore<T>
{
    protected Dictionary<string, T> Table { get; } = new();

    public int Count => Table.Count;
    public void Remove(string key) => Table.TryRemove(key);
    public void Clear() => Table.Clear();
}

public class ActionTable : ActionTableCore<Action>
{
    public void Add(string key, Action action) => Table.TryAdd(key, action);

    /// <summary>
    /// otherTable을 참조 방식 등록.
    /// otherTable의 내용이 변경되면, 변경된 내용을 포함하여 실행한다.
    /// </summary>
    public void Register(string key, ActionTable otherTable) => Add(key, otherTable.Invoke);

    public void Invoke()
    {
        foreach (var action in Table.Values) action?.Invoke();
    }
}

    
public class ActionTable<T> : ActionTableCore<Action<T>>
{
    public void Add(string key, Action action) => Table.TryAdd(key, _ => action());
    public void Add(string key, Action<T> action) => Table.TryAdd(key, action);

    /// <summary>
    /// otherTable을 참조 방식 등록.
    /// otherTable의 내용이 변경되면, 변경된 내용을 포함하여 실행한다.
    /// </summary>
    public void Register(string key, ActionTable<T> otherTable)
    {
        Table.TryAdd(key, otherTable.Invoke);
    }

    public void Invoke(T value)
    {
        foreach (var action in Table.Values) action?.Invoke(value);
    }
}

    
public class ActionTable<T0, T1> : ActionTableCore<Action<T0, T1>>
{
    public void Add(string key, Action action) => Table.TryAdd(key, (_, _) => action());
    public void Add(string key, Action<T0> action) => Table.TryAdd(key, (x, _) => action(x));
    public void Add(string key, Action<T0, T1> action) => Table.TryAdd(key, action);

    public virtual void Invoke(T0 value0, T1 value1)
    {
        foreach (var action in Table.Values) action?.Invoke(value0, value1);
    }
}