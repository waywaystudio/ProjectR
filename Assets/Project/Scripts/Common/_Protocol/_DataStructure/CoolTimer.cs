using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common
{
    [Serializable] 
    public class CoolTimer
    {
        [SerializeField] protected float coolTime;
        [SerializeField] protected SectionType invokeSection;
        
        public bool IsReady => !isRunning;
        public float CoolTime => coolTime;
        public SectionType InvokeSection => invokeSection;
        public FloatEvent EventTimer { get; private set; } = new();

        private bool isRunning;
        private CancellationTokenSource cts;
        
        public void Play() => Play(coolTime);
        public void Play(float duration) => Play(duration, null);
        public void Play(float duration, Action callback)
        {
            if (isRunning)
            {
                Debug.Log("Already Running in");
                return;
            }
            
            cts              = new CancellationTokenSource();
            coolTime         = duration;
            EventTimer.Value = coolTime;
            
            AwaitCoolTime(callback, cts).Forget();
        }
        
        public void Dispose()
        {
            cts?.Cancel();
            cts = null;
        }

        private void Stop()
        {
            cts?.Cancel();

            isRunning = false;
        }

        public void AddListener(string key, Action<float> action) => EventTimer.AddListener(key, action);
        public void RemoveListener(string key) => EventTimer.RemoveListener(key);
        

        private async UniTask AwaitCoolTime(Action callback, CancellationTokenSource cts)
        {
            isRunning = true;

            while (EventTimer.Value > 0f)
            {
                EventTimer.Value -= Time.deltaTime;

                if (EventTimer.Value <= 0)
                {
                    callback?.Invoke();
                    break;
                }

                await UniTask.Yield(cts.Token);
            }

            Stop();
        }
    }
}
