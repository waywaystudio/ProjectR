using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Common
{
    [Serializable] 
    public class AwaitCoolTimer : AwaitTimer
    {
        public void Register(ISequencer sequencer, SectionType sectionType)
        {
            if (sectionType == SectionType.None) return;
            
            var section = sectionType switch
            {
                SectionType.Activation => sequencer.Activation,
                SectionType.Cancel     => sequencer.Cancellation,
                SectionType.Complete     => sequencer.Complete,
                SectionType.End     => sequencer.End,
                _ => null,
            };
            
            section?.Add("CoolTimer", Play);
        }
    }
    
    [Serializable]
    public class AwaitTimer
    {
        [SerializeField] private float timer;

        public Action Action;

        public bool IsReady => RemainTime <= 0f;
        public float Timer { get => timer; set => timer = value;}
        public float RemainTime => RemainTimer.Value;
        
        [Sirenix.OdinInspector.ShowInInspector]
        public FloatEvent RemainTimer { get; }

        private bool isPaused;
        private CancellationTokenSource cts;

        public AwaitTimer() : this(0f) { }
        public AwaitTimer(float timer)
        {
            this.timer  = timer;
            RemainTimer = new FloatEvent();
        }

        public void Play() => Play(null);
        public void Play(Action callback)
        {
            cts               = new CancellationTokenSource();
            RemainTimer.Value = timer;
            
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
            
            RemainTimer.Value = timer;
        }

        public void Pause() => isPaused = true;
        public void Resume() => isPaused = false;
        

        private async UniTask AwaitCoolTime(Action callback, CancellationTokenSource cts)
        {
            while (RemainTimer.Value > 0f)
            {
                if (!isPaused)
                {
                    Action?.Invoke();
                    RemainTimer.Value -= Time.deltaTime;
                }

                await UniTask.Yield(PlayerLoopTiming.Update, cts.Token);
            }

            RemainTimer.Value = 0f;
            
            callback?.Invoke();
        }
    }
}
