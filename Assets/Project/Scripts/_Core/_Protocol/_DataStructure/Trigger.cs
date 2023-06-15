using System;
using System.Threading;
using Cysharp.Threading.Tasks;


public class WaitTrigger
{
    public WaitTrigger(Action onComplete, Func<bool> condition)
    {
        this.onComplete = onComplete;
        this.condition  = condition;
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

public class TimeTrigger
{
    public TimeTrigger(Action onComplete, float delaySecond)
    {
        this.onComplete  = onComplete;
        this.delaySecond = delaySecond;

        onValueChanged = new ActionTable<float>();
    }

    private readonly float delaySecond;
    private readonly ActionTable<float> onValueChanged;
    private readonly Action onComplete;
    private CancellationTokenSource cts;
    
    public void Pull()
    {
        cts = new CancellationTokenSource();
        
        Fire(cts.Token).Forget();
    }
    
    public void Dispose()
    {
        cts?.Cancel();
        cts = null;
    }

    public void AddListener(string key, Action<float> action) => onValueChanged.Add(key, action);
    public void RemoveListener(string key) => onValueChanged.Unregister(key);
    

    private async UniTaskVoid Fire(CancellationToken cancellationToken)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delaySecond), cancellationToken: cancellationToken);
        
        if (!cancellationToken.IsCancellationRequested)
        {
            onComplete?.Invoke();
        }
    }
}
