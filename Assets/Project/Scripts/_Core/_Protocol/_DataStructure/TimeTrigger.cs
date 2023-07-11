using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class InstanceTimer
{
    private CancellationTokenSource cts;

    public void Play(float duration, Action callback)
    {
        TimeTicker(duration, callback).Forget();
    }
    
    public void Stop()
    {
        cts?.Cancel();
        cts = null;
    }
    
    private async UniTaskVoid TimeTicker(float duration, Action callback)
    {
        cts = new CancellationTokenSource();

        var timer = duration;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            
            await UniTask.Yield(cts.Token);
        }
        
        callback?.Invoke();
        Stop();
    }
}

[Serializable]
public class TimeTrigger
{
    [SerializeField] private float duration;

    private float timer;
    private CancellationTokenSource cts;

    private float Duration => duration * Weight.Invoke();
    private Func<float> Weight { get; set; }
    private Action Callback { get; set; }
    private ActionTable<float> OnValueChanged { get; } = new();

    public bool IsRunning { get; private set; }
    public float Progress => Mathf.Clamp01(Timer / Duration);
    public float Timer
    {
        get => timer;
        set
        {
            timer = value;
            OnValueChanged.Invoke(value);
        }
    }
    

    public void Initialize(Func<float> weight = null, Action callback = null)
    {
        timer    = Duration;
        Weight   = weight is null ? () => 1f : weight;
        Callback = callback;
    }

    public void Play()
    {
        TimeTicker().Forget();
    }

    public TimeTrigger AddListener(string key, Action<float> action) { OnValueChanged.Add(key, action); return this; }
    public TimeTrigger RemoveListener(string key) { OnValueChanged.Remove(key); return this; }
    public TimeTrigger ChangeWeight(Func<float> weight) { Weight  = weight; return this; }
    public TimeTrigger ChangeCallback(Action callback) { Callback = callback; return this; }
    
    public void Stop()
    {
        cts?.Cancel();
        
        cts       = null;
        IsRunning = false;
    }

    public void Dispose()
    {
        Stop();
        OnValueChanged.Clear();
    }
    
    
    private async UniTaskVoid TimeTicker()
    {
        IsRunning = true;
        cts       = new CancellationTokenSource();
        Timer     = Duration;

        while (Timer > 0f)
        {
            Timer -= Time.deltaTime;
            
            await UniTask.Yield(cts.Token);
        }
        
        Callback?.Invoke();
        Stop();
    }
}
