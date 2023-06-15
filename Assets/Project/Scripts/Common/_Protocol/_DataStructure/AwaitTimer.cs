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
        public AwaitCoolTimer() : this(0f) { }
        public AwaitCoolTimer(float timer)
        {
            this.timer  = timer;
            RemainTimer = new FloatEvent();
        }
        
        public void Register(ISequencer sequencer, SectionType sectionType)
        {
            if (sectionType == SectionType.None) return;
            
            var section = sectionType switch
            {
                SectionType.Activation => sequencer.ActiveSection,
                SectionType.Cancel     => sequencer.CancelSection,
                SectionType.Complete     => sequencer.CompleteSection,
                SectionType.End     => sequencer.EndSection,
                _ => null,
            };
            
            section?.Add("CoolTimer", Play);
        }
    }
    
    [Serializable]
    public class AwaitTimer
    {
        [SerializeField] protected float timer;

        public bool IsReady => 
            !isRunning; 
            // RemainTime <= 0f;
        public float Timer { get => timer; set => timer = value;}
        public float RemainTime => RemainTimer.Value;
        public FloatEvent RemainTimer { get; set; }

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
            cts               = new CancellationTokenSource();
            RemainTimer.Value = timer;
            
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
            
            isRunning         = false;
            RemainTimer.Value = 0f;
        }

        public void Pause() => isPaused = true;
        public void Resume() => isPaused = false;

        private bool isRunning;

        private async UniTask AwaitCoolTime(Action callback, CancellationTokenSource cts)
        {
            if (isRunning)
            {
                Debug.Log("Already Running in");
                return;
            }
            
            isRunning = true;
            
            while (RemainTimer.Value > 0f)
            {
                if (!isPaused)
                {
                    RemainTimer.Value -= Time.deltaTime;

                    if (RemainTimer.Value <= 0)
                    {
                        callback?.Invoke();
                        Stop();
                        break;
                    }
                }

                await UniTask.Yield(PlayerLoopTiming.Update, cts.Token);
            }
        }
    }
}
