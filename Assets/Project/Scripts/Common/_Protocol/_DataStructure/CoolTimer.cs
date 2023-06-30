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

        public FloatEvent EventTimer { get; private set; } = new();
        public bool IsReady => !isRunning;
        public float CoolTime => coolTime * (Retriever is not null 
            ? CombatFormula.GetHasteValue(Retriever.Invoke()) 
            : 1f);

        private bool isRunning;
        private CancellationTokenSource cts;
        private Func<float> Retriever { get; set; }

        public void SetRetriever(Func<float> hasteRetriever) => Retriever = hasteRetriever;
        
        public void Play() => Play(CoolTime);
        public void Play(float duration) => Play(duration, null);
        public void Play(float duration, Action callback)
        {
            if (isRunning)
            {
                Debug.Log("Already Running in");
                return;
            }
            
            cts              = new CancellationTokenSource();
            EventTimer.Value = duration;
            
            AwaitCoolTime(callback, cts).Forget();
        }
        
        public void Dispose()
        {
            cts?.Cancel();
            cts = null;
        }
        
        public void AddListener(string key, Action<float> action) => EventTimer.AddListener(key, action);
        public void RemoveListener(string key) => EventTimer.RemoveListener(key);
        

        private void Stop()
        {
            cts?.Cancel();

            isRunning = false;
        }

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
