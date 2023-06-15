using System;
using System.Threading;
using Cysharp.Threading.Tasks;

[Serializable]
public class AwaitEventTable : Table<string, Func<CancellationToken,UniTask>>
{
    private CancellationTokenSource cts;

    public bool IsDone { get; private set; }
    public void Cancel() => cts?.Cancel();
    
    public async UniTask Invoke()
    {
        cts = new CancellationTokenSource();
        
        try
        {
            IsDone = false;
            
            foreach (var func in valueList)
            {
                if (func == null) continue;

                await func.Invoke(cts.Token);
            }
        }
        finally
        {
            cts?.Dispose();
        }
        
        IsDone = true;
    }
}

[Serializable]
public class AwaitEventTable<T> : Table<string, Func<T, CancellationToken,UniTask>>
{
    private CancellationTokenSource cts;
    
    public bool IsDone { get; private set; }
    public void Cancel() => cts?.Cancel();
    
    public async UniTask Invoke(T value)
    {
        cts = new CancellationTokenSource();
        
        try
        {
            IsDone = false;
            
            foreach (var func in valueList)
            {
                if (func == null) continue;

                await func.Invoke(value, cts.Token);
            }
        }
        finally
        {
            cts?.Dispose();
        }
        
        IsDone = true;
    }
}

[Serializable]
public class AwaitEventTable<T0, T1> : Table<string, Func<T0, T1, CancellationToken,UniTask>>
{
    private CancellationTokenSource cts;
    
    public bool IsDone { get; private set; }
    public void Cancel() => cts?.Cancel();
    
    public async UniTask Invoke(T0 value1, T1 value2)
    {
        cts = new CancellationTokenSource();
        
        try
        {
            IsDone = false;
            
            foreach (var func in valueList)
            {
                if (func == null) continue;

                await func.Invoke(value1, value2, cts.Token);
            }
        }
        finally
        {
            cts?.Dispose();
        }
        
        IsDone = true;
    }
}