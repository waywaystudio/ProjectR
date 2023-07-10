using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class WaitTrigger
{
    public WaitTrigger(Func<bool> condition, Action onComplete)
    {
        this.condition  = condition;
        this.onComplete = onComplete;
    }

    private CancellationTokenSource cts; 
    private readonly Action onComplete;
    private readonly Func<bool> condition;
    
    public void Pull()
    {
        cts = new CancellationTokenSource();
        
        Fire(cts.Token).Forget();
    }

    public void Cancel()
    {
        cts?.Cancel();
    }

    public void Dispose()
    {
        cts?.Cancel();
        cts = null;
    }
    

    private async UniTaskVoid Fire(CancellationToken cancellationToken)
    {
        await UniTask.WaitUntil(condition, cancellationToken: cancellationToken);
        
        if (!cancellationToken.IsCancellationRequested)
        {
            onComplete?.Invoke();
        }
    } 
}

[Serializable]
public class TimeTrigger
{
    [SerializeField] protected float delaySecond;

    private CancellationTokenSource cts;

    public Observable<float> Timer { get; } = new();
    public bool IsPulled { get; private set; } = true;
    public float DelaySecond => delaySecond;


    public void Pull()
    {
        cts         = new CancellationTokenSource();
        Timer.Value = delaySecond;
        IsPulled    = false;
        
        Fire(cts.Token).Forget();
    }

    public void Stop()
    {
        IsPulled = true;
        
        cts?.Cancel();
        cts = null;
    }

    public void Dispose()
    {
        Stop();
    }

    public void AddListener(string key, Action<float> action) => Timer.AddListener(key, action);
    public void RemoveListener(string key) => Timer.RemoveListener(key);
    

    private async UniTaskVoid Fire(CancellationToken ct)
    {
        while (Timer.Value > 0)
        {
            Timer.Value -= Time.deltaTime;

            await UniTask.Yield(ct);
        }

        Stop();
    }
}
