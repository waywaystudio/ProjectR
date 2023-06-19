using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common
{
    [Serializable] 
    public class CastTimer
    {
        [SerializeField] protected float castingTime;
        [SerializeField] protected SectionType callbackSection;

        public float CastingTime { get => castingTime; set => castingTime = value;}
        public SectionType CallbackSection => callbackSection;
        public FloatEvent EventTimer { get; private set; } = new();

        private Action endCallback;
        private bool isRunning;
        private CancellationTokenSource cts;
    
        public void Play() => Play(castingTime);
        public void Play(float duration) => Play(duration, endCallback);
        public void Play(float duration, Action callback)
        {
            if (duration == 0) return;
        
            cts = new CancellationTokenSource();

            castingTime      = duration;
            EventTimer.Value = 0f;
            endCallback      = callback;
        
            AwaitCoolTime(cts).Forget();
        }

        public void Stop()
        {
            cts?.Cancel();

            isRunning = false;
        }

        public void Dispose()
        {
            cts?.Cancel();
            cts = null;
        }

        public void AddListener(string key, Action<float> action) => EventTimer.AddListener(key, action);
        public void RemoveListener(string key) => EventTimer.RemoveListener(key);
    

        private async UniTask AwaitCoolTime(CancellationTokenSource cts)
        {
            if (isRunning)
            {
                Debug.Log("Already Running in");
                return;
            }
        
            isRunning = true;
            
            while (EventTimer.Value < castingTime)
            {
                EventTimer.Value += Time.deltaTime;

                if (EventTimer.Value >= castingTime)
                {
                    endCallback?.Invoke();
                    break;
                }

                await UniTask.Yield(cts.Token);
            }

            Stop();
        }
    }
}
