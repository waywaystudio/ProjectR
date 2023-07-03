using System.Threading;
using Common;
using Common.StatusEffects;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Venturers.Priest.StatusEffects
{
    public class LightWeaverStatusEffect : StatusEffect
    {
        private CancellationTokenSource cts;
        
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            Builder
                .Add(SectionType.Active, "OvertimeExecution", () => OvertimeExecution().Forget())
                .Add(SectionType.End, "Stop", Stop);
        }
        
        
        private async UniTaskVoid OvertimeExecution()
        {
            cts = new CancellationTokenSource();

            while (ProgressTime.Value > 0)
            {
                ProgressTime.Value -= Time.deltaTime;

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
