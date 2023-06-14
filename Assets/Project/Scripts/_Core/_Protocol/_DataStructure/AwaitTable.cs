using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

public class AwaitTable
{
    private readonly Dictionary<string, Action> actionTable = new();
    private readonly Dictionary<string, Func<CancellationToken, UniTask>> awaitTable = new();
    private CancellationTokenSource cts;
    
    public async UniTask Invoke(CancellationToken cancellationToken = default)
    {
        cts = new CancellationTokenSource();

        try
        {
            foreach (var action in actionTable.Values) action.Invoke();
            foreach (var action in awaitTable.Values)
            {
                if (cts.IsCancellationRequested) break;

                await action.Invoke(cts.Token);
            }
        }
        finally
        {
            cts?.Dispose();
        }
    }

    public void Cancel() => cts?.Cancel();

    /// <summary>
    /// otherTable을 값 방식으로 등록.
    /// 함수 실행 시점에 otherTable의 Action만 등록한다.
    /// </summary>
    public void AddAwait(string key, Func<CancellationToken, UniTask> action) => awaitTable.TryAdd(key, action);
    public void AddAwait(string key, Func<UniTask> action) => AddAwait(key, _ => action());
    public void Add(string key, Action action) => actionTable.TryAdd(key, action);

    /// <summary>
    /// otherTable을 참조 방식 등록.
    /// otherTable의 내용이 변경되면, 변경된 내용을 포함하여 실행한다.
    /// </summary>
    public void Register(string key, AwaitTable awaitTable) => AddAwait(key, awaitTable.Invoke);
    public void Register(string key, ActionTable actionTable) => Add(key, actionTable.Invoke);
    
    public void Remove(string key)
    {
        awaitTable.Remove(key);
        actionTable.TryRemove(key);
    }

    public void Clear()
    {
        awaitTable.Clear();
        actionTable.Clear();
    }
    
    public bool IsNullOrEmpty() => awaitTable.IsNullOrEmpty();
    public void IterateKey(Action<string> action)
    {
        foreach (var key in actionTable.Keys) action.Invoke(key);
        foreach (var key in awaitTable.Keys) action.Invoke(key);
    }
}

public class AwaitTable<T>
{
    private readonly Dictionary<string, Action<T>> actionTable = new();
    private readonly SortedDictionary<string, Func<T, CancellationToken, UniTask>> awaitTable = new();
    private CancellationTokenSource cts;
    
    public async UniTask Invoke(T value, CancellationToken cancellationToken = default)
    {
        cts = new CancellationTokenSource();

        try
        {
            foreach (var action in actionTable.Values) action.Invoke(value);
            foreach (var action in awaitTable.Values)
            {
                if (cts.IsCancellationRequested) break;

                await action.Invoke(value, cts.Token);
            }
        }
        finally
        {
            cts?.Dispose();
        }
    }

    public void Cancel() => cts?.Cancel();

    /// <summary>
    /// otherTable을 값 방식으로 등록.
    /// 함수 실행 시점에 otherTable의 Action만 등록한다.
    /// </summary>
    public void AddAwait(string key, Func<T, CancellationToken, UniTask> action) => awaitTable.TryAdd(key, action);
    public void AddAwait(string key, Func<T, UniTask> action) => AddAwait(key, (value, _) => action(value));
    public void AddAwait(string key, Func<UniTask> action) => AddAwait(key, (_, _) => action());
    public void Add(string key, Action action) => Add(key, _ => action?.Invoke());
    public void Add(string key, Action<T> action) => actionTable.TryAdd(key, action);

    /// <summary>
    /// otherTable을 참조 방식 등록.
    /// otherTable의 내용이 변경되면, 변경된 내용을 포함하여 실행한다.
    /// </summary>
    public void Register(string key, AwaitTable actionTable) => AddAwait(key, () => actionTable.Invoke());
    public void Register(string key, AwaitTable<T> actionTable) => AddAwait(key, actionTable.Invoke);
    public void Register(string key, ActionTable actionTable) => Add(key, actionTable.Invoke);
    public void Register(string key, ActionTable<T> actionTable) => Add(key, actionTable.Invoke);

    public void Remove(string key)
    {
        awaitTable.Remove(key);
        actionTable.TryRemove(key);
    }

    public void Clear()
    {
        awaitTable.Clear();
        actionTable.Clear();
    }
    
    public bool IsNullOrEmpty() => awaitTable.IsNullOrEmpty();
}

/*
 * Annotation
 * Example
 * actionTable.Register("myAsyncAction", async cancellationToken =>
 * {
 *     await UniTask.Delay(1000, cancellationToken: cancellationToken);
 *     Debug.Log("This is an asynchronous action");
 * });
 */
