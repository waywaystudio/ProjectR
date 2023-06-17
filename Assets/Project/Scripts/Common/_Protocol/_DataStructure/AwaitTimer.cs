using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common
{
    [Serializable]
    public class AwaitTimer
    {
        [SerializeField] protected float timer;

        public bool IsReady => !isRunning;
        public float Timer { get => timer; set => timer = value;}
        public FloatEvent EventTimer { get; private set; } = new();

        protected Action EndCallback;
        private bool isRunning;
        private bool isIncrease;
        private CancellationTokenSource cts;

        public AwaitTimer() : this(0f, null) { }
        public AwaitTimer(float timer, Action endCallback) : this(false, timer, endCallback) { }
        public AwaitTimer(bool isIncrease = false, float timer = 0f, Action endCallback = null)
        {
            this.isIncrease  = isIncrease;
            this.timer       = timer;
            
            EndCallback = endCallback;
        }
        

        public void Play() => Play(timer);
        public void Play(float duration) => Play(duration, EndCallback);
        public void Play(float duration, Action callback)
        {
            if (duration == 0) return;
            
            cts = new CancellationTokenSource();
            
            if (isIncrease)
            {
                timer = 0;
            }
            else
            {
                timer = duration;
            }

            EndCallback       = callback;
            EventTimer.Value = timer;
            
            AwaitCoolTime(duration, cts).Forget();
        }

        public void Stop()
        {
            cts?.Cancel();
            
            isRunning        = false;
            EventTimer.Value = 0f;
        }

        public void Dispose()
        {
            cts?.Cancel();
            cts = null;
        }

        public void AddListener(string key, Action<float> action) => EventTimer.AddListener(key, action);
        public void RemoveListener(string key) => EventTimer.RemoveListener(key);
        

        private async UniTask AwaitCoolTime(float duration, CancellationTokenSource cts)
        {
            if (isRunning)
            {
                Debug.Log("Already Running in");
                return;
            }
            
            isRunning = true;

            if (isIncrease)
            {
                if (timer >= duration) return;
                
                while (EventTimer.Value < duration)
                {
                    EventTimer.Value += Time.deltaTime;

                    if (EventTimer.Value >= duration)
                    {
                        EndCallback?.Invoke();
                        break;
                    }

                    await UniTask.Yield(cts.Token);
                }
            }
            else
            {
                if (timer == 0.0f) return;
                
                while (EventTimer.Value > 0f)
                {
                    EventTimer.Value -= Time.deltaTime;

                    if (EventTimer.Value <= 0)
                    {
                        EndCallback?.Invoke();
                        break;
                    }

                    await UniTask.Yield(cts.Token);
                }
            }

            Stop();
        }
    }
}
