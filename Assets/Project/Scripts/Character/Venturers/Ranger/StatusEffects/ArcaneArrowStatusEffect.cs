using System.Threading;
using Common;
using Common.StatusEffects;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Venturers.Ranger.StatusEffects
{
    public class ArcaneArrowStatusEffect : StatusEffect
    {
        private CancellationTokenSource cts;

        public int Stack { get; private set; } = 1;

        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            Builder
                .Add(Section.Active, "Effectuate", () => OvertimeExecution().Forget())
                .Add(Section.Override, "StackUp", StackUp)
                .Add(Section.End, "Stop", Stop);
        }
        
        
        protected override void Dispose()
        {
            base.Dispose();

            Stop();
        }

        private void StackUp()
        {
            if (Stack >= 6) return;
            Stack++;
        }

        private async UniTaskVoid OvertimeExecution()
        {
            cts = new CancellationTokenSource();
            
            while (true)
            {
                ProgressTime.Value -= Time.deltaTime;

                if (ProgressTime.Value <= 0f)
                {
                    break;
                }

                await UniTask.Yield(cts.Token);
            }
            
            Invoker.Complete();
        }
        
        private void Stop()
        {
            cts?.Cancel();
            cts = null;
        }
    }
}
