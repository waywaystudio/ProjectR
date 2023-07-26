using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Runes.Tasks
{
    public class TimeLimit : TaskRune
    {
        private CancellationTokenSource cts;
        private float timer;

        // public override bool IsSuccess => Math.Abs(timer / Max.Value) < 1f;

        public TimeLimit(float limit)
        {
            Max.Value = limit;
        }

        public override void ActiveTask()
        {
            PlayTimer().Forget();
        }

        public override void Accomplish()
        {
            StopTimer();

            IsSuccess = timer < Max.Value;
        }

        public override void Defeat()
        {
            StopTimer();

            timer = 0f;
        }


        private async UniTaskVoid PlayTimer()
        {
            cts   =  new CancellationTokenSource();
            timer =  0f;
            timer += Time.deltaTime;

            while (!cts.IsCancellationRequested)
            {
                if (timer > Max.Value)
                {
                    StopTimer();
                    return;
                }
                
                await UniTask.Yield(cts.Token);
            }
        }

        private void StopTimer()
        {
            cts?.Cancel();
            cts = null;
        }
    }
}
