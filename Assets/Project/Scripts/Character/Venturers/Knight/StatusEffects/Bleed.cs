using System.Threading;
using Common;
using Common.StatusEffects;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Venturers.Knight.StatusEffects
{
    public class Bleed : StatusEffect
    {
        [SerializeField] protected float interval;

        private CancellationTokenSource cts;
        private float hasteWeight;
        private float tickBuffer;

        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            Builder
                .Add(SectionType.Active, "SetHasteWeightAndTickBuffer", UpdateHasteWeight)
                .Add(SectionType.Active, "OvertimeExecution", () => OvertimeExecution().Forget())
                .Add(SectionType.End, "StopOverTimeExecution", Stop);
        }


        private void UpdateHasteWeight()
        {
            hasteWeight = tickBuffer = interval * CombatFormula.GetHasteValue(Provider.StatTable.Haste);
        }

        private async UniTaskVoid OvertimeExecution()
        {
            cts = new CancellationTokenSource();
            
            while (ProgressTime.Value > 0)
            {
                if (tickBuffer > 0f)
                {
                    ProgressTime.Value -= Time.deltaTime;
                    tickBuffer         -= Time.deltaTime;
                }
                else
                {
                    executor.ToTaker(Taker);
                    tickBuffer = hasteWeight;
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
