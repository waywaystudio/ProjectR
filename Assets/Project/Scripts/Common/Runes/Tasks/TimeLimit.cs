using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Runes.Tasks
{
    public class TimeLimit : TaskRune
    {
        private CancellationTokenSource cts;

        public TimeLimit(float limit)
        {
            Max = limit;
        }

        public override void ActiveTask()
        {
            PlayTimer().Forget();
        }

        public override void Accomplish()
        {
            StopTimer();

            IsSuccess = Progress.Value >= 0f;
        }

        public override void Defeat()
        {
            StopTimer();

            Progress.Value = Max;
        }


        private async UniTaskVoid PlayTimer()
        {
            cts            = new CancellationTokenSource();
            Progress.Value = Max;

            while (!cts.IsCancellationRequested)
            {
                Progress.Value -= Time.deltaTime;

                if (Progress.Value <= 0f)
                {
                    StopTimer();
                    Progress.Value = 0f;
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
