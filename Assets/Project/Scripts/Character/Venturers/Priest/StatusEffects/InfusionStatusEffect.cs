using System.Threading;
using Common;
using Common.StatusEffects;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Venturers.Priest.StatusEffects
{
    public class InfusionStatusEffect : StatusEffect
    {
        [SerializeField] protected float interval = 2f;
        [SerializeField] private StatEntity speedUpStat;
        [SerializeField] private StatEntity criticalChanceStat;
        [SerializeField] private StatEntity hasteStat;
        
        private CancellationTokenSource cts;
        private float hasteWeight;
        private float tickBuffer;
        
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            Builder
                .Add(Section.Active, "SetHasteWeightAndTickBuffer", SetHasteWeightAndTickBuffer)
                .Add(Section.Active, "OvertimeExecution", () => OvertimeExecution().Forget())
                .Add(Section.Active, "ProvideBuff", ProvideBuff)
                .Add(Section.End, "StopEffectuate", Stop)
                .Add(Section.End, "RemoveBuffStat", EndBuff);
        }
        
        
        protected override void Dispose()
        {
            base.Dispose();

            Stop();
        }

        private void SetHasteWeightAndTickBuffer()
        {
            hasteWeight = tickBuffer =
                interval * CombatFormula.GetHasteValue(Provider.StatTable.Haste);
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
                    Invoker.Hit(Taker);
                    tickBuffer = hasteWeight;
                }

                await UniTask.Yield(cts.Token);
            }
            
            Invoker.Complete();
        }

        private void ProvideBuff()
        {
            Taker.StatTable.Add(speedUpStat);

            if (Provider.StatusEffectTable.ContainsKey(DataIndex.LightWeaverStatusEffect))
            {
                Taker.StatTable.Add(criticalChanceStat);
                Taker.StatTable.Add(hasteStat);
            }
        }

        private void EndBuff()
        {
            Taker.StatTable.Remove(speedUpStat);
            Taker.StatTable.Remove(criticalChanceStat);
            Taker.StatTable.Remove(hasteStat);
        }
        
        private void Stop()
        {
            cts?.Cancel();
            cts = null;
        }
    }
}
