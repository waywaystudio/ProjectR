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
        }
        
        public void Register(ISequencer sequencer, SectionType sectionType)
        {
            if (sectionType == SectionType.None) return;
            
            var action = sectionType switch
            {
                SectionType.Activation => sequencer.ActiveAction,
                SectionType.Cancel     => sequencer.CancelAction,
                SectionType.Complete   => sequencer.CompleteAction,
                SectionType.End        => sequencer.EndAction,
                _                      => null,
            };
            
            action?.Add("CoolTimer", Play);
        }
    }
    
    [Serializable]
    public class AwaitTimer
    {
        [SerializeField] protected float timer;

        public bool IsReady => !isRunning;
        public float Timer { get => timer; set => timer = value;}
        public float RemainTime => RemainTimer.Value;
        public FloatEvent RemainTimer { get; private set; } = new();

        private bool isPaused;
        private bool isRunning;
        private Action endCallback;
        private CancellationTokenSource cts;

        public AwaitTimer() : this(0f) { }
        public AwaitTimer(Action endCallback) : this(0f, endCallback) { }
        public AwaitTimer(float timer, Action endCallback = null)
        {
            this.timer       = timer;
            this.endCallback = endCallback;
        }
        

        public void Play() => Play(null);
        public void Play(Action callback) => Play(timer, callback);
        public void Play(float duration) => Play(duration, null);
        public void Play(float duration, Action callback)
        {
            cts = new CancellationTokenSource();
            
            timer             = duration;
            endCallback       = callback;
            RemainTimer.Value = timer;
            
            AwaitCoolTime(cts).Forget();
        }

        public void Stop()
        {
            cts?.Cancel();
            
            isRunning         = false;
            RemainTimer.Value = 0f;
        }

        public void Pause() => isPaused = true;
        public void Resume() => isPaused = false;

        public void AddListener(Action action) => RemainTimer.AddListener(action);
        public void AddListener(Action<float> action) => RemainTimer.AddListener(action);
        public void AddListener(string key, Action action) => RemainTimer.AddListener(key, action);
        public void AddListener(string key, Action<float> action) => RemainTimer.AddListener(key, action);
        public void RemoveListener(string key) => RemainTimer.RemoveListener(key);
        

        private async UniTask AwaitCoolTime(CancellationTokenSource cts)
        {
            if (isRunning)
            {
                Debug.Log("Already Running in");
                return;
            }

            if (timer == 0.0f) return;
            
            isRunning = true;
            
            while (RemainTimer.Value > 0f)
            {
                if (!isPaused)
                {
                    RemainTimer.Value -= Time.deltaTime;

                    if (RemainTimer.Value <= 0)
                    {
                        endCallback?.Invoke();
                        break;
                    }
                }

                await UniTask.Yield(PlayerLoopTiming.Update, cts.Token);
            }
            
            Stop();
        }
    }
}
