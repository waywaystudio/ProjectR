using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common
{
    [Serializable]
    public class AwaitTimer
    {
        [SerializeField] private float coolTime;

        public bool IsReady => RemainCoolTime <= 0f;
        public float CoolTime => coolTime;
        public float RemainCoolTime => RemainTimer.Value;
        public FloatEvent RemainTimer { get; }

        private bool isPaused;
        private CancellationTokenSource cts;

        public AwaitTimer(float coolTime)
        {
            this.coolTime = coolTime;
            RemainTimer   = new FloatEvent(coolTime);
        }

        public void Play() => Play(null);
        public void Play(Action callback)
        {
            AwaitCoolTime(callback, cts).Forget();
        }

        public async UniTask PlayAwait()
        {
            cts = new CancellationTokenSource();
            
            try
            {
                await AwaitCoolTime(null, cts);
            }
            finally
            {
                cts?.Dispose();
            }
        }

        public void Stop()
        {
            cts?.Cancel();
            
            RemainTimer.Value = coolTime;
        }

        public void Pause() => isPaused = true;
        public void Resume() => isPaused = false;
        

        private async UniTask AwaitCoolTime(Action callback, CancellationTokenSource cts)
        {
            while (RemainTimer.Value > 0)
            {
                if (!isPaused)
                {
                    RemainTimer.Value -= Time.deltaTime;
                }

                await UniTask.Yield(PlayerLoopTiming.Update, cts.Token);
            }
            
            callback?.Invoke();
        }
    }
}
