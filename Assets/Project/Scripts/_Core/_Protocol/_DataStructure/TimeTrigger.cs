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

    private bool isIncrease;
    private float timer;
    private CancellationTokenSource cts;

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
    
    private float Duration => duration * DurationWeight.Invoke();
    private Action Callback { get; set; }
    private ActionTable<float> OnValueChanged { get; } = new();
    private ValueWeight DurationWeight { get; set; }
    
    /// <summary>
    /// 생성자 함수.
    /// </summary>
    /// <param name="isIncrease">재생 시 timer 증감 방향 설정</param>
    /// <param name="weight">timer 계수 (미설정 == 1f)</param>
    public void Initialize(bool isIncrease = true, Func<float> weight = null, Action callback = null)
    {
        this.isIncrease = isIncrease;
        timer           = Duration;
        DurationWeight  = new ValueWeight(weight);
        Callback        = callback;
    }

    /// <summary>
    /// TimeTrigger 클래스 SerializeField의 duration 값을 바탕으로 돌아간다.
    /// DurationWeight 를 통해 계수를 설정할 수 있다. 
    /// </summary>
    public void Play()
    {
        TimeTicker(isIncrease).Forget();
    }


    public TimeTrigger AddListener(string key, Action<float> action) { OnValueChanged.Add(key, action); return this; }
    public TimeTrigger RemoveListener(string key) { OnValueChanged.Remove(key); return this; }
    public TimeTrigger ChangeWeight(Func<float> weight) { DurationWeight  = new ValueWeight(weight); return this; }
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

    
    private async UniTaskVoid TimeTicker(bool isIncrease)
    {
        if (IsRunning) return;
        
        IsRunning = true;
        cts       = new CancellationTokenSource();

        if (isIncrease)
        {
            Timer = 0f;

            while (Timer < Duration)
            {
                Timer += Time.deltaTime;
                
                await UniTask.Yield(cts.Token);
            }
        }
        else
        {
            Timer = Duration;

            while (Timer > 0f)
            {
                Timer -= Time.deltaTime;
            
                await UniTask.Yield(cts.Token);
            }
        }

        Callback?.Invoke();
        Stop();
    }
}
