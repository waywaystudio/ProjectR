using System;
using System.Collections.Generic;

public abstract class ActionTableCore<T> : Dictionary<string, T>
{
    protected ActionTableCore() { }
    protected ActionTableCore(int capacity) : base(capacity) { }

    public abstract void Register(string key, Action action);
    public void Unregister(string key) => this.TryRemove(key);
        
    public void Debug()
    {
#if UNITY_EDITOR
        Register("__Debug__", () => UnityEngine.Debug.Log("ActionTable Debug"));
#endif
    }
}

public class ActionTable : ActionTableCore<Action>
{
    public ActionTable() { }
    public ActionTable(int capacity) : base(capacity) { }
    
    public override void Register(string key, Action action) => TryAdd(key, action);

    /// <summary>
    /// otherTable을 참조 방식 등록.
    /// otherTable의 내용이 변경되면, 변경된 내용을 포함하여 실행한다.
    /// </summary>
    public void Register(string key, ActionTable otherTable)
    {
        Register(key, otherTable.Invoke);
    }

    /// <summary>
    /// otherTable을 값 방식으로 등록.
    /// 함수 실행 시점에 otherTable의 Action만 등록한다.
    /// </summary>
    public void Register(ActionTable otherTable)
    {
        foreach (var item in otherTable) TryAdd(item.Key, item.Value);
    }

    public void Invoke()  
    {
        foreach (var action in Values) action?.Invoke();
    }
}

    
public class ActionTable<T> : ActionTableCore<Action<T>>
{
    public ActionTable(){}
    public ActionTable(int capacity) : base(capacity) {}

    public override void Register(string key, Action action) => TryAdd(key, _ => action());
    public void Register(string key, Action<T> action) => TryAdd(key, action);

    public void Register(ActionTable otherTable)
    {
        foreach (var item in otherTable) TryAdd(item.Key, _ => item.Value.Invoke());
    }

    public void Register(ActionTable<T> otherTable)
    {
        foreach (var item in otherTable) TryAdd(item.Key, item.Value.Invoke);
    }

    public void Invoke(T value)
    {
        foreach (var action in Values) action?.Invoke(value);
    }
}

    
public class ActionTable<T0, T1> : ActionTableCore<Action<T0, T1>>
{
    public ActionTable(){}
    public ActionTable(int capacity) : base(capacity) {}

    public override void Register(string key, Action action) => TryAdd(key, (_, _) => action());
    public void Register(string key, Action<T0> action) => TryAdd(key, (x, _) => action(x));
    public void Register(string key, Action<T0, T1> action) => TryAdd(key, action);
    
    public void Register(ActionTable otherTable)
    {
        foreach (var item in otherTable) TryAdd(item.Key, (_, _) => item.Value.Invoke());
    }

    public void Register(ActionTable<T0> otherTable)
    {
        foreach (var item in otherTable) TryAdd(item.Key, (x, _) => item.Value.Invoke(x));
    } 
    
    public void Register(ActionTable<T0, T1> otherTable)
    {
        foreach (var item in otherTable) TryAdd(item.Key, item.Value.Invoke);
    }

    public void Invoke(T0 value0, T1 value1)
    {
        foreach (var action in Values) action?.Invoke(value0, value1);
    }
}