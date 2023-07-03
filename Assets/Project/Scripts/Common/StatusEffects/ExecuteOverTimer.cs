using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.StatusEffects
{
    [Serializable]
    public class ExecuteOverTimer
    {
        [SerializeField] private float duration;
        [SerializeField] private int tickCount;
        
        private CancellationTokenSource cts;
        private float hasteWeight;
        private float tickBuffer;
        private Action executeAction;
        private Action completeAction;

        public FloatEvent Progress { get; private set; } = new();

        public void Initialize(Action execute, Action complete)
        {
            executeAction  = execute;
            completeAction = complete;
        }
        
        private async UniTaskVoid OvertimeExecution()
        {
            cts = new CancellationTokenSource();
            
            while (Progress.Value > 0)
            {
                if (tickBuffer > 0f)
                {
                    Progress.Value -= Time.deltaTime;
                    tickBuffer     -= Time.deltaTime;
                }
                else
                {
                    executeAction?.Invoke();
                    tickBuffer = hasteWeight;
                }
            
                await UniTask.Yield(cts.Token);
            }
            
            completeAction?.Invoke();
        }
        
        private void Stop()
        {
            cts?.Cancel();
            cts = null;
        }
    }
}
